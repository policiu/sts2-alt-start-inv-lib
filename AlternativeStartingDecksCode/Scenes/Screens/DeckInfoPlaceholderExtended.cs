using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public class DeckInfoPlaceholderExtended
{
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

        return result;
    }
}