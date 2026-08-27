using BaseLib.Config;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode;

public static class AlternativeStartingInventoryLogger
{
    /// <inheritdoc cref="M:BaseLib.Config.ModConfig.ModConfigLogger.Warn(System.String,System.Boolean)" />
    public static void Warn(string message, bool showInGui = false)
    {
        AlternativeStartingInventoryLib.Logger.Warn(message);
        if (!showInGui || ModConfig.ModConfigLogger.PendingUserMessages.Contains(message))
            return;
        ModConfig.ModConfigLogger.PendingUserMessages.Add(message);
    }

    /// <inheritdoc cref="M:BaseLib.Config.ModConfig.ModConfigLogger.Warn(System.String,System.Boolean)" />
    public static void Error(string message, bool showInGui = true)
    {
        AlternativeStartingInventoryLib.Logger.Error(message);
        if (!showInGui || ModConfig.ModConfigLogger.PendingUserMessages.Contains(message))
            return;
        ModConfig.ModConfigLogger.PendingUserMessages.Add(message);
    }

    /// <inheritdoc cref="M:BaseLib.Config.ModConfig.ModConfigLogger.Warn(System.String,System.Boolean)" />
    public static void Info(string message, bool showInGui = false)
    {
        AlternativeStartingInventoryLib.Logger.Info(message);
        if (!showInGui || ModConfig.ModConfigLogger.PendingUserMessages.Contains(message))
            return;
        ModConfig.ModConfigLogger.PendingUserMessages.Add(message);
    }

    public static void Debug(string message)
    {
        AlternativeStartingInventoryLib.Logger.Debug(message);
    }
}

internal class AlternativeStartingInventoryConfig : SimpleModConfig
{
    public static int MaxItemsToShow { get; set; } = 25;
}