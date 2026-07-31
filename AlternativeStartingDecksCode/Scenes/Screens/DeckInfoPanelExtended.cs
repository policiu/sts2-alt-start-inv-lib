using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Scenes.Screens;

public partial class DeckInfoPanelExtended : Control
{
    private const string DeckInformationNode = "DeckInformation";
    private const string DeckInformationContainerNode = "DeckInformation/ScrollContainer/VBoxContainer";

    private NDeckHistory? _deckHistory;

    private Control? _deckHistoryContainer;
    private Control? _deckInformation;
    private Control? _deckInformationContainer;

    private bool _loaded;
    private PotionHistoryExtended? _potionHistory;
    private Control? _potionHistoryContainer;
    private NRelicHistory? _relicHistory;
    private Control? _relicHistoryContainer;

    public static DeckInfoPanelExtended? LoadScene(string name = "DeckInfoPanel")
    {
        var result = DeckInfoPanel.LoadScene(name);
        if (result == null) return null;

        result = result.SafelySetScript(PreloadManager.Cache.GetAsset(
            "res://AlternativeStartingDecksCode/Scenes/Screens/DeckInfoPanelExtended.cs"));

        return result as DeckInfoPanelExtended;
    }

    public override void _Ready()
    {
        _deckInformation = GetNode<Control>(DeckInformationNode);
        _deckInformationContainer = GetNode<Control>(DeckInformationContainerNode);

        _deckHistoryContainer = GetNode<Control>("DeckInformation/ScrollContainer/VBoxContainer/DeckContainer");
        _potionHistoryContainer =
            GetNode<Control>("DeckInformation/ScrollContainer/VBoxContainer/PotionAndHeaderContainer/PotionContainer");
        _relicHistoryContainer = GetNode<Control>("DeckInformation/ScrollContainer/VBoxContainer/RelicContainer");
    }


    private void LoadDeckInformation(AlternativeStartingInventory inventory, CharacterModel characterModel)
    {
        var tmpPlayer = new Player(characterModel, 0, 0, 0, 0, 0, 0, 0, null, null);

        if (inventory.Cards.Any())
        {
            if (_deckHistory == null)
            {
                _deckHistory = (NDeckHistory?)DeckHistory.LoadScene();
                _deckHistoryContainer?.AddChild(_deckHistory);
                if (_deckHistory != null)
                {
                    _deckHistory._headerLabel._maxFontSize = 24;
                    _deckHistory._headerLabel._minFontSize = 24;
                }
            }

            _deckHistory?.LoadDeck(tmpPlayer,
                inventory.Cards.Select(c => c.ToMutable().ToSerializable()).Take(25));

            _deckHistoryContainer?.Show();
        }
        else
        {
            _deckHistoryContainer?.Hide();
        }

        if (inventory.Relics.Any())
        {
            if (_relicHistory == null)
            {
                _relicHistory = (NRelicHistory?)RelicHistory.LoadScene();
                _relicHistoryContainer?.AddChild(_relicHistory);
                if (_relicHistory != null) _relicHistory._headerLabel._maxFontSize = 24;
                if (_relicHistory != null) _relicHistory._headerLabel._minFontSize = 24;
            }

            _relicHistory?.LoadRelics(tmpPlayer,
                inventory.Relics.Select(c => c.ToMutable().ToSerializable()).Take(25)
            );
            _relicHistoryContainer?.Show();
        }
        else
        {
            _relicHistoryContainer?.Hide();
        }

        if (inventory.Potions.Any())
        {
            if (_potionHistory == null)
            {
                _potionHistory = (PotionHistoryExtended?)PotionHistoryExtended.LoadScene();
                if (_potionHistoryContainer != null)
                {
                    _potionHistoryContainer.AddChild(_potionHistory);
                    _potionHistoryContainer.GetParent().GetNode<MegaRichTextLabel>("Header")._maxFontSize = 24;
                    _potionHistoryContainer.GetParent().GetNode<MegaRichTextLabel>("Header")._minFontSize = 24;
                }
            }

            _potionHistory?.LoadPotions(tmpPlayer, inventory.Potions.ToList());
            ((Control?)_potionHistoryContainer?.GetParent())?.Show();
        }
        else
        {
            ((Control?)_potionHistoryContainer?.GetParent())?.Hide();
        }
    }

    public void ShowDeckInformation(AlternativeStartingInventory inventory, CharacterModel characterModel)
    {
        LoadDeckInformation(inventory, characterModel);
        GetNode<NBackButton>("DeckInformation/BackButton").Enable();
        _deckInformation?.Show();
    }

    public void HideDeckInformation()
    {
        _deckInformation?.Hide();
    }
}