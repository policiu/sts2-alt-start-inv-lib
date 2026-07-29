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
        var var_1_2y3j0 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/fuzzy_nine_patch_char_select.png");
        var var_2_rcnbj = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/spectral_glphy_space_two.tres");
        var var_3_2g4s6 = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
       
       // Apply Dependencies
        result.GetNode<NinePatchRect>("./NinePatchRect").Texture = var_1_2y3j0;
        result.GetNode<Label>("VBoxContainer/Name").AddThemeFontOverride("font", var_2_rcnbj);
        result.GetNode<Label>("VBoxContainer/Name").SetScript(var_3_2g4s6);
       
       result.Name = name;
       
       return result;
     }
    
}
