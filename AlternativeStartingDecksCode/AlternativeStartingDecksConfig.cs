using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using BaseLib.Config;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

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
    public static Dictionary<string, Dictionary<string, StartingInventory>>
        AlternativeStartingDecksByCharacter { get; set; } = new();
}