using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode.Utils;

public static class StartingInventoryManager
{
    /// <summary>
    ///     Called when mods should add/register new inventories for characters.
    /// </summary>
    public static event EventHandler? StartingInventoryInit;

    /// <summary>
    ///     Add a new starting inventory to add/replace to a character
    /// </summary>
    /// <param name="characterModel">Name of Character, use nameof(class)</param>
    /// <param name="inventory">Inventory to add</param>
    public static void AddNewInventoryForCharacter(
        CharacterModel characterModel,
        StartingInventory inventory)
    {
        var characterClassName = characterModel.GetType().Name;
        AlternativeStartingInventoryGlobals.AlternativeStartingInventoryByCharacter.TryAdd(characterClassName,
            new Dictionary<string, StartingInventory>());

        AlternativeStartingInventoryGlobals.AlternativeStartingInventoryByCharacter[characterClassName][inventory.Id] =
            inventory;
    }


    public static List<StartingInventory> GetStartingInventoriesForCharacter(CharacterModel character)
    {
        return GetStartingInventoriesForCharacter(character.GetType().Name);
    }

    public static List<StartingInventory> GetStartingInventoriesForCharacter(
        string characterClassName)
    {
        if (AlternativeStartingInventoryGlobals.AlternativeStartingInventoryByCharacter.TryGetValue(characterClassName,
                out var inventories))
            return inventories.Values.ToList();
        return [];
    }

    public static bool RemoveStartingInventoryForCharacter(CharacterModel character, string inventoryId)
    {
        AlternativeStartingInventoryGlobals.AlternativeStartingInventoryByCharacter.TryGetValue(
            character.GetType().Name, out var inventories);

        if (inventories?.ContainsKey(inventoryId) ?? false)
        {
            inventories.Remove(inventoryId);
            return true;
        }

        return false;
    }

    internal static void LoadDefaultInventoryForAllCharacters()
    {
        foreach (var character in ModelDb.AllCharacters)
            AddNewInventoryForCharacter(character, new StartingInventory(character, "default"));
    }

    internal static void LoadAllInventories()
    {
        if (GetStartingInventoriesForCharacter(ModelDb.Character<Ironclad>()).Any(inv => inv.Id == "default")) return;
        LoadDefaultInventoryForAllCharacters();
        StartingInventoryInit?.Invoke(null, EventArgs.Empty);
    }
}