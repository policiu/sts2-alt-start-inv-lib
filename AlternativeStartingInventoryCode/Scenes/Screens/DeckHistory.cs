/* GENERATED CODE, DO NOT MODIFY */
using AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Scenes.Screens;

public static class DeckHistory
{

     public static Node? LoadScene(string name = "DeckHistory")
     {
        var result = PreloadManager.Cache.GetScene("res://AlternativeStartingInventory/scenes/screens/deck_history.tscn")
           .Instantiate();
       if (result == null) return null; 
       
       // Load Dependencies
        var var_1_e8xms = PreloadManager.Cache.GetAsset<Script>("res://src/Core/Nodes/Screens/RunHistoryScreen/NDeckHistory.cs");
        var var_2_wstha = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_glyph_space_one.tres");
        var var_3_xkgtr = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_glyph_space_one.tres");
        var var_17_4a7ov = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
       
       // Apply Dependencies
        result = result.SafelySetScript(var_1_e8xms)!;
        result.GetNode<RichTextLabel>("./Header").AddThemeFontOverride("normal_font", var_2_wstha);
        result.GetNode<RichTextLabel>("./Header").AddThemeFontOverride("bold_font", var_3_xkgtr);
        result.GetNode<RichTextLabel>("./Header").SafelySetScript(var_17_4a7ov);
       
       result.Name = name;
       
       return result;
     }
    
}
