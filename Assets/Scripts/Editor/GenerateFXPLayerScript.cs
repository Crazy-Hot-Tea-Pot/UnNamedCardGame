using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;

public class GenerateFXPLayerScript
{
#if UNITY_EDITOR
    [MenuItem("Tools/Generate FXPlayerToLinkAnimation Script")]
    public static void GenerateFXPlayerScript()
    {
        // Path to SoundEnums file
        string enumFilePath = "Assets/Scripts/Generated Scripts/SoundEnums.cs";
        string outputScriptPath = "Assets/Scripts/Generated Scripts/FXPlayerToLinkAnimation.cs";

        // Read the SoundFX enum
        string[] enumLines = File.ReadAllLines(enumFilePath);
        var soundFXNames = ParseEnumValues(enumLines, "SoundFX");

        if (soundFXNames == null || soundFXNames.Count == 0)
        {
            Debug.LogError("SoundFX enum not found or empty. Ensure it's generated and located at: " + enumFilePath);
            return;
        }

        Debug.Log($"Parsed SoundFX names: {string.Join(", ", soundFXNames)}");

        // Read existing FXPlayerToLinkAnimation script (if it exists)
        StringBuilder outputBuilder = new StringBuilder();
        bool scriptExists = File.Exists(outputScriptPath);
        string[] existingScript = scriptExists ? File.ReadAllLines(outputScriptPath) : null;

        // Analyze existing methods
        var existingMethods = ParseExistingMethods(existingScript);

        // Generate the new script content
        outputBuilder.AppendLine("using UnityEngine;");
        outputBuilder.AppendLine();
        outputBuilder.AppendLine("public class FXPlayerToLinkAnimation : MonoBehaviour");
        outputBuilder.AppendLine("{");

        foreach (string sound in soundFXNames)
        {
            if (existingMethods.ContainsKey(sound))
            {
                // Preserve existing method
                outputBuilder.AppendLine(existingMethods[sound]);
            }
            else
            {
                // Add new method
                outputBuilder.AppendLine($"    public void PlaySound_{sound}()");
                outputBuilder.AppendLine("    {");
                outputBuilder.AppendLine($"        SoundManager.PlayFXSound(SoundFX.{sound});");
                outputBuilder.AppendLine("    }");
            }
        }

        // Optionally, handle methods for removed sounds
        foreach (var removedMethod in existingMethods.Keys)
        {
            if (!soundFXNames.Contains(removedMethod))
            {
                outputBuilder.AppendLine($"    // WARNING: Method PlaySound_{removedMethod}() is no longer linked to any sound.");
                outputBuilder.AppendLine($"    // public void PlaySound_{removedMethod}() {{ /* Legacy code */ }}");
            }
        }

        outputBuilder.AppendLine("}");

        // Write the script to the output file
        File.WriteAllText(outputScriptPath, outputBuilder.ToString());
        AssetDatabase.Refresh();

        Debug.Log("FXPlayerToLinkAnimation generated successfully.");
    }

    private static HashSet<string> ParseEnumValues(string[] lines, string enumName)
    {
        bool inEnum = false;
        HashSet<string> values = new HashSet<string>();

        foreach (string line in lines)
        {
            if (line.Contains($"enum {enumName}"))
            {
                inEnum = true;
                continue;
            }

            if (inEnum)
            {
                if (line.Contains("}")) break; // End of enum
                string value = line.Trim().Split(',')[0].Trim(); // Get value before comma
                if (!string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-zA-Z_]\w*$"))
                {
                    values.Add(value);
                }
            }
        }

        return values;
    }

    private static Dictionary<string, string> ParseExistingMethods(string[] scriptLines)
    {
        if (scriptLines == null) return new Dictionary<string, string>();

        Dictionary<string, string> methods = new Dictionary<string, string>();
        StringBuilder currentMethod = new StringBuilder();
        string methodName = null;

        foreach (string line in scriptLines)
        {
            if (line.TrimStart().StartsWith("public void PlaySound_"))
            {
                // Extract method name
                var match = Regex.Match(line, @"PlaySound_(\w+)\(");
                if (match.Success) methodName = match.Groups[1].Value;
            }

            if (methodName != null)
            {
                currentMethod.AppendLine(line);

                // End of method
                if (line.Contains("}"))
                {
                    methods[methodName] = currentMethod.ToString();
                    currentMethod.Clear();
                    methodName = null;
                }
            }
        }

        return methods;
    }
#endif
}
