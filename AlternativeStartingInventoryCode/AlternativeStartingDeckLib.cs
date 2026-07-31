using System.Reflection;
using BaseLib.Config;
using BaseLib.Utils;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace AlternativeStartingInventory.AlternativeStartingInventoryCode;

[ModInitializer(nameof(Initialize))]
public partial class AlternativeStartingInventoryLib : Node
{
    public const string ModId = "AlternativeStartingInventory"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

        Harmony harmony = new(ModId);

        harmony.PatchAll();

        ModConfigRegistry.Register(ModId, new AlternativeStartingInventoryConfig());
        CustomLocTableManager.Register("deck_panel_info");
    }
}
