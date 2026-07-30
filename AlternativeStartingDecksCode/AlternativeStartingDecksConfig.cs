using BaseLib.Config;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

public class AlternativeStartingInventory(CharacterModel characterModel, string id, EpochModel? requiredEpoch = null)
{
    private readonly string _overrideText;
    private EpochModel? _requiredEpoch = requiredEpoch;
    public int Hp { get; init; } = characterModel.StartingHp;
    public int Gold { get; init; } = characterModel.StartingGold;
    public IEnumerable<CardModel> Cards { get; init; } = characterModel.StartingDeck;
    public IEnumerable<RelicModel> Relics { get; init; } = characterModel.StartingRelics;
    public IEnumerable<PotionModel> Potions { get; init; } = characterModel.StartingPotions;

    public string Description { get; init; } =
        new LocString("characters", characterModel.CharacterSelectDesc).GetFormattedText();

    public string Name { get; init; } =
        new LocString("characters", characterModel.CharacterSelectTitle).GetFormattedText();

    public string Id { get; } = id;

    public string UnlockText
    {
        get
        {
            if (_overrideText != string.Empty) return _overrideText;
            var loc = requiredEpoch?.UnlockInfo;
            if (loc != null)
            {
                loc.Add("IsRevealed", false);
                return loc.GetFormattedText();
            }

            return string.Empty;
        }
        init => _overrideText = value;
    }

    public string RequiredEpochId { get; } = requiredEpoch?.Id ?? string.Empty;

    public bool IsLocked
    {
        get
        {
            if (RequiredEpochId != string.Empty) return !SaveManager.Instance.Progress.IsEpochObtained(RequiredEpochId);

            return false;
        }
    }
}

public static class AlternativeStartingDecksLogger
{
    public static void Warn(string message)
    {
        ModConfig.ModConfigLogger.Warn("[AlternativeStartingDecks] " + message);
    }
}

internal class AlternativeStartingDecksConfig : SimpleModConfig
{
    /// <summary>
    ///     List of Starting Inventories by Character
    /// </summary>
    public static Dictionary<string, Dictionary<string, AlternativeStartingInventory>>
        AlternativeStartingDecksByCharacter { get; set; } = new();
}