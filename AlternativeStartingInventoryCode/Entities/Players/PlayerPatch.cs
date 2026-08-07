using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Entities.Players;

[HarmonyPatch]
public class PlayerPatch
{
    /// <summary>
    ///     Fix bug in <see cref="Player.SetMaxPotionCountInternal" /> where we can get a -1
    ///     when looking for an empty slot
    /// </summary>
    [HarmonyPatch(typeof(Player), nameof(Player.SetMaxPotionCountInternal))]
    public class SetMaxPotionCountInternalPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Player __instance, int newMaxPotionCount)
        {
            if (newMaxPotionCount >= __instance._potionSlots.Count || newMaxPotionCount < 0) return true;

            try
            {
                AlternativeStartingInventoryLogger.Info("Injecting Player.SetMaxPotionCountInternal");
                Inject(__instance, newMaxPotionCount);
                AlternativeStartingInventoryLogger.Info("Bypassing Player.SetMaxPotionCountInternal");
                return false;
            }
            catch (Exception ex)
            {
                AlternativeStartingInventoryLogger.Error(
                    $"Unable to Inject Player.SetMaxPotionCountInternal Patch:\n{ex}", false);
                return true;
            }
        }

        private static void Inject(Player player, int newMaxPotionCount)
        {
            for (var index1 = player._potionSlots.Count - 1; index1 >= newMaxPotionCount; index1--)
            {
                if (index1 < 0) break;

                var index2 = player._potionSlots.IndexOf(null);
                // This line fixes the bug. Probably.
                if (index2 != -1 && index2 < newMaxPotionCount)
                    player._potionSlots[index2] = player._potionSlots[index1];
                else
                    player.DiscardPotionInternal(player._potionSlots[index1]);
                player._potionSlots.RemoveAt(index1);
            }

            // From Google
            var eventField = AccessTools.Field(typeof(Player), nameof(Player.MaxPotionCountChanged));
            var eventDelegate = eventField.GetValue(player) as Action<int>;

            eventDelegate?.Invoke(player.MaxPotionCount);
        }
    }
}