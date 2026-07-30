using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch]
internal class DeckInitializationPatch
{
    [HarmonyPatch(typeof(ModelDb), "InitIds")]
    [HarmonyPrefix]
    private static void LatePostInit()
    {
        StartingInventoryManager.LoadDefaultInventoryForAllCharacters();

        // Load Some Examples
        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.First(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.First(), "example")
            {
                Cards = [ModelDb.Card<Anger>()]
            });
    }
}