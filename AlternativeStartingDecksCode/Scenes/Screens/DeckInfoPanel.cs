/* GENERATED CODE, DO NOT MODIFY */
using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
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
        var var_9_qi74u = PreloadManager.Cache.GetAsset<Shader>("res://shaders/hsv.gdshader");
        var var_ShaderMaterial_k5jb6 = new ShaderMaterial();
        var var_1_2y3j0 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/fuzzy_nine_patch_char_select.png");
        var var_2_rcnbj = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/spectral_glphy_space_two.tres");
        var var_3_2g4s6 = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var var_StyleBoxEmpty_ig1cs = new StyleBoxEmpty();
        var var_StyleBoxEmpty_l70hg = new StyleBoxEmpty();
        var var_4_ig1cs = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_glyph_space_one.tres");
        var var_5_l70hg = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_glyph_space_one.tres");
        var var_6_qi74u = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
        var var_4_acp43 = PreloadManager.Cache.GetAsset<Script>("res://src/Core/Nodes/CommonUi/NBackButton.cs");
        var var_5_s8klb = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/back_button.tres");
        var var_6_lh5hm = PreloadManager.Cache.GetAsset<Material>("res://themes/canvas_item_material_additive_shared.tres");
        var var_7_ig1cs = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/compressed.sprites/back_button_outline.tres");
        var var_8_l70hg = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/compressed.sprites/back_button_x.tres");
        var var_10_o6yww = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/placeholder_controller_icon.png");
       
       // Apply Dependencies
        result.GetNode<NinePatchRect>("./NinePatchRect").Texture = var_1_2y3j0;
        result.GetNode<Label>("VBoxContainer/Name").AddThemeFontOverride("font", var_2_rcnbj);
        result.GetNode<Label>("VBoxContainer/Name").SafelySetScript(var_3_2g4s6);
        result.GetNode<NinePatchRect>("DeckInformation/Bg").Texture = var_1_2y3j0;
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").AddThemeFontOverride("normal_font", var_4_ig1cs);
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").AddThemeFontOverride("bold_font", var_5_l70hg);
        result.GetNode<RichTextLabel>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/Header").SafelySetScript(var_6_qi74u);
        result.GetNode<Control>("DeckInformation/BackButton").SafelySetScript(var_4_acp43);
        result.GetNode<TextureRect>("DeckInformation/BackButton/Shadow").Texture = var_5_s8klb;
        result.GetNode<TextureRect>("DeckInformation/BackButton/Outline").Texture = var_7_ig1cs;
        result.GetNode<TextureRect>("DeckInformation/BackButton/Image").Texture = var_5_s8klb;
        result.GetNode<TextureRect>("DeckInformation/BackButton/Image/Icon").Texture = var_8_l70hg;
        result.GetNode<TextureRect>("DeckInformation/BackButton/ControllerIcon").Texture = var_10_o6yww;
       
       result.Name = name;
       
       return result;
     }
    
}
