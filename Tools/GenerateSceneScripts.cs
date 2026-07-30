using System.Text.RegularExpressions;

namespace AlternativeStartingDecks.Scripts;

internal class AssetInformation
{
    public string Id = "";
    public bool Loaded;
    public string Path = "";
    public string Type = "";

    public string VariableName => "var_" + Id;
}

internal class RequiresInformation
{
    public string Id = "";
    public string resourceName = "";
    public string Type = "";
}

internal class NodeInformation
{
    public readonly List<RequiresInformation> Requires = new();

    public string Id = "";
    public bool IsSubResource;
    public bool Loaded;
    public string Name = "";
    public string Parent = "";
    public string Type = "";
    public string VariableName => "var_" + Id;
}

public class GenerateSceneScripts
{
    private static readonly Dictionary<string, AssetInformation> AssetInformation = new();
    private static readonly Dictionary<string, NodeInformation> NodeInformation = new();
    private static readonly Regex RegexExt = new(@"type=""(.*?)"" .*path=""(.*?)"" id=""(.*?)""");
    private static readonly Regex RegexNode = new(@"name=""(.*?)"" .*type=""(.*?)"" parent=""(.*?)""");
    private static readonly Regex RegexSub = new(@"type=""(.*?)"" id=""(.*?)""");
    private static readonly Regex RegexReq = new(@"(.*?) = .*?\(""(.*?)""\)");

    private static string ToTileCase(string stri, string delim, string repl = "")
    {
        return string.Join(repl, stri.Split(delim)
            .Select(str => str.Length > 0 ? char.ToUpper(str[0]) + str.Substring(1) : str));
    }

    public static void Main(string[] args)
    {
        if (args.Length == 0 || !File.Exists(args[0])) return;

        var path = args[0].Replace("\\", "/");
        var prjPath = args[1];
        var name = Path.GetFileNameWithoutExtension(path);
        name = ToTileCase(name, "_");

        var resourcePath = "res://AlternativeStartingDecks/" + path.Split("AlternativeStartingDecks/")[^1];
        var codeNameSpace = string.Join(".", path.Split("AlternativeStartingDecks/")[^1].Split("/")[..^1]);
        codeNameSpace = ToTileCase(codeNameSpace, ".", ".");

        Console.WriteLine(resourcePath);
        Console.WriteLine(prjPath);
        Console.WriteLine(codeNameSpace);
        var outputFile = $"{prjPath}/AlternativeStartingDecksCode/{codeNameSpace.Replace(".", "/")}/{name}.cs";

        ParseTscnFile(path);
        var (loadCode, applyCode) = GenerateAssetCode();


        var code = OutputCsText(codeNameSpace, name, resourcePath, loadCode, applyCode);

        OutputFile(code, outputFile);
    }

    private static void ParseTscnFile(string path)
    {
        AssetInformation.Clear();
        NodeInformation.Clear();

        var lastResource = "";
        foreach (var line in File.ReadAllLines(path))
            if (line.StartsWith("[ext_resource"))
            {
                var groups = RegexExt.Match(line).Groups;

                AssetInformation[groups[3].Value] = new AssetInformation
                {
                    Type = groups[1].Value,
                    Path = groups[2].Value,
                    Id = groups[3].Value
                };
            }

            else if (line.StartsWith("[node"))
            {
                var groups = RegexNode.Match(line).Groups;
                lastResource = groups[3].Value + "/" + groups[1].Value;
                NodeInformation[lastResource] = new NodeInformation
                {
                    Parent = groups[3].Value,
                    Type = groups[2].Value,
                    Name = groups[1].Value
                };
            }
            else if (line.StartsWith("[sub_resource"))
            {
                var groups = RegexSub.Match(line).Groups;
                lastResource = groups[2].Value;

                NodeInformation[lastResource] = new NodeInformation
                {
                    Id = groups[2].Value,
                    Type = groups[1].Value,
                    IsSubResource = true
                };
            }

            else if (line.Contains("ExtResource") || line.Contains("SubResource"))
            {
                // Need to handle []
                var list = new List<string>();
                if (line.Contains("["))
                {
                    var subStr = line.Split("[").Last();
                    subStr = subStr.Split("]").First();

                    var groups = RegexReq.Match(line).Groups;

                    list.AddRange(subStr.Split(",").Select(s => groups[1].Value + " = " + s.Trim()));
                }
                else
                {
                    list.Add(line);
                }

                var type = line.Contains("ExtResource") ? "ExtResource" : "SubResource";
                if (lastResource == "") continue;
                foreach (var item in list)
                {
                    var groups = RegexReq.Match(item).Groups;
                    NodeInformation[lastResource].Requires.Add(new RequiresInformation
                    {
                        resourceName = groups[1].Value,
                        Type = type,
                        Id = groups[2].Value
                    });
                }
            }
    }

