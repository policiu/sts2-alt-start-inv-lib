using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;

public class PlayerInventoryEventArgs : EventArgs
{
    public required Player Player { get; set; }
}

public class AddContentEventArgs : EventArgs
{
    public required Control Owner { get; set; }
}

/// <summary>
///     An Inventory which can replace the starting information of a player
/// </summary>
/// <param name="defaultCharacterOptions">CharacterModel to default values to</param>
/// <param name="id">Unique id of the inventory. Must be distinct per Character.</param>
public class StartingInventory(CharacterModel defaultCharacterOptions, string id)
{
    private readonly string _unlockText = "";

    /// <summary>
    ///     Starting Hp
    /// </summary>
    public int Hp { get; init; } = defaultCharacterOptions.StartingHp;

    /// <summary>
    ///     Starting Gold
    /// </summary>
    public int Gold { get; init; } = defaultCharacterOptions.StartingGold;

    /// <summary>
    ///     Starting Cards
    /// </summary>
    public IEnumerable<CardModel> Cards { get; init; } = defaultCharacterOptions.StartingDeck;

    /// <summary>
    ///     Starting Relics for a run
    /// </summary>
    public IEnumerable<RelicModel> Relics { get; init; } = defaultCharacterOptions.StartingRelics;

    /// <summary>
    ///     Starting Potions.
    ///     Note: Discards over max potion limit
    /// </summary>
    public IEnumerable<PotionModel> Potions { get; init; } = defaultCharacterOptions.StartingPotions;

    /// <summary>
    ///     Epochs to lock the deck behind.
    ///     Note: Deck is hidden while Epochs are not revealed
    /// </summary>
    public List<EpochModel> RequiredEpochs { get; init; } = new();


    /// <summary>
    ///     What to show in the flavor text of the deck
    /// </summary>
    public string Description { get; init; } =
        new LocString("characters", defaultCharacterOptions.CharacterSelectDesc).GetFormattedText();

    /// <summary>
    ///     What to show in the title of the deck
    /// </summary>
    public string Name { get; init; } =
        new LocString("characters", defaultCharacterOptions.CharacterSelectTitle).GetFormattedText();

    /// <summary>
    ///     Unique Id per character
    /// </summary>
    public string Id { get; } = id;

    /// <summary>
    ///     Text shown while deck is locked
    /// </summary>
    public string UnlockText
    {
        get
        {
            if (_unlockText != string.Empty) return _unlockText;
            var baseStr = new LocString("deck_panel_info", "UNLOCK_DEFAULT_STRING").GetRawText();
            var epochStrings = RequiredEpochs.Where(epoch => !SaveManager.Instance.Progress.IsEpochObtained(epoch.Id))
                .Select(epoch => epoch.Title.GetFormattedText());
            return string.Format(baseStr,
                string.Join(", ", epochStrings));
        }
        init => _unlockText = value;
    }


    public bool IsLocked =>
        RequiredEpochs.Any(requiredEpoch => !SaveManager.Instance.Progress.IsEpochRevealed(requiredEpoch.Id));

    public bool IsHidden
    {
        get
        {
            return RequiredEpochs.Any(requiredEpoch =>
            {
                var serializableEpoch =
                    SaveManager.Instance.Progress._epochs.FirstOrDefault(e => e.Id == requiredEpoch.Id);
                return serializableEpoch is null or { State: EpochState.NoSlot };
            });
        }
    }

    #region Events

    /// <summary>
    ///     Called before setting the Local Player's starting hp and gold
    /// </summary>
    public event EventHandler<PlayerInventoryEventArgs>? BeforePlayerDataApply;

    /// <summary>
    ///     Called after setting the Local Player's starting hp and gold
    /// </summary>
    public event EventHandler<PlayerInventoryEventArgs>? AfterPlayerDataApply;

    public void OnAfterPlayerDataApply(PlayerInventoryEventArgs eventArgs)
    {
        AfterPlayerDataApply?.Invoke(this, eventArgs);
    }

    public void OnBeforePlayerDataApply(PlayerInventoryEventArgs eventArgs)
    {
        BeforePlayerDataApply?.Invoke(this, eventArgs);
    }

    /// <summary>
    ///     Called before setting the Local Player's starting deck, relics, and potions
    /// </summary>
    public event EventHandler<PlayerInventoryEventArgs>? BeforePlayerInventoryApply;

    /// <summary>
    ///     Called after setting the Local Player's starting deck, relics, and potions
    /// </summary>
    public event EventHandler<PlayerInventoryEventArgs>? AfterPlayerInventoryApply;

    public void OnAfterPlayerInventoryApply(PlayerInventoryEventArgs eventArgs)
    {
        AfterPlayerInventoryApply?.Invoke(this, eventArgs);
    }

    public void OnBeforePlayerInventoryApply(PlayerInventoryEventArgs eventArgs)
    {
        BeforePlayerInventoryApply?.Invoke(this, eventArgs);
    }

    /// <summary>
    ///     Called when Setting up the Deck Information Display on the Character Select Screen
    /// </summary>
    public event EventHandler<AddContentEventArgs>? DisplayContentForCharacterSelect;

    public void OnDisplayContentForCharacterSelect(AddContentEventArgs eventArgs)
    {
        DisplayContentForCharacterSelect?.Invoke(this, eventArgs);
    }

    #endregion
}