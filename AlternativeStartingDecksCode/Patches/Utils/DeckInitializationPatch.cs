using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch]
internal class DeckInitializationPatch
{
    [HarmonyPatch(typeof(ModelDb), "InitIds")]
    [HarmonyPrefix]
    private static void LatePostInit()
    {
        StartingInventoryManager.LoadDefaultInventoryForAllCharacters();

        // Load Some Examples
        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.First(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.First(), "example")
            {
                Cards = [ModelDb.Card<Anger>()]
            });

        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.First(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.First(), "example2",
                EpochModel.Get<Ironclad2Epoch>())
            {
                Gold = 20,
                Hp = 3,
                Name = "The Run!",
                Description = "As hard as possible",
                Relics = [ModelDb.Relic<Anchor>(), ModelDb.Relic<Pear>()],
                Potions = [ModelDb.Potion<HeartOfIron>()]
            });


        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.First(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.First(), "example5"
            )
            {
                Gold = 20,
                Hp = 3,
                Name = "The Run!",
                Description = "As hard as possible",
                Relics = [ModelDb.Relic<Anchor>(), ModelDb.Relic<Pear>()],
                Potions = [ModelDb.Potion<HeartOfIron>()]
            });


        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.First(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.First(), "example4")
            {
                Gold = 20,
                Hp = 3,
                Name = "All Cards!",
                Description = "As hard as possible",
                Relics = ModelDb.AllRelics,
                Potions = [ModelDb.Potion<HeartOfIron>()],
                Cards = ModelDb.AllCards
            });

        StartingInventoryManager.AddNewInventoryForCharacter(ModelDb.AllCharacters.Last(),
            new AlternativeStartingInventory(ModelDb.AllCharacters.Last(), "example-3")
            {
                Name = "Example"
            });
    }
}