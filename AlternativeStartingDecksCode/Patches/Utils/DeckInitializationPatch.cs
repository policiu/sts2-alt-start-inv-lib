using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch]
internal class DeckInitializationPatch
{
    [HarmonyPatch(typeof(ModelDb), "InitIds")]
    [HarmonyPrefix]
    private static void LatePostInit()
    {
        StartingInventoryManager.LoadDefaultInventoryForAllCharacters();
    }
}