using Godot;
using Object = Godot.GodotObject;

namespace AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;

public static class GodotUtils
{
    public static T? SafelySetScript<T>(this T obj, Resource resource) where T : Object
    {
        var godotObjectId = obj.GetInstanceId();
        // Replaces old C# instance with a new one. Old C# instance is disposed.
        obj.SetScript(resource);
        // Get the new C# instance
        return Object.InstanceFromId(godotObjectId) as T;
    }

    public static T? SafelySetScript<T>(this T obj, string resource) where T : Object
    {
        return SafelySetScript<T>(obj, ResourceLoader.Load(resource));
    }
}