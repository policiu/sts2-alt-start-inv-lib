using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Nodes.HoverTips;

public class CustomHoverTip
{
    public Node? Content;
    public IHoverTip HoverTip;
}

public partial class CustomHoverTipSet : NHoverTipSet
{
    internal static NHoverTipSet? CreateAndShowWithContent(
        Control owner,
        IEnumerable<CustomHoverTip> hoverTips,
        HoverTipAlignment alignment = HoverTipAlignment.None)
    {
        if (shouldBlockHoverTips)
            return null;

        var child = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip_set.tscn").Instantiate<NHoverTipSet>();

        // Hmm?
        child = child.SafelySetScript(
            PreloadManager.Cache.GetAsset("res://AlternativeStartingDecksCode/Nodes/HoverTips/CustomHoverTipSet.cs"));
        if (child == null) return null;

        HoverTipsContainer.AddChildSafely(child);
        _activeHoverTips.Add(owner, child);
        ((CustomHoverTipSet)child).Init(owner, hoverTips);

        if (NGame.IsDebugHidingHoverTips)
            child.Visible = false;

        owner.Connect(Node.SignalName.TreeExiting, Callable.From((Action)(() => Remove(owner))));
        child.SetAlignment(owner, alignment);

        return child;
    }

    public void Init(Control control, IEnumerable<CustomHoverTip> hoverTips)
    {
        var tips = hoverTips.ToList();
        base.Init(control, tips.Select(c => c.HoverTip).ToList());


        for (var i = 0;
             i < IHoverTip.RemoveDupes(tips.Select(c => c.HoverTip)).Count();
             i++)
            _textHoverTipContainer.GetChildren()[i].GetNode("TextContainer/VBoxContainer").AddChild(tips[i].Content);
    }
}