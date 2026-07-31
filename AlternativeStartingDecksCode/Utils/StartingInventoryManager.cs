using MegaCrit.Sts2.Core.Models;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;

public static class StartingInventoryManager
{
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
        AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter.TryAdd(characterClassName,
            new Dictionary<string, StartingInventory>());

        AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter[characterClassName][inventory.Id] =
            inventory;
    }


    public static List<StartingInventory> GetStartingInventoriesForCharacter(CharacterModel character)
    {
        return GetStartingInventoriesForCharacter(character.GetType().Name);
    }

    public static List<StartingInventory> GetStartingInventoriesForCharacter(
        string characterClassName)
    {
        return AlternativeStartingDecksConfig.AlternativeStartingDecksByCharacter[characterClassName].Values.ToList();
    }

    internal static void LoadDefaultInventoryForAllCharacters()
    {
        foreach (var character in ModelDb.AllCharacters)
            AddNewInventoryForCharacter(character, new StartingInventory(character, "default"));
    }
}