using System.Diagnostics;
using AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;
using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
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
    private static bool OnDeckPressed(DeckInfoPanelExtended panel, DeckInfoPlaceholderExtended deck,
        StartingInventory inventory,
        CharacterModel characterModel, bool isCharacterLocked)
    {
        // Prevent if locked :)
        if (isCharacterLocked)
        {
            AlternativeStartingDecksGlobals.StartingInventory = null;
            return false;
        }

        if (inventory.IsLocked) return false;

        foreach (var node in deck.GetParent().FindChildren("*", "", false, false))
            if (node is DeckInfoPlaceholderExtended otherDeck)
                otherDeck.SetSelected(false);

        panel.ShowDeckInformation(inventory, characterModel);
        deck.SetSelected(true);
        AlternativeStartingDecksGlobals.StartingInventory = inventory;
        return true;
    }

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

                Debugger.Break();
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
            var next = DeckInfoPanelExtended.LoadScene();
            if (next == null) return;
            infoPanelOg.AddSibling(next);
        }
    }

    [HarmonyPatch(typeof(NCharacterSelectScreen), nameof(NCharacterSelectScreen.SelectCharacter))]
    public static class SelectCharacterPatch
    {
        // Oh no
        private static Tween? _deckInfoTween;
        private static Vector2 _deckInfoPosition;

        private static readonly string DeckInfoName = "DeckInfoPanel";

        [HarmonyPostfix]
        public static void Postfix(NCharacterSelectScreen __instance, NCharacterSelectButton charSelectButton,
            CharacterModel characterModel)
        {
            try
            {
                InjectSelectCharacter(__instance, charSelectButton, characterModel);
            }
            catch (Exception e)
            {
                AlternativeStartingDecksLogger.Warn("Unable to inject the Character SelectScreen. " + "\n" + e.Message +
                                                    "\n" + e.StackTrace);
                AlternativeStartingDecksLogger.Warn(e.InnerException?.Message ?? " ");
            }
        }

        private static void InjectSelectCharacter(NCharacterSelectScreen screen,
            NCharacterSelectButton charSelectButton, CharacterModel characterModel)
        {
            // If there aren't enough decks, don't show our menu

            if (SetPanelVisibility(screen, characterModel)) return;
            RunTween(screen);
            LoadDecks(screen, charSelectButton);
        }

        private static bool SetPanelVisibility(NCharacterSelectScreen screen, CharacterModel characterModel)
        {
            var deckInfoPanel = screen.GetNodeOrNull<Control>(DeckInfoName);
            var infoPanel = screen.GetNodeOrNull<Control>("InfoPanel");
            var inventories = StartingInventoryManager.GetStartingInventoriesForCharacter(characterModel);


            if (inventories.Count == 0 || (inventories.Count == 1 && inventories.First().Id == "default"))
            {
                deckInfoPanel?.SetVisible(false);
                foreach (var node in infoPanel.GetChildren())
                {
                    var child = (Control?)node;

                    child?.SetVisible(true);
                }

                return true;
            }

            deckInfoPanel?.SetVisible(true);
            foreach (var node in infoPanel.GetChildren())
            {
                var child = (Control?)node;

                child?.SetVisible(false);
            }

            return false;
        }


        /// <summary>
        ///     Slide into view
        /// </summary>
        private static void RunTween(NCharacterSelectScreen screen)
        {
            var deckInfoPanel = screen.GetNodeOrNull<Control>(DeckInfoName);
            if (deckInfoPanel == null) return;
            if (_deckInfoTween != null) deckInfoPanel.Position = _deckInfoPosition;
            _deckInfoPosition = deckInfoPanel.Position;
            _deckInfoTween?.Kill();
            _deckInfoTween = screen.CreateTween().SetParallel();

            _deckInfoTween.TweenProperty(deckInfoPanel, (NodePath)"position", deckInfoPanel.Position, 0.5)
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo)
                .From(deckInfoPanel.Position + new Vector2(-300f, 0.0f));
        }

        private static void LoadDecks(NCharacterSelectScreen screen, NCharacterSelectButton charSelectButton)
        {
            var deckInfoPanel = screen.GetNodeOrNull<DeckInfoPanelExtended>(DeckInfoName);
            var container = deckInfoPanel.GetNodeOrNull<Control>("VBoxContainer/ScrollContainer/VBoxContainer");

            // Clear our overrides in case something goes wrong
            AlternativeStartingDecksGlobals.StartingInventory = null;

            if (container == null) return;
            foreach (var child in container.GetChildren())
                child.QueueFree();

            var character = screen._selectedButton?._character.GetType().Name;

            if (character is null) return;

            var inventories = StartingInventoryManager.GetStartingInventoriesForCharacter(character);
            var isFirstVisibleDeckSelected = false;
            foreach (var inventory in inventories)
            {
                if (inventory.IsHidden) continue;
                var deckInfoControl = (Control?)DeckInfoPlaceholderExtended.LoadScene();
                if (deckInfoControl == null) continue;
                deckInfoControl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;


                container.AddChild(deckInfoControl);

                var deckHelper = (DeckInfoPlaceholderExtended)deckInfoControl;
                deckHelper.SetVisible(true);

                // Might be slow? Should profile?
                // Prob fine, it's just a menu :)
                SetupDeckStrings(deckHelper, charSelectButton.Character, inventory, charSelectButton.IsLocked,
                    inventory.IsLocked);

                // Apply Event
                deckHelper.Button.MousePressed += _ => OnDeckPressed(deckInfoPanel, deckHelper, inventory,
                    charSelectButton._character, charSelectButton.IsLocked);
                if (!isFirstVisibleDeckSelected)
                    // Make sure to apply first deck
                    isFirstVisibleDeckSelected = OnDeckPressed(deckInfoPanel, deckHelper, inventory,
                        charSelectButton._character,
                        charSelectButton.IsLocked);
            }

            // We didn't show any decks
            if (!isFirstVisibleDeckSelected) deckInfoPanel.HideDeckInformation();
        }

        private static void SetupDeckStrings(DeckInfoPlaceholderExtended deckHelper,
            CharacterModel character,
            StartingInventory inventory, bool characterIsLocked, bool inventoryIsLocked)
        {
            if (characterIsLocked)
            {
                deckHelper.DeckName =
                    new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title").GetFormattedText();
                deckHelper.DeckDescription = character.GetUnlockText().GetFormattedText();
                deckHelper.Hp = "??/??";
                deckHelper.Gold = "???";
                deckHelper.Relics = "???";
                deckHelper.Potions = "???";
                deckHelper.Deck = "???";
            }

            else if (inventoryIsLocked)
            {
                deckHelper.DeckName =
                    new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title").GetFormattedText();
                deckHelper.DeckDescription = inventory.UnlockText;
                deckHelper.Hp = "??/??";
                deckHelper.Gold = "???";
                deckHelper.Relics = "???";
                deckHelper.Potions = "???";
                deckHelper.Deck = "???";
            }
            else

            {
                deckHelper.DeckName = inventory.Name;

                deckHelper.DeckDescription = inventory.Description;

                deckHelper.Deck = inventory.Cards.Count().ToString();
                deckHelper.Hp = $"{inventory.Hp.ToString()}/{inventory.Hp.ToString()}";
                deckHelper.Relics = inventory.Relics.Count().ToString();
                deckHelper.Potions = inventory.Potions.Count().ToString();
                deckHelper.Gold = inventory.Gold.ToString();
            }
        }
    }
}

[HarmonyPatch(nameof(NCharacterSelectButton), nameof(NCharacterSelectButton.Select))]
public static class NCharacterSelectButtonSelectPatch
{
    public static void Postfix(NCharacterSelectButton __instance)
    {
        var button = SharedUi.Button;
        if (button != null) button.Text = __instance._character.CharacterSelectTitle;
    }
}