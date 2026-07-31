using AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;

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