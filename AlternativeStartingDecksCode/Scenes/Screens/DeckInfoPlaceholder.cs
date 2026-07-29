/* GENERATED CODE, DO NOT MODIFY */
using Godot;
using MegaCrit.Sts2.Core.Assets;
namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public static class DeckInfoPlaceholder
{

     public static Node? LoadScene(string name = "DeckInfoPlaceholder")
     {
        var result = PreloadManager.Cache.GetScene("res://AlternativeStartingDecks/scenes/screens/deck_info_placeholder.tscn")
           .Instantiate();
       if (result == null) return null; 
       
       // Load Dependencies
        var var_1_7ej3d = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_bold_shared.tres");
        var var_FontVariation_3c48f = new FontVariation();
        var var_16_oejt8 = PreloadManager.Cache.GetAsset<FontVariation>("res://themes/kreon_regular_shared.tres");
        var var_FontVariation_ueawe = new FontVariation();
        var var_FontVariation_3rwao = new FontVariation();
        var var_StyleBoxFlat_0gfss = new StyleBoxFlat();
        var var_15_ho10k = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
        var var_17_6erg2 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_heart.tres");
        var var_18_6f3yj = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var var_19_wr6f5 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_gold.tres");
        var var_20_v5b6b = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_deck.tres");
        var var_21_2hq02 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/game_over_screen/discovery_potion.png");
        var var_22_ubig5 = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/game_over_screen/discovery_relic.png");
       
       // Apply Dependencies
        var_FontVariation_3c48f.BaseFont = var_1_7ej3d;
        var_FontVariation_ueawe.BaseFont = var_16_oejt8;
        var_FontVariation_3rwao.BaseFont = var_1_7ej3d;
        result.GetNode<RichTextLabel>("VBoxContainer/DeckLabel").AddThemeFontOverride("normal_font", var_FontVariation_3c48f);
        result.GetNode<RichTextLabel>("VBoxContainer/DeckLabel").SetScript(var_15_ho10k);
        result.GetNode<RichTextLabel>("VBoxContainer/DescriptionLabel").AddThemeFontOverride("normal_font", var_FontVariation_ueawe);
        result.GetNode<RichTextLabel>("VBoxContainer/DescriptionLabel").SetScript(var_15_ho10k);
        result.GetNode<TextureRect>("VBoxContainer/HpGold/Hp/Icon").Texture = var_17_6erg2;
        result.GetNode<Label>("VBoxContainer/HpGold/Hp/Label").AddThemeFontOverride("font", var_FontVariation_3rwao);
        result.GetNode<Label>("VBoxContainer/HpGold/Hp/Label").SetScript(var_18_6f3yj);
        result.GetNode<TextureRect>("VBoxContainer/HpGold/Gold/Icon").Texture = var_19_wr6f5;
        result.GetNode<Label>("VBoxContainer/HpGold/Gold/Label").AddThemeFontOverride("font", var_FontVariation_3rwao);
        result.GetNode<Label>("VBoxContainer/HpGold/Gold/Label").SetScript(var_18_6f3yj);
        result.GetNode<TextureRect>("VBoxContainer/HpGold/Deck/Icon").Texture = var_20_v5b6b;
        result.GetNode<Label>("VBoxContainer/HpGold/Deck/Label").AddThemeFontOverride("font", var_FontVariation_3rwao);
        result.GetNode<Label>("VBoxContainer/HpGold/Deck/Label").SetScript(var_18_6f3yj);
        result.GetNode<TextureRect>("VBoxContainer/HpGold/Potions/Icon").Texture = var_21_2hq02;
        result.GetNode<Label>("VBoxContainer/HpGold/Potions/Label").AddThemeFontOverride("font", var_FontVariation_3rwao);
        result.GetNode<Label>("VBoxContainer/HpGold/Potions/Label").SetScript(var_18_6f3yj);
        result.GetNode<TextureRect>("VBoxContainer/HpGold/Relics/Icon").Texture = var_22_ubig5;
        result.GetNode<Label>("VBoxContainer/HpGold/Relics/Label").AddThemeFontOverride("font", var_FontVariation_3rwao);
        result.GetNode<Label>("VBoxContainer/HpGold/Relics/Label").SetScript(var_18_6f3yj);
       
       result.Name = name;
       
       return result;
     }
    
}
