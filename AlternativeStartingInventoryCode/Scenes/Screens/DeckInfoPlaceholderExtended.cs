using AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Scenes.Screens;

public partial class DeckInfoPlaceholderExtended : Control
{
    private static readonly string _deckDescriptionNode = "VBoxContainer/DescriptionLabel";
    private static readonly string _deckNameNode = "VBoxContainer/DeckLabel";

    private Theme? _baseTheme;

    private Control? _buttonImage;

    private string _deckNode = "VBoxContainer/HpGold/Deck";
    private string _goldNode = "VBoxContainer/HpGold/Gold";
    private Tween? _hoverTween;

    private string _hpNode = "VBoxContainer/HpGold/Hp";
    private Color _originalColor;
    private Vector2 _originalHoverScale;
    private Control? _outline;
    private string _potionsNode = "VBoxContainer/HpGold/Potions";
    private string _relicsNode = "VBoxContainer/HpGold/Relics";
    private StyleBoxFlat? _selectedTheme;

    // Style
    public Color DownColor = Colors.Gray;
    public Color OutlineColor = new("C0C0C0");
    public Color OutlineTransparentColor = new("FF000000");

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

    public NButton Button => GetNode<NButton>("Button");

    public override void _Ready()
    {
        _selectedTheme =
            (StyleBoxFlat?)PreloadManager.Cache.GetAsset(
                "res://AlternativeStartingInventory/themes/selected_theme.tres");
        if (_selectedTheme != null) StartLoopingBorder(CreateTween().SetLoops(), _selectedTheme, 1.0f);
        _originalHoverScale = Scale;
        _originalColor = Modulate;

        Button.MouseEntered += OnFocus;
        Button.FocusEntered += OnFocus;
        Button.MouseExited += OnUnfocus;
        Button.FocusExited += OnUnfocus;

        base._Ready();
    }


    private void StartLoopingBorder(Tween tween, StyleBoxFlat styleBox, float duration)
    {
        if (tween.IsValid()) tween.Kill();

        tween = CreateTween().SetLoops();
        tween.TweenProperty(styleBox, "border_color:a", 0.0f, duration);
        tween.TweenProperty(styleBox, "border_color:a", 1.0f, duration);
    }

    public void SetSelected(bool selected)
    {
        if (!IsInstanceValid(_selectedTheme))
            _selectedTheme =
                (StyleBoxFlat?)PreloadManager.Cache.LoadAsset(
                    "res://AlternativeStartingInventory/themes/selected_theme.tres");
        if (selected && _selectedTheme != null)
            AddThemeStyleboxOverride("panel", _selectedTheme);
        else
            RemoveThemeStyleboxOverride("panel");
    }


    public static Node? LoadScene(string name = "DeckInfoPlaceholder")
    {
        var result = DeckInfoPlaceholder.LoadScene(name);
        if (result == null) return null;

        DisableAutoSize(result);

        // Setting Script directly disposes the previous object
        result = result.SafelySetScript(PreloadManager.Cache.GetAsset(
            "res://AlternativeStartingInventoryCode/Scenes/Screens/DeckInfoPlaceholderExtended.cs"));

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

    protected void OnFocus()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"modulate", OutlineColor, 0.05);
    }

    protected void OnUnfocus()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"modulate", _originalColor, 0.05);
    }

    protected void OnPress()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"scale", _originalHoverScale * .9f, 0.25)
            .SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }

    protected void OnUnpress()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"scale", _originalHoverScale, 0.25)
            .SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
    }
}