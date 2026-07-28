using AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Timer = Godot.Timer;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Ui;

// Don't Look, def won't crash... promise?
internal static class SharedUi
{
    public static Button? Button;
}

public class DeckSelectorPatch
{
    [HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen._Ready))]
    public static class ReadyPatch
    {
        public static void Postfix(NCharacterSelectScreen __instance)
        {
            try
            {
                InjectDeckSelectorUi(__instance);
            }
            catch (Exception e)
            {
                ModConfig.ModConfigLogger.Error(
                    "AlternativeStartingDecks was unable to add the Deck Selection to the Character SelectScreen." +
                    "This is likely either due to a recent game update, or mod incompatibility." +
                    $"{e.Message}");
            }
        }

        private static void InjectDeckSelectorUi(NCharacterSelectScreen screen)
        {
            var button = new Button();
            var infoPanelOg = screen.GetNodeOrNull<Control>("InfoPanel");

            void Clicked()
            {
                button.Text = button.GetGlobalMousePosition() + " " +
                              ((Control)button.GetParent()).GetLocalMousePosition() + '\n';
                button.Text += (screen.GetViewport().GuiGetHoveredControl()?.Name ?? "No Element") + "\n";
                button.Text += screen.GetViewport().GuiGetHoveredControl()?.GetParentControl()?.Name ?? "No Element";
            }


            button.Text = "Add Character Select";
            button.CustomMinimumSize = new Vector2(150, 50);
            button.Position = new Vector2(-940, -1000);
            screen.GetNodeOrNull("CharSelectButtons/ButtonContainer")?.AddSibling(button);
            var a = screen.GetNodeOrNull<Button>("CharSelectButtons");
            button.MouseEntered += Clicked;
            SharedUi.Button = button;

            // Add Timer
            var myTimer = new Timer();
            myTimer.WaitTime = 1;
            myTimer.SetOneShot(false);
            myTimer.Timeout += Clicked;
            button.AddSibling(myTimer);
            myTimer.Start();

            // Anyways :)
            var next = DeckInfoPanel.LoadScene();
            infoPanelOg.AddSibling(next);
        }
    }

    [HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.SelectCharacter))]
    public static class SelectCharacterPatch
    {
        // Oh no
        private static Tween? _deckInfoTween;
        private static Vector2 _deckInfoPosition;

        [HarmonyPostfix]
        public static void Postfix(NCharacterSelectScreen __instance)
        {
            try
            {
                InjectSelectCharacter(__instance);
            }
            catch (Exception e)
            {
                ModConfig.ModConfigLogger.Warn("[ASD] Unable to inject the Character SelectScreen. " + "\n" +
                                               e.Message);
            }
        }

        private static void InjectSelectCharacter(NCharacterSelectScreen screen)
        {
            var deckInfoPanel = screen.GetNodeOrNull<Control>("DeckInfoPanel");
            if (deckInfoPanel == null) return;
            if (_deckInfoTween != null) deckInfoPanel.Position = _deckInfoPosition;
            _deckInfoPosition = deckInfoPanel.Position;
            _deckInfoTween?.Kill();
            _deckInfoTween = screen.CreateTween().SetParallel();

            _deckInfoTween.TweenProperty(deckInfoPanel, (NodePath)"position", deckInfoPanel.Position, 0.5)
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo)
                .From(deckInfoPanel.Position + new Vector2(300f, 0.0f));
        }
    }
}

[HarmonyPatch(nameof(NCharacterSelectButton), nameof(NCharacterSelectButton.Select))]
public static class NCharacterSelectButton_Select
{
    public static void Postfix(NCharacterSelectButton __instance)
    {
        var button = SharedUi.Button;
        if (button != null) button.Text = __instance._character.CharacterSelectTitle;
    }
}