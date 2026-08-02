using AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Random;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Multiplayer.Game.Lobby;

public class StartRunLobbyPatch
{
    [HarmonyPatch(typeof(StartRunLobby), nameof(StartRunLobby.BeginRunLocally))]
    public class BeginRunLocallyPatch
    {
        [HarmonyPrefix]
        public static void Prefix(StartRunLobby __instance, ref BeginRunLocallyState __state)
        {
            __state = new BeginRunLocallyState();
            __state.IsRandom = __instance.LocalPlayer.character is RandomCharacter;
        }

        [HarmonyPostfix]
        public static void Postfix(StartRunLobby __instance, ref BeginRunLocallyState __state, string seed)
        {
            if (__state.IsRandom)
            {
                AlternativeStartingInventoryLogger.Info(
                    "Local Player has selected a random character. Selecting a deck...");
                var rng = new Rng((uint)StringHelper.GetDeterministicHashCode(seed),
                    AlternativeStartingInventoryLib.ModId + "-random-deck");

                AlternativeStartingInventoryGlobals.StartingInventory =
                    StartingInventoryManager.GetStartingInventoriesForCharacter(__instance.LocalPlayer
                        .character).Where(inv => !inv.IsLocked).TakeRandom(1, rng).FirstOrDefault();

                AlternativeStartingInventoryLogger.Info(
                    AlternativeStartingInventoryGlobals.StartingInventory != null
                        ? $"Selected: {AlternativeStartingInventoryGlobals.StartingInventory.Name}"
                        : "No deck found. Defaulting to standard.");
            }
        }

        public struct BeginRunLocallyState
        {
            public bool IsRandom;
        }
    }
}