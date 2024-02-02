#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ScriptingDefineSymbolsSection
{
	public static bool HasLoaded => !string.IsNullOrEmpty(scriptingDefines);

	private static BuildPlayerOptions buildOptions => GeneralPreBuildWindow.buildOptions;
	private static ScriptingDefineSymbolsScriptableObject symbolsData => ScriptingDefineSymbolsScriptableObject.Instance;

	private static GUIStyle mainHeader;
	private static GUIStyle helpHeader;

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
		mainHeader = new GUIStyle(GUI.skin.label);

		Texture2D newBackground = EditorWindowUtility.CreateHeaderBackground(new Color(0.8f, 0.1f, 0.4f), 300, 20);
		mainHeader.normal.background = newBackground;
		mainHeader.alignment = TextAnchor.MiddleCenter;

		mainHeader.fontSize += 5;

		helpHeader = new GUIStyle(mainHeader);
		helpHeader.normal.background = EditorWindowUtility.CreateHeaderBackground(new Color(0.2f, 0.7f, 0.4f), 300, 20);
	}

	public static void DrawSection()
	{
		if (mainHeader == null)
		{
			LoadStyles();
		}

		if (string.IsNullOrEmpty(scriptingDefines))
		{
			return;
		}

		GUILayout.BeginHorizontal();
		float width = Screen.width * 0.8f;
		width = width < 300 ? 300 : width;

		GUILayout.Space(width * 0.12f);
		GUILayout.BeginVertical();

		DrawDefineSymbols(width);
		GUILayout.Space(15);
		DrawHelp(width);

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private static void DrawDefineSymbols(float width)
	{
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));
		GUILayout.Label("Scripting Define Symbols", mainHeader);
		GUILayout.Space(5);


		if (originalScriptingDefines != scriptingDefines)
		{
			GUILayout.Box("[!!!] You've changed the Scripting Define Symbols, Press Apply Settings to make these changes persistent. [!!!]");
		}

		EditorWindowUtility.DrawButton("Apply Settings", width, () =>
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefines);
			originalScriptingDefines = scriptingDefines;
		});

		for (int i = 0; i < scriptingSymbols.Length; ++i)
		{
			DrawSymbolControl(scriptingSymbols[i].symbol, scriptingSymbols[i].label);
		}

		GUILayout.EndVertical();
	}

	private static void DrawHelp(float width)
	{
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));

		GUILayout.Label($"Help", helpHeader);
		EditorWindowUtility.DrawButton($"Open scriptable object for defines", width, () =>
		{
			Selection.activeObject = symbolsData;
		});

		GUILayout.EndVertical();
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