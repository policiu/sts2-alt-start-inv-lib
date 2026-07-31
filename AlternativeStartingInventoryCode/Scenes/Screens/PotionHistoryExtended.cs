using AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Scenes.Screens;

public partial class PotionHistoryExtended : Control
{
    public static Node? LoadScene(string name = "PotionHistory")
    {
        Node? result = new HBoxContainer();
        result.Name = name;
        result = result.SafelySetScript(
            PreloadManager.Cache.GetAsset(
                "res://AlternativeStartingInventoryCode/Scenes/Screens/PotionHistoryExtended.cs"));
        return result;
    }

    public void LoadPotions(Player player, List<PotionModel> potions)
    {
        var list = potions.Select((p, idx) => p.ToMutable()).ToList();

        var list2 = new List<NPotionHolder>();
        this.FreeChildren();

        for (var num = 0; num < potions.Count; num++)
        {
            var nPotionHolder = NPotionHolder.Create(false);

            this.AddChildSafely(nPotionHolder);
            list2.Add(nPotionHolder);
        }

        for (var num2 = 0; num2 < list.Count && num2 < potions.Count; num2++)
        {
            var nPotion = NPotion.Create(list[num2]);
            if (nPotion == null) continue;
            nPotion.Model.Owner = player;
            list2[num2].AddPotion(nPotion);
            nPotion.Position = Vector2.Zero;
        }
    }
}