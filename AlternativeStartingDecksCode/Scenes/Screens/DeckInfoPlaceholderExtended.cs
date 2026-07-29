using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public partial class DeckInfoPlaceholderExtended : Control
{
    private static readonly Resource ThisScript =
        PreloadManager.Cache.LoadAsset(
            "res://AlternativeStartingDecksCode/Scenes/Screens/DeckInfoPlaceholderExtended.cs");

    private string _deckNode = "VBoxContainer/HpGold/Deck";
    private string _goldNode = "VBoxContainer/HpGold/Gold";

    private string _hpNode = "VBoxContainer/HpGold/Hp";
    private string _potionsNode = "VBoxContainer/HpGold/Potions";
    private string _relicsNode = "VBoxContainer/HpGold/Relics";

    public string Hp
    {
        get => GetNode<Label>(_hpNode + "/Label").Text;
        set => GetNode<Label>(_hpNode + "/Label").Text = value;
    }

    public string Gold
    {
        get => GetNode<Label>(_goldNode + "/Label").Text;
        set => GetNode<Label>(_goldNode + "/Label").Text = value;
    }

    public string Deck
    {
        get => GetNode<Label>(_deckNode + "/Label").Text;
        set => GetNode<Label>(_deckNode + "/Label").Text = value;
    }

    public string Potions
    {
        get => GetNode<Label>(_potionsNode + "/Label").Text;
        set => GetNode<Label>(_potionsNode + "/Label").Text = value;
    }

    public string Relics
    {
        get => GetNode<Label>(_relicsNode + "/Label").Text;
        set => GetNode<Label>(_relicsNode + "/Label").Text = value;
    }

    public static Node? LoadScene(string name = "DeckInfoPlaceholder")
    {
        var result = DeckInfoPlaceholder.LoadScene(name);
        if (result == null) return null;

        // Update the stupid MegaLabel
        var children = result.FindChildren("Label", recursive: true);

        foreach (var child in children)
        {
            if (child == null) continue;


            var script = (MegaLabel)child;
            script.AutoSizeEnabled = false;
            script.MinFontSize = 28;
            script.MaxFontSize = 100;
        }

        result = result.SafelySetScript(ThisScript);

        return result;
    }
}