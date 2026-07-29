using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

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


    private static void ReplaceStartingDeck(Player player)
    {
        var startingDeck = new List<CardModel>();
        var startingInventory = StartingInventoryManager.GetStartingInventoriesForCharacter(player.Character);
        var cards = startingInventory[0].Cards;

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