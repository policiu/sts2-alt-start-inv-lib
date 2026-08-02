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

/// <summary>
///     An Inventory which can replace the starting information of a player
/// </summary>
/// <param name="defaultCharacterOptions">CharacterModel to default values to</param>
/// <param name="id">Unique id of the inventory. Must be distinct per Character.</param>
public class StartingInventory(CharacterModel defaultCharacterOptions, string id)
{
    private readonly string _unlockText = "";

    public int Hp { get; init; } = defaultCharacterOptions.StartingHp;
    public int Gold { get; init; } = defaultCharacterOptions.StartingGold;
    public IEnumerable<CardModel> Cards { get; init; } = defaultCharacterOptions.StartingDeck;
    public IEnumerable<RelicModel> Relics { get; init; } = defaultCharacterOptions.StartingRelics;
    public IEnumerable<PotionModel> Potions { get; init; } = defaultCharacterOptions.StartingPotions;

    public List<EpochModel> RequiredEpochs { get; init; } = new();


    public string Description { get; init; } =
        new LocString("characters", defaultCharacterOptions.CharacterSelectDesc).GetFormattedText();

    public string Name { get; init; } =
        new LocString("characters", defaultCharacterOptions.CharacterSelectTitle).GetFormattedText();

    public string Id { get; } = id;

    public string UnlockText
    {
        get
        {
            if (_unlockText != string.Empty) return _unlockText;
            return string.Format(new LocString("deck_panel_info", "UNLOCK_DEFAULT_STRING").GetFormattedText(),
                string.Join(",",
                    RequiredEpochs.Where(epoch => !SaveManager.Instance.Progress.IsEpochObtained(epoch.Id))
                        .Select(epoch => epoch.Title.GetFormattedText())));
        }
        init => _unlockText = value;
    }


    public bool IsLocked =>
        RequiredEpochs.Any(requiredEpoch => !SaveManager.Instance.Progress.IsEpochObtained(requiredEpoch.Id));

    public bool IsHidden =>
        RequiredEpochs.Any(requiredEpoch => !SaveManager.Instance.Progress.IsEpochRevealed(requiredEpoch.Id));

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
}