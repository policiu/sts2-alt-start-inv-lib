/* GENERATED CODE, DO NOT MODIFY */
using AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Scenes.Screens;

public static class DeckInfoPanel
{

     public static Node? LoadScene(string name = "DeckInfoPanel")
     {
        var result = PreloadManager.Cache.GetScene("res://AlternativeStartingInventory/scenes/screens/deck_info_panel.tscn")
           .Instantiate();
       if (result == null) return null; 
       
       // Load Dependencies
        var var_1_2y3j0 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/fuzzy_nine_patch_char_select.png");
        var var_2_rcnbj = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/spectral_glphy_space_two.tres");
        var var_3_2g4s6 = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var var_StyleBoxEmpty_ig1cs = new StyleBoxEmpty();
        var var_StyleBoxEmpty_l70hg = new StyleBoxEmpty();
        var var_4_ig1cs = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_glyph_space_one.tres");
        var var_5_l70hg = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_glyph_space_one.tres");
        var var_6_qi74u = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
       
       // Apply Dependencies
        result.GetNode<NinePatchRect>("./NinePatchRect").Texture = var_1_2y3j0;
        result.GetNode<Label>("VBoxContainer/Name").AddThemeFontOverride("font", var_2_rcnbj);
        result.GetNode<Label>("VBoxContainer/Name").SafelySetScript(var_3_2g4s6);
        result.GetNode<NinePatchRect>("DeckInformation/Bg").Texture = var_1_2y3j0;
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").AddThemeFontOverride("normal_font", var_4_ig1cs);
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").AddThemeFontOverride("bold_font", var_5_l70hg);
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").SafelySetScript(var_6_qi74u);
       
       result.Name = name;
       
       return result;
     }
    
}
