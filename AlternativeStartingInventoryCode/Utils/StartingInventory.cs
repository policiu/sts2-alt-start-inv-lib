using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;

public class StartingInventory(CharacterModel characterModel, string id)
{
    private readonly string _overrideText = string.Empty;
    private readonly string _unlockText = "";

    public int Hp { get; init; } = characterModel.StartingHp;
    public int Gold { get; init; } = characterModel.StartingGold;
    public IEnumerable<CardModel> Cards { get; init; } = characterModel.StartingDeck;
    public IEnumerable<RelicModel> Relics { get; init; } = characterModel.StartingRelics;
    public IEnumerable<PotionModel> Potions { get; init; } = characterModel.StartingPotions;

    public List<EpochModel> RequiredEpochs { get; init; } = new();

    public string Description { get; init; } =
        new LocString("characters", characterModel.CharacterSelectDesc).GetFormattedText();

    public string Name { get; init; } =
        new LocString("characters", characterModel.CharacterSelectTitle).GetFormattedText();

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
}