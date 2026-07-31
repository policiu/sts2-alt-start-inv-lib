using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode;

public static class AlternativeStartingDecksGlobals
{
    public static StartingInventory? StartingInventory = null;

    // Needed for when we are populating the deck
    // It happens when both ContextLocal, RunManager, and LobbyRun all are missing
    // the local player's NetId
    public static ulong NetId = 0;

    /// <summary>
    ///     List of Starting Inventories by Character
    /// </summary>
    public static Dictionary<string, Dictionary<string, StartingInventory>>
        AlternativeStartingInventoryByCharacter { get; set; } = new();
}