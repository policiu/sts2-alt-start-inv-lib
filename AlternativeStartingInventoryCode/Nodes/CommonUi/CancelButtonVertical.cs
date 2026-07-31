using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Nodes.CommonUi;

public partial class CancelButtonVertical : NButton
{
    public static readonly Vector2 HoverScale = Vector2.One * 1.05f;
    public static readonly Vector2 DownScale = Vector2.One;
    private Control? _buttonImage;
    private Tween? _hoverTween;
    private Vector2 _originalHoverScale;
    private Control? _outline;

    // Style
    public Color DownColor = Colors.Gray;
    public Color OutlineColor = new("F0B400");
    public Color OutlineTransparentColor = new("FF000000");


    public override void _Ready()
    {
        ConnectSignals();
        _outline = GetNode<Control>((NodePath)"Outline");
        _buttonImage = GetNode<Control>((NodePath)"Image");
        _originalHoverScale = Scale;
    }

    public new void Enable()
    {
        base.Enable();

        if (_outline != null) _outline.Modulate = Colors.Transparent;
        if (_buttonImage != null) _buttonImage.Modulate = Colors.White;
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"scale", _originalHoverScale * NBackButton._hoverScale, 0.05);
        _hoverTween.TweenProperty(_outline, (NodePath)"modulate", OutlineColor, 0.05);
    }

    protected override void OnUnfocus()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"scale", _originalHoverScale, 0.5)
            .SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
        _hoverTween.TweenProperty(_outline, (NodePath)"modulate", OutlineTransparentColor, 0.5);
    }

    protected override void OnPress()
    {
        base.OnPress();
        _hoverTween?.Kill();
        _hoverTween = CreateTween().SetParallel();
        _hoverTween.TweenProperty(this, (NodePath)"scale", _originalHoverScale * NBackButton._downScale, 0.25)
            .SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
        _hoverTween.TweenProperty(_buttonImage, (NodePath)"modulate", DownColor, 0.25)
            .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
        _hoverTween.TweenProperty(_outline, (NodePath)"modulate", OutlineTransparentColor, 0.25)
            .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
    }
}