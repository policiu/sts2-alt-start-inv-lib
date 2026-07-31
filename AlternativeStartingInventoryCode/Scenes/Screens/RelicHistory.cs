/* GENERATED CODE, DO NOT MODIFY */
using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public static class RelicHistory
{

     public static Node? LoadScene(string name = "RelicHistory")
     {
        var result = PreloadManager.Cache.GetScene("res://AlternativeStartingDecks/scenes/screens/relic_history.tscn")
           .Instantiate();
       if (result == null) return null; 
       
       // Load Dependencies
        var var_1_wpu8f = PreloadManager.Cache.GetAsset<Script>("res://src/Core/Nodes/Screens/RunHistoryScreen/NRelicHistory.cs");
        var var_2_tgkx6 = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_glyph_space_one.tres");
        var var_3_wm4y4 = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_glyph_space_one.tres");
        var var_4_onatx = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
       
       // Apply Dependencies
        result = result.SafelySetScript(var_1_wpu8f)!;
        result.GetNode<RichTextLabel>("./Header").AddThemeFontOverride("normal_font", var_2_tgkx6);
        result.GetNode<RichTextLabel>("./Header").AddThemeFontOverride("bold_font", var_3_wm4y4);
        result.GetNode<RichTextLabel>("./Header").SafelySetScript(var_4_onatx);
       
       result.Name = name;
       
       return result;
     }
    
}
