using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

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
        var cards = new List<CardModel>();
        // Test by only returning a random deck
        IEnumerable<CardModel> ncards =
        [
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>(),
            ModelDb.Card<StrikeNecrobinder>()
        ];

        foreach (var card in ncards)
        {
            var mutable = card.ToMutable();
            mutable.FloorAddedToDeck = 1;
            cards.Add(mutable);
        }

        player.PopulateDeck(cards);
    }
}