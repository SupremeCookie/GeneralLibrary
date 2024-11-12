#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ScriptingDefineSymbolsSection
{
	public static bool HasLoaded { get; private set; } = false;
	public static void ResetHasLoaded() { HasLoaded = false; }

	private static BuildPlayerOptions buildOptions => GeneralPreBuildWindow.buildOptions;
	private static ScriptingDefineSymbolsScriptableObject symbolsData => ScriptingDefineSymbolsScriptableObject.Instance;

	private static GUIStyle mainHeader;
	private static GUIStyle helpHeader;
	private static GUIStyle applyBox;

	private static string scriptingDefines;
	private static string originalScriptingDefines;

	private static ScriptingSymbol[] scriptingSymbols => symbolsData.GetSymbols();


	public static void Load()
	{
		LoadBuildOptions();
		LoadScriptingDefines();
		LoadStyles();

		HasLoaded = true;
	}

	private static void LoadBuildOptions()
	{
		var copyBuildOptions = buildOptions;
		copyBuildOptions.targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
		GeneralPreBuildWindow.buildOptions = copyBuildOptions;
	}

	private static void LoadScriptingDefines()
	{
		scriptingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);
		originalScriptingDefines = scriptingDefines;
	}

	private static void LoadStyles()
	{
		mainHeader = EditorWindowUtility.CreateHeaderStyle();
		mainHeader.normal.background = EditorWindowUtility.CreateHeaderBackground(new Color(0.8f, 0.1f, 0.4f), 300, 20);

		helpHeader = new GUIStyle(mainHeader);
		helpHeader.normal.background = EditorWindowUtility.CreateHeaderBackground(new Color(0.2f, 0.7f, 0.4f), 300, 20);

		applyBox = new GUIStyle(EditorStyles.helpBox);
		applyBox.normal.background = EditorWindowUtility.CreateHeaderBackground(new Color(1f, 0f, 0f), 300, 20);
		applyBox.normal.textColor = Color.white;
		applyBox.fontSize += 3;
	}


	public static void DrawSection()
	{
		if (mainHeader == null || applyBox == null)
		{
			LoadStyles();
		}

		GUILayout.BeginHorizontal();
		float width = Screen.width * 0.8f;
		width = Mathf.Max(width, 300);

		GUILayout.Space(width * 0.12f);
		GUILayout.BeginVertical();

		DrawDefineSymbols(width, out bool applySymbolSettings);
		GUILayout.Space(15);
		DrawHelp(width);

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();


		// Note DK: moving the application of data outside of the begin/end layouts so the editor doesn't throw ArgumentExceptions about getting control 0's position. 
		if (applySymbolSettings)
		{
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefines);
			originalScriptingDefines = scriptingDefines;
		}
	}

	private static void DrawDefineSymbols(float width, out bool shouldApplySettings)
	{
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));
		GUILayout.Label("Scripting Define Symbols", mainHeader);
		GUILayout.Space(5);

		if (!originalScriptingDefines.Equals(scriptingDefines))
		{
			GUILayout.Box("[!!!] You've changed the Scripting Define Symbols, Press Apply Settings to make these changes persistent. [!!!]", applyBox);
		}

		bool applyButtonPressed = false;
		EditorWindowUtility.DrawButton("Apply Settings", width, () =>
		{
			applyButtonPressed = true;
		});

		shouldApplySettings = applyButtonPressed;

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
				if (scriptingDefines.Length > 0 && scriptingDefines[scriptingDefines.Length - 1] != ';')
				{
					prefix = ";";
				}

				scriptingDefines += $"{prefix}{symbol}";
			}
		}
	}
}
#endif