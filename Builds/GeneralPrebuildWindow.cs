#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public class BuildWindowHandler
{
	[InitializeOnLoadMethod]
	public static void OnLoadingRuntime()
	{
		BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayer);
	}

	public static void BuildPlayer(BuildPlayerOptions options)
	{
		var prebuildWindow = (GeneralPreBuildWindow)EditorWindow.GetWindow(typeof(GeneralPreBuildWindow));
		GeneralPreBuildWindow.buildOptions = options;
		prebuildWindow.Show();
	}
}

public class GeneralPreBuildWindow : BuildPlayerWindow
{
	private const string DEBUG_MENU = "DEBUG_MENU";
	private const string DISABLE_COMBAT_GROUPS = "DISABLE_COMBAT_GROUPS";
	private const string FAVOR_DEBUG_UNLOCKER = "FAVOR_DEBUG_UNLOCKER";

	public static BuildPlayerOptions buildOptions { get { return IniControl.GeneralScriptableData.BuildOptions; } set { IniControl.GeneralScriptableData.BuildOptions = value; } }

	private void OnGUI()
	{
		{
			var scriptingDefinesForTarget = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);

			bool valueIsEnabled = scriptingDefinesForTarget.Contains(DEBUG_MENU);
			var newValueIsEnabled = GUILayout.Toggle(valueIsEnabled, "Is Debug Menu Enabled");

			if (valueIsEnabled != newValueIsEnabled)
			{
				if (!newValueIsEnabled)
				{
					var startIndexOfString = scriptingDefinesForTarget.IndexOf(DEBUG_MENU);
					scriptingDefinesForTarget = scriptingDefinesForTarget.Remove(startIndexOfString, DEBUG_MENU.Length);
				}
				else
				{
					scriptingDefinesForTarget += $";{DEBUG_MENU}";
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefinesForTarget);
			}
		}


		{
			var scriptingDefinesForTarget = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);

			bool valueIsEnabled = scriptingDefinesForTarget.Contains(DISABLE_COMBAT_GROUPS);
			var newValueIsEnabled = GUILayout.Toggle(valueIsEnabled, "Are Combat Groups Disabled");

			if (valueIsEnabled != newValueIsEnabled)
			{
				if (!newValueIsEnabled)
				{
					var startIndexOfString = scriptingDefinesForTarget.IndexOf(DISABLE_COMBAT_GROUPS);
					scriptingDefinesForTarget = scriptingDefinesForTarget.Remove(startIndexOfString, DISABLE_COMBAT_GROUPS.Length);
				}
				else
				{
					scriptingDefinesForTarget += $";{DISABLE_COMBAT_GROUPS}";
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefinesForTarget);
			}
		}


		{
			var scriptingDefinesForTarget = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);

			bool valueIsEnabled = scriptingDefinesForTarget.Contains(FAVOR_DEBUG_UNLOCKER);
			var newValueIsEnabled = GUILayout.Toggle(valueIsEnabled, "Is the Favor Debug Unlocker Enabled");

			if (valueIsEnabled != newValueIsEnabled)
			{
				if (!newValueIsEnabled)
				{
					var startIndexOfString = scriptingDefinesForTarget.IndexOf(FAVOR_DEBUG_UNLOCKER);
					scriptingDefinesForTarget = scriptingDefinesForTarget.Remove(startIndexOfString, FAVOR_DEBUG_UNLOCKER.Length);
				}
				else
				{
					scriptingDefinesForTarget += $";{FAVOR_DEBUG_UNLOCKER}";
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildOptions.targetGroup, scriptingDefinesForTarget);
			}
		}



		if (GUILayout.Button("Build Player"))
		{
			BuildPipeline.BuildPlayer(buildOptions);
			Close();
		}
	}
}
#endif