/* GENERATED CODE, DO NOT MODIFY */
using Godot;
using MegaCrit.Sts2.Core.Assets;
namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public static class DeckInfoPanel
{

     public static Node? LoadScene(string name = "DeckInfoPanel")
     {
        var result = PreloadManager.Cache.GetScene("res://AlternativeStartingDecks/scenes/screens/deck_info_panel.tscn")
           .Instantiate();
       if (result == null) return null; 
       
       // Load Dependencies
        var var_5_iqkhv = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_shared.tres");
        var var_FontVariation_3c48f = new FontVariation();
        var var_1_2y3j0 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/fuzzy_nine_patch_char_select.png");
        var var_2_rcnbj = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/spectral_glphy_space_two.tres");
        var var_3_2g4s6 = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var var_9_y4xqx = PreloadManager.Cache.GetAsset<Texture2D>("res://images/debug/placeholder_64.png");
        var var_8_mrmwp = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
        var var_7_0gfss = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_shared.tres");
       
       // Apply Dependencies
        var_FontVariation_3c48f.BaseFont = var_5_iqkhv;
        result.GetNode<NinePatchRect>("./NinePatchRect").Texture = var_1_2y3j0;
        result.GetNode<Label>("VBoxContainer/Name").AddThemeFontOverride("font", var_2_rcnbj);
        result.GetNode<Label>("VBoxContainer/Name").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/Relic/Icon").Texture = var_9_y4xqx;
        result.GetNode<TextureRect>("VBoxContainer/Relic/Icon/Outline").Texture = var_9_y4xqx;
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Name/RichTextLabel").AddThemeFontOverride("normal_font", var_FontVariation_3c48f);
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Name/RichTextLabel").SetScript(var_8_mrmwp);
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Description").AddThemeFontOverride("normal_font", var_7_0gfss);
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Description").AddThemeFontOverride("bold_font", var_5_iqkhv);
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Description").SetScript(var_8_mrmwp);
       
       result.Name = name;
       
       return result;
     }
    
}
