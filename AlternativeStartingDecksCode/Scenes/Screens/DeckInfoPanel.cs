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
        var var_3_2g4s6 = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var var_8_mrmwp = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");
        var var_4_ykdud = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_heart.tres");
        var var_6_p3aly = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_gold.tres");
        var var_9_ykdud = PreloadManager.Cache.GetAsset<Texture2D>("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_deck.tres");
        var var_10_iqkhv = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/game_over_screen/discovery_potion.png");
        var var_11_p3aly = PreloadManager.Cache.GetAsset<Texture2D>("res://images/ui/game_over_screen/discovery_relic.png");
        var var_9_y4xqx = PreloadManager.Cache.GetAsset<Texture2D>("res://images/debug/placeholder_64.png");
       
       // Apply Dependencies
        result.GetNode<NinePatchRect>("./NinePatchRect").Texture = var_1_2y3j0;
        result.GetNode<Label>("VBoxContainer/Name").SetScript(var_3_2g4s6);
        result.GetNode<RichTextLabel>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/DeckLabel").SetScript(var_8_mrmwp);
        result.GetNode<RichTextLabel>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/DescriptionLabel").SetScript(var_8_mrmwp);
        result.GetNode<TextureRect>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Hp/Icon").Texture = var_4_ykdud;
        result.GetNode<Label>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Hp/Label").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Gold/Icon").Texture = var_6_p3aly;
        result.GetNode<Label>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Gold/Label").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Deck/Icon").Texture = var_9_ykdud;
        result.GetNode<Label>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Deck/Label").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Potions/Icon").Texture = var_10_iqkhv;
        result.GetNode<Label>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Potions/Label").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Relics/Icon").Texture = var_11_p3aly;
        result.GetNode<Label>("VBoxContainer/ScrollContainer/PanelContainer/VBoxContainer/Placeholder/HpGold/Relics/Label").SetScript(var_3_2g4s6);
        result.GetNode<TextureRect>("VBoxContainer/Relic/Icon").Texture = var_9_y4xqx;
        result.GetNode<TextureRect>("VBoxContainer/Relic/Icon/Outline").Texture = var_9_y4xqx;
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Name/RichTextLabel").SetScript(var_8_mrmwp);
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Description").SetScript(var_8_mrmwp);
       
       result.Name = name;
       
       return result;
     }
    
}