    private static Tuple<List<string>, List<string>> GenerateAssetCode()
    {
        var load = new List<string>();
        var apply = new List<string>();
        var variableName = "";

        bool GenerateSubResource(string s, out NodeInformation? subResourceInfo)
        {
            NodeInformation.TryGetValue(s, out subResourceInfo);
            if (subResourceInfo == null) return true;
            if (!subResourceInfo.Loaded)
            {
                load.Add(GetLoadSub(subResourceInfo));
                subResourceInfo.Loaded = true;
            }

            return false;
        }

        foreach (var nodeInformation in NodeInformation.Values)
        foreach (var requiresInformation in nodeInformation.Requires)
        {
            var resourceName = requiresInformation.resourceName;
            var id = requiresInformation.Id;
            var requiresType = requiresInformation.Type;

            if (requiresType == "ExtResource")
            {
                AssetInformation.TryGetValue(id, out var assetInfo);

                if (assetInfo == null) continue;

                if (!assetInfo.Loaded)
                {
                    load.Add(GetLoadAsset(assetInfo));
                    assetInfo.Loaded = true;
                }

                variableName = assetInfo.VariableName;
            }
            else
            {
                if (GenerateSubResource(id, out var subResourceInfo)) continue;

                variableName = subResourceInfo?.VariableName;
            }

            if (nodeInformation is { IsSubResource: true, Loaded: false })
                GenerateSubResource(nodeInformation.Id, out _);

            if (resourceName == "texture")
            {
                apply.Add(GetNodeCode(nodeInformation) + $".Texture = {variableName};");
            }
            else if (resourceName == "script")
            {
                var nodeCode = GetNodeCode(nodeInformation);
                if (nodeInformation.IsSubResource || nodeCode == "result")
                    apply.Add($"{nodeCode} = {nodeCode}.SafelySetScript({variableName})!;");
                else
                    apply.Add($"{nodeCode}.SafelySetScript({variableName});");
            }
            else if (resourceName == "base_font")
            {
                apply.Add($"        {nodeInformation.VariableName}.BaseFont = {variableName};");
            }
            else if (resourceName == "custom_effects")
            {
                apply.Add(GetNodeCode(nodeInformation) + $".CustomEffects.Add({variableName});");
            }
            else if (resourceName.StartsWith("theme_override_fonts"))
            {
                var overrideName = resourceName.Remove(0, "theme_override_fonts/".Length);
                apply.Add(GetNodeCode(nodeInformation) +
                          $".AddThemeFontOverride(\"{overrideName}\", {variableName});");
            }
        }

        return new Tuple<List<string>, List<string>>(load, apply);
    }

    private static string GetLoadSub(NodeInformation subResourceInfo)
    {
        return $"var {subResourceInfo.VariableName} = new {subResourceInfo.Type}();";
    }

    private static string GetLoadAsset(AssetInformation asset)
    {
        return $"var {asset.VariableName} = PreloadManager.Cache.GetAsset<{asset.Type}>(\"{asset.Path}\");";
    }

    private static string GetNodeCode(NodeInformation node)
    {
        if (node.IsSubResource) return node.VariableName;
        if (node is { Parent: "", Name: "" }) return "result";
        return $"result.GetNode<{node.Type}>(\"{node.Parent}/{node.Name}\")";
    }


    private static string OutputCsText(string codePath, string resourceName, string resourcePath,
        List<string> dependencyStrings, List<string> applyDependencyStrings)
    {
        return $$"""
                 /* GENERATED CODE, DO NOT MODIFY */
                 using AlternativeStartingDecks.AlternativeStartingDecksCode.Patches.Utils;
                 using Godot;
                 using MegaCrit.Sts2.Core.Assets;
                 namespace AlternativeStartingDecks.AlternativeStartingDecksCode.{{codePath}};

                 public static class {{resourceName}}
                 {

                      public static Node? LoadScene(string name = "{{resourceName}}")
                      {
                         var result = PreloadManager.Cache.GetScene("{{resourcePath}}")
                            .Instantiate();
                        if (result == null) return null; 
                        
                        // Load Dependencies
                 {{"        " + string.Join("\n        ", dependencyStrings)}}
                        
                        // Apply Dependencies
                 {{"        " + string.Join("\n        ", applyDependencyStrings)}}
                        
                        result.Name = name;
                        
                        return result;
                      }
                     
                 }

                 """;
    }

    private static void OutputFile(string text, string path)
    {
        Directory.CreateDirectory(string.Join("/", path.Split("/")[..^1]));
        File.WriteAllText(path, text);
    }
}