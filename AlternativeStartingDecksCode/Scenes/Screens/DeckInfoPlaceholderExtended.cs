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

    private static readonly StyleBoxFlat SelectedTheme =
        (StyleBoxFlat)PreloadManager.Cache.LoadAsset("res://AlternativeStartingDecks/themes/selected_theme.tres");

    private static readonly string _deckDescriptionNode = "VBoxContainer/DescriptionLabel";
    private static readonly string _deckNameNode = "VBoxContainer/DeckLabel";

    private Theme? _baseTheme;

    private string _deckNode = "VBoxContainer/HpGold/Deck";
    private string _goldNode = "VBoxContainer/HpGold/Gold";

    private string _hpNode = "VBoxContainer/HpGold/Hp";
    private string _potionsNode = "VBoxContainer/HpGold/Potions";
    private string _relicsNode = "VBoxContainer/HpGold/Relics";

    // Add static constructor
    static DeckInfoPlaceholderExtended()
    {
    }

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

    public string DeckName
    {
        get => GetNode<MegaRichTextLabel>(_deckNameNode).Text;
        set => GetNode<MegaRichTextLabel>(_deckNameNode).SetTextAutoSize(value);
    }

    public string DeckDescription
    {
        get => GetNode<MegaRichTextLabel>(_deckDescriptionNode).Text;
        set => GetNode<MegaRichTextLabel>(_deckDescriptionNode).SetTextAutoSize(value);
    }

    public BaseButton Button => GetNode<BaseButton>("Button");

    public override void _Ready()
    {
        StartLoopingBorder(CreateTween().SetLoops(), SelectedTheme, 1.0f);
        base._Ready();
    }


    public void StartLoopingBorder(Tween tween, StyleBoxFlat styleBox, float duration)
    {
        if (tween.IsValid()) tween.Kill();

        tween = CreateTween().SetLoops();
        tween.TweenProperty(styleBox, "border_color:a", 0.0f, duration);
        tween.TweenProperty(styleBox, "border_color:a", 1.0f, duration);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            AddThemeStyleboxOverride("panel", SelectedTheme);
        else
            RemoveThemeStyleboxOverride("panel");
    }


    public static Node? LoadScene(string name = "DeckInfoPlaceholder")
    {
        var result = DeckInfoPlaceholder.LoadScene(name);
        if (result == null) return null;

        DisableAutoSize(result);

        // Setting Script directly disposes the previous object
        result = result.SafelySetScript(ThisScript);

        return result;
    }

    private static void DisableAutoSize(Node result)
    {
        // Duplicating removes all script variables, so we need to reapply the important ones (AutoSize off)
        var children = result.FindChildren("Label", recursive: true);
        foreach (var child in children)
        {
            if (child == null) continue;

            var script = (MegaLabel)child;
            script.AutoSizeEnabled = false;
            script.MinFontSize = 28;
            script.MaxFontSize = 100;
        }

        children = [result.GetNode(_deckNameNode), result.GetNode(_deckDescriptionNode)];

        foreach (var child in children)
        {
            if (child == null) continue;

            var script = (MegaRichTextLabel)child;
            script.AutoSizeEnabled = false;
            script.MinFontSize = 28;
            script.MaxFontSize = 28;
        }
    }
}