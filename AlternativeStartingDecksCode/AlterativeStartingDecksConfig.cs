using BaseLib.Config;
using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

public readonly struct AlternativeStartingInventory
{
    public readonly IEnumerable<CardModel> Cards = [];
    public readonly IEnumerable<RelicModel> Relics = [];
    public readonly IEnumerable<PotionModel> Potions = [];

    public AlternativeStartingInventory(IEnumerable<CardModel> cards, IEnumerable<RelicModel> relics,
        IEnumerable<PotionModel> potions)
    {
        Cards = cards;
        Relics = relics;
        Potions = potions;
    }

    public void Deconstruct(out IEnumerable<CardModel> cards, out IEnumerable<RelicModel> relics,
        out IEnumerable<PotionModel> potions)
    {
        cards = Cards;
        relics = Relics;
        potions = Potions;
    }
}

public static class AlternativeStartingDecksLogger
{
    public static void Warn(string message)
    {
        ModConfig.ModConfigLogger.Warn("[AlternativeStartingDecks]" + message);
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