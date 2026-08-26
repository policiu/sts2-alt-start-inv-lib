using System.Diagnostics;
using AlternativeStartingInventory.AlternativeStartingInventoryCode.Scenes.Screens;
using AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Patches.Ui;

public class DeckSelectorPatch
{
    private static bool OnDeckPressed(NCharacterSelectScreen screen, DeckInfoPanelExtended panel,
        DeckInfoPlaceholderExtended deck,
        StartingInventory inventory,
        CharacterModel characterModel, bool isCharacterLocked)
    {
        // Prevent if locked :)
        if (isCharacterLocked)
        {
            // Clear in case something goes really wrong
            AlternativeStartingInventoryGlobals.StartingInventory = null;
            return false;
        }

        if (inventory.IsLocked) return false;

        // Note: We still show the deck while the player is ready
        panel.ShowDeckInformation(inventory, characterModel);

        // Prevent if ready
        if (screen.Lobby.LocalPlayer.isReady) return false;

        // Unselect Previous decks
        foreach (var node in deck.GetParent().FindChildren("*", "", false, false))
            if (node is DeckInfoPlaceholderExtended otherDeck)
                otherDeck.SetSelected(false);

        deck.SetSelected(true);
        AlternativeStartingInventoryGlobals.StartingInventory = inventory;
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
                    "AlternativeStartingInventory was unable to add the Deck Selection to the Character SelectScreen." +
                    "This is likely either due to a recent game update, or mod incompatibility." +
                    $"{e.Message}");

                Debugger.Break();
            }
        }

        private static void InjectDeckSelectorUi(NCharacterSelectScreen screen)
        {
            var infoPanelOg = screen.GetNodeOrNull<Control>("InfoPanel");

            var next = DeckInfoPanelExtended.LoadScene();
            if (next == null) return;
            next.SetVisible(false);
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
                AlternativeStartingInventoryGlobals.NetId = __instance.Lobby.NetService.NetId;
                InjectSelectCharacter(__instance, charSelectButton, characterModel);
            }
            catch (Exception e)
            {
                AlternativeStartingInventoryLogger.Warn("Unable to inject the Character SelectScreen. " + "\n" +
                                                        e.Message +
                                                        "\n" + e.StackTrace);
                AlternativeStartingInventoryLogger.Warn(e.InnerException?.Message ?? " ");
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
                AlternativeStartingInventoryGlobals.StartingInventory = null;
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
            var characterModel = charSelectButton.Character;
            var character = characterModel.GetType().Name;

            var title = charSelectButton.IsLocked
                ? new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title").GetFormattedText()
                : new LocString("characters", characterModel.CharacterSelectTitle).GetFormattedText();
            deckInfoPanel.SetTitle(title);
            var container = deckInfoPanel.GetNodeOrNull<Control>("VBoxContainer/ScrollContainer/VBoxContainer");

            // Clear our overrides in case something goes wrong
            AlternativeStartingInventoryGlobals.StartingInventory = null;

            if (container == null) return;
            foreach (var child in container.GetChildren())
                child.QueueFree();

            var inventories = StartingInventoryManager.GetStartingInventoriesForCharacter(character);
            var isFirstVisibleDeckSelected = false;
            var first = true;
            foreach (var inventory in inventories)
            {
                if (inventory.IsHidden) continue;
                var deckInfoControl = (DeckInfoPlaceholderExtended?)DeckInfoPlaceholderExtended.LoadScene();
                if (deckInfoControl == null) continue;
                deckInfoControl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

                container.AddChild(deckInfoControl);

                // Update Focus
                deckInfoControl.Button.FocusNeighborTop =
                    first
                        ? deckInfoControl.Button.GetPath()
                        : ((DeckInfoPlaceholderExtended)container.GetChild(-2)).Button.GetPath();
                deckInfoControl.Button.FocusNeighborBottom = charSelectButton.GetPath();
                if (!first)
                    ((DeckInfoPlaceholderExtended)container.GetChild<Control>(-2)).Button.FocusNeighborBottom =
                        deckInfoControl.Button.GetPath();
                else
                    charSelectButton.FocusNeighborTop =
                        deckInfoControl.Button.GetPath();

                var deckHelper = deckInfoControl;
                deckHelper.SetVisible(true);

                // Might be slow? Should profile?
                // Prob fine, it's just a menu :)
                SetupDeckStrings(deckHelper, characterModel, inventory, charSelectButton.IsLocked,
                    inventory.IsLocked);

                // Apply Event
                deckHelper.Button.GuiInput += e =>
                {
                    if (e.IsActionReleased("ui_confirm"))
                        OnDeckPressed(screen, deckInfoPanel, deckHelper, inventory, characterModel,
                            charSelectButton.IsLocked);
                };
                deckHelper.Button.Released += _ => OnDeckPressed(screen, deckInfoPanel, deckHelper, inventory,
                    characterModel, charSelectButton.IsLocked);
                if (!isFirstVisibleDeckSelected)
                    // Make sure to apply first deck
                    isFirstVisibleDeckSelected = OnDeckPressed(screen, deckInfoPanel, deckHelper, inventory,
                        characterModel,
                        charSelectButton.IsLocked);

                first = false;
            }

            // We didn't show any decks
            if (!isFirstVisibleDeckSelected)
            {
                deckInfoPanel.HideDeckInformation();
                charSelectButton.FocusNeighborTop = charSelectButton.GetPath();
            }
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