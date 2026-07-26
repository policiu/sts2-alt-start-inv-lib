using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch(typeof(Ironclad), nameof(Ironclad.StartingDeck), MethodType.Getter)]
public class ReplaceIroncladStarterCardsExample
{
    public static IEnumerable<CardModel> Postfix(IEnumerable<CardModel> __result)
    {
        return InjectDifferentCards(__result);
    }


    private static IEnumerable<CardModel> InjectDifferentCards(IEnumerable<CardModel> _)
    {
        return
        [
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>(),
            ModelDb.Card<StrikeDefect>()
        ];
    }
}