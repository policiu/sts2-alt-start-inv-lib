using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using BaseLib.Config;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

public static class AlternativeStartingDecksLogger
{
    /// <inheritdoc cref="M:BaseLib.Config.ModConfig.ModConfigLogger.Warn(System.String,System.Boolean)" />
    public static void Warn(string message, bool showInGui = false)
    {
        AlternativeStartingDeckLib.Logger.Warn(message);
        if (!showInGui || ModConfig.ModConfigLogger.PendingUserMessages.Contains(message))
            return;
        ModConfig.ModConfigLogger.PendingUserMessages.Add(message);
    }

    /// <inheritdoc cref="M:BaseLib.Config.ModConfig.ModConfigLogger.Warn(System.String,System.Boolean)" />
    public static void Error(string message, bool showInGui = true)
    {
        AlternativeStartingDeckLib.Logger.Error(message);
        if (!showInGui || ModConfig.ModConfigLogger.PendingUserMessages.Contains(message))
            return;
        ModConfig.ModConfigLogger.PendingUserMessages.Add(message);
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