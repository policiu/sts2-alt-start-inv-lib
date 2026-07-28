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
        var charSelectPng = PreloadManager.Cache.GetTexture2D("res://images/ui/fuzzy_nine_patch_char_select.png");
        var hpPng = PreloadManager.Cache.GetTexture2D(
            "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_heart.tres");
        var goldPng =
            PreloadManager.Cache.GetTexture2D("res://images/atlases/ui_atlas.sprites/top_bar/top_bar_gold.tres");
        var placeholderPng = PreloadManager.Cache.GetTexture2D("res://images/debug/placeholder_64.png");

        var megaLabelScript = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaLabel.cs");
        var megaLabelRichScript = PreloadManager.Cache.GetAsset<Script>("res://addons/mega_text/MegaRichTextLabel.cs");

        // Apply Dependencies
        result.GetNode<NinePatchRect>("NinePatchRect").Texture = charSelectPng;
        result.GetNode<TextureRect>("VBoxContainer/HpGoldSpacer/HpGold/Hp/Icon").Texture = hpPng;
        result.GetNode<TextureRect>("VBoxContainer/HpGoldSpacer/HpGold/Gold/Icon").Texture = goldPng;
        result.GetNode<TextureRect>("VBoxContainer/Relic/Icon").Texture = placeholderPng;
        result.GetNode<RichTextLabel>("VBoxContainer/Relic/Description").SetScript(megaLabelRichScript);

        result.Name = name;

        return result;
    }
}