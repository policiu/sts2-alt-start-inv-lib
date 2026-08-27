using AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;

[HarmonyPatch]
internal class DeckInitializationPatch
{
    [HarmonyPatch(typeof(ModelDb), "InitIds")]
    [HarmonyPostfix]
    private static void LatePostInit()
    {
        StartingInventoryManager.LoadAllInventories();
    }
}