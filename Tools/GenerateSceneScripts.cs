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

internal class SubResourceInformation
{
    public readonly Dictionary<string, string> Requires = new();
    public string Name = "";
    public string Parent = "";
    public string Type = "";
}

public class GenerateSceneScripts
{
    private static readonly HashSet<string> AllowedSub = ["texture", "script"];

    private static readonly Dictionary<string, AssetInformation> AssetInformation = new();
    private static readonly Dictionary<string, SubResourceInformation> SubResourceInformation = new();
    private static readonly Regex RegexExt = new(@"type=""(.*?)"" .*path=""(.*?)"" id=""(.*?)""");
    private static readonly Regex RegexSub = new(@"name=""(.*?)"" .*type=""(.*?)"" parent=""(.*?)""");
    private static readonly Regex RegexReq = new(@"(.*?) = ExtResource\(""(.*?)""\)");

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
        SubResourceInformation.Clear();

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
                var groups = RegexSub.Match(line).Groups;
                lastResource = groups[3].Value + "/" + groups[1].Value;
                SubResourceInformation[lastResource] = new SubResourceInformation
                {
                    Parent = groups[3].Value,
                    Type = groups[2].Value,
                    Name = groups[1].Value
                };
            }
            else if (line.StartsWith("[subresource"))
            {
                lastResource = "";
            }

            else if (line.Contains("ExtResource"))
            {
                if (lastResource == "") continue;
                var groups = RegexReq.Match(line).Groups;
                SubResourceInformation[lastResource].Requires[groups[1].Value] = groups[2].Value;
            }
    }

    private static Tuple<List<string>, List<string>> GenerateAssetCode()
    {
        var load = new List<string>();
        var apply = new List<string>();

        foreach (var subResource in SubResourceInformation.Values)
        foreach (var (resourceName, id) in subResource.Requires)
        {
            if (!AllowedSub.Contains(resourceName)) continue;

            AssetInformation.TryGetValue(id, out var assetInfo);

            if (assetInfo == null) continue;

            if (!assetInfo.Loaded)
            {
                load.Add(GetLoadAsset(assetInfo));
                assetInfo.Loaded = true;
            }

            if (resourceName == "texture")
                apply.Add(GetNodeCode(subResource) + $".Texture = {assetInfo.VariableName};");
            else if (resourceName == "script")
                apply.Add(GetNodeCode(subResource) + $".SetScript({assetInfo.VariableName});");
        }

        return new Tuple<List<string>, List<string>>(load, apply);
    }

    private static string GetLoadAsset(AssetInformation asset)
    {
        return $"        var {asset.VariableName} = PreloadManager.Cache.GetAsset<{asset.Type}>(\"{asset.Path}\");";
    }

    private static string GetNodeCode(SubResourceInformation subResource)
    {
        return $"        result.GetNode<{subResource.Type}>(\"{subResource.Parent}/{subResource.Name}\")";
    }


    private static string OutputCsText(string codePath, string resourceName, string resourcePath,
        List<string> dependencyStrings, List<string> applyDependencyStrings)
    {
        return $$"""
                 /* GENERATED CODE, DO NOT MODIFY */
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
                 {{string.Join("\n", dependencyStrings)}}
                        
                        // Apply Dependencies
                 {{string.Join("\n", applyDependencyStrings)}}
                        
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