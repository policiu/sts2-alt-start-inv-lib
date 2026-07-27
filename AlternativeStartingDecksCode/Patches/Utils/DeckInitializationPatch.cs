using AlternativeStartingDecks.AlternativeStartingDecksCode.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

[HarmonyPatch]
internal class DeckInitializationPatch
{
    [HarmonyPatch(typeof(ModelDb), "InitIds")]
    [HarmonyPrefix]
    private static void LatePostInit()
    {
        StartingInventoryManager.AddNewInventoryForCharacter(nameof(Ironclad), "default",
            new AlternativeStartingInventory([ModelDb.Card<DefendNecrobinder>()], potions: [],
                relics: [ModelDb.Relic<Pear>()]));
    }
}