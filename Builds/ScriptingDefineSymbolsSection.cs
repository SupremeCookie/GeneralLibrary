#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ScriptingDefineSymbolsSection
{
	public static bool HasLoaded => !string.IsNullOrEmpty(scriptingDefines);

	private static BuildPlayerOptions buildOptions => GeneralPreBuildWindow.buildOptions;
	private static ScriptingDefineSymbolsScriptableObject symbolsData => ScriptingDefineSymbolsScriptableObject.Instance;

	private static GUIStyle header;

	private static string scriptingDefines;
	private static string originalScriptingDefines;

	private static ScriptingSymbol[] scriptingSymbols => symbolsData.GetSymbols();

	public static void Load()
	{
		if (buildOptions.targetGroup == BuildTargetGroup.Unknown)
		{
			var copyBuildOptions = buildOptions;
			copyBuildOptions.targetGroup = BuildTargetGroup.Standalone;
			GeneralPreBuildWindow.buildOptions = copyBuildOptions;
		}

		scriptingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);
		originalScriptingDefines = scriptingDefines;
		LoadStyles();
	}

	private static void LoadStyles()
	{
		header = new GUIStyle(GUI.skin.label);

		const int width = 300;
		const int height = 20;
		Texture2D newBackground = new Texture2D(width, height);

		Color pixelColor = new Color(0.8f, 0.1f, 0.4f);
		Color[] pixels = new Color[newBackground.width * newBackground.height];
		for (int i = 0; i < pixels.Length; ++i)
		{
			if ((i % width) == 0 || (i % width) == (width - 1)
				|| i < width || i >= (width * (height - 1)))
			{
				pixels[i] = Color.black;
				continue;
			}

			pixels[i] = pixelColor;
		}

		newBackground.SetPixels(pixels);
		newBackground.Apply();

		header.normal.background = newBackground;
		header.alignment = TextAnchor.MiddleCenter;

		header.fontSize += 5;
	}

	public static void DrawSection()
	{
		if (header == null)
		{
			LoadStyles();
		}

		if (string.IsNullOrEmpty(scriptingDefines))
		{
			return;
		}

		GUILayout.BeginHorizontal();
		float width = Screen.width * 0.6f;
		width = width < 300 ? 300 : width;

		GUILayout.Space(width * 0.33f);
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));

		GUILayout.Label("Scripting Define Symbols", header);
		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		float buttonWidth = width * 0.5f;
		GUILayout.Space(buttonWidth * 0.5f);
		if (GUILayout.Button($"Apply Settings", GUILayout.MaxWidth(buttonWidth)))
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefines);
			originalScriptingDefines = scriptingDefines;
		}
		GUILayout.EndHorizontal();

		if (originalScriptingDefines != scriptingDefines)
		{
			GUILayout.Box("[!!!] You've changed the Scripting Define Symbols, Press Apply Settings to make these changes persistent. [!!!]");
		}


		for (int i = 0; i < scriptingSymbols.Length; ++i)
		{
			DrawSymbolControl(scriptingSymbols[i].symbol, scriptingSymbols[i].label);
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private static void DrawSymbolControl(string symbol, string label)
	{
		bool valueIsEnabled = scriptingDefines.Contains(symbol);
		var newValueIsEnabled = GUILayout.Toggle(valueIsEnabled, label);

		if (valueIsEnabled != newValueIsEnabled)
		{
			if (!newValueIsEnabled)
			{
				var startIndexOfString = scriptingDefines.IndexOf(symbol);
				scriptingDefines = scriptingDefines.Remove(startIndexOfString, symbol.Length);
			}
			else
			{
				string prefix = "";
				if (scriptingDefines[scriptingDefines.Length - 1] != ';')
				{
					prefix = ";";
				}

				scriptingDefines += $"{prefix}{symbol}";
			}
		}
	}
}
#endif