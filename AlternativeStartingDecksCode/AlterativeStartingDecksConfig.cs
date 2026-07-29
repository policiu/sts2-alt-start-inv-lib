using BaseLib.Config;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

public readonly struct AlternativeStartingInventory(CharacterModel characterModel, string id)
{
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