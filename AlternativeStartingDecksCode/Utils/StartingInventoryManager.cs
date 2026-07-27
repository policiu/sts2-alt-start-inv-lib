namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;

public static class StartingInventoryManager
{
    /// <summary>
    ///     Add a new starting inventory to add/replace to a character
    /// </summary>
    /// <param name="characterClassName">Name of Character, use nameof(class)</param>
    /// <param name="inventoryDescriptor">Unique descriptor of the deck to prevent duplicates. Not shown to the player.</param>
    /// <param name="inventory">Inventory to add</param>
    public static void AddNewInventoryForCharacter(
        string characterClassName,
        string inventoryDescriptor,
        AlternativeStartingInventory inventory)
    {
        AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter.TryAdd(characterClassName,
            new Dictionary<string, AlternativeStartingInventory>());

        AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter[characterClassName][inventoryDescriptor] =
            inventory;
    }
}