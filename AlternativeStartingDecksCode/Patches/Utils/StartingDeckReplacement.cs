using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch(nameof(Player), nameof(Player.PopulateStartingInventory))]
public class StartingDeckReplacement
{
    public static bool Prefix(Player __instance)
    {
        // Copied from base since we aren't calling 'em
        // Surely this causes *no* problems
        if (__instance.IsInventoryPopulated)
            throw new InvalidOperationException("Inventory is already populated.");
        if (!(__instance.RunState is NullRunState))
            throw new InvalidOperationException(
                "A player's starting inventory must be populated before being added to a run.");

        try
        {
            var startingInventory = AlternativeStartingDecksGlobals.StartingInventory;
            if (startingInventory == null) return false;
            // TODO: Maybe check for all nulls?
            ReplaceDeck(__instance, startingInventory.Cards);
            ReplaceRelics(__instance, startingInventory.Relics);
            ReplacePotions(__instance, startingInventory.Potions);

            // Don't run the original Method
            return false;
        }
        catch (Exception ex)
        {
            AlternativeStartingDecksLogger.Warn("Failed to replace deck: " + ex.InnerException);
            __instance.Deck.Clear();
            __instance._potionSlots.ForEach(pot => pot?.Discard());
            // Probably major problems here
            __instance._relics.Clear();
            // Just run the original method and hope we didn't kill it
            return true;
        }
    }


    private static void ReplaceDeck(Player player, IEnumerable<CardModel> cards)
    {
        var startingDeck = new List<CardModel>();

        foreach (var card in cards)
        {
            var mutable = card.ToMutable();
            mutable.FloorAddedToDeck = 1;
            startingDeck.Add(mutable);
        }

        player.PopulateDeck(startingDeck);
    }

    private static void ReplaceRelics(Player player, IEnumerable<RelicModel> relics)
    {
        var list = relics.Select(r => r.ToMutable()).ToList();
        foreach (var relic in list)
        {
            relic.FloorAddedToDeck = 1;
            SaveManager.Instance.MarkRelicAsSeen(relic);
        }

        player.PopulateRelics(list);
    }

    private static void ReplacePotions(Player player, IEnumerable<PotionModel> potions)
    {
        foreach (var r in potions)
        {
            r.ToMutable();
            player.AddPotionInternal(r);
        }
    }
}