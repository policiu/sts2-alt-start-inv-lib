using BaseLib.Config;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch(nameof(Player), nameof(Player.PopulateStartingDeck))]
public class StartingDeckReplacement
{
    public static bool Prefix(Player __instance)
    {
        try
        {
            ReplaceStartingDeck(__instance);
        }
        catch
        {
            // Just run the original method and hope we didn't kill it
            return true;
        }

        // Don't run the original Method
        return false;
    }

    private static AlternativeStartingInventory GetStartingInventory(Player player)
    {
        var found = AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter.TryGetValue(
            player.Character.GetType().Name, out var startingInventory);

        if (found && startingInventory?.Count > 0)
            return startingInventory.First().Value;

        // If we don't find anything. Panic!
        // Later, just return the default items
        ModConfig.ModConfigLogger.Warn(
            $"Failed to find {player.Character.GetType().Name} in {nameof(StartingDeckReplacement)}");
        return new AlternativeStartingInventory([
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>()
        ], potions: [ModelDb.Potion<VulnerablePotion>()], relics: [ModelDb.Relic<Anchor>()]);
    }

    private static void ReplaceStartingDeck(Player player)
    {
        var startingDeck = new List<CardModel>();
        var startingInventory = GetStartingInventory(player);
        var (
            cards, _, _
            ) = startingInventory;

        // Fallback

        foreach (var card in cards)
        {
            var mutable = card.ToMutable();
            mutable.FloorAddedToDeck = 1;
            startingDeck.Add(mutable);
        }

        player.PopulateDeck(startingDeck);
    }
}