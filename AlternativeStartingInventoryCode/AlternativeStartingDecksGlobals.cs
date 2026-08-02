using AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode;

public static class AlternativeStartingInventoryGlobals
{
    public static StartingInventory? StartingInventory = null;

    // Needed for when we are populating the deck
    // It happens when both ContextLocal, RunManager, and LobbyRun all are missing
    // the local player's NetId
    public static ulong NetId = 0;

    /// <summary>
    ///     List of Starting Inventories by Character
    /// </summary>
    internal static Dictionary<string, Dictionary<string, StartingInventory>>
        AlternativeStartingInventoryByCharacter { get; set; } = new();
}