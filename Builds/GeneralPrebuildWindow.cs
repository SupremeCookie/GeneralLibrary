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

	public static BuildPlayerOptions buildOptions { get { return IniControl.GeneralScriptableData.BuildOptions; } set { IniControl.GeneralScriptableData.BuildOptions = value; } }

	private void OnGUI()
	{
		{
			var scriptingDefinesForTarget = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildOptions.targetGroup);

			bool debugMenuIsEnabled = scriptingDefinesForTarget.Contains(DEBUG_MENU);
			var newDebugMenuIsEnabled = GUILayout.Toggle(debugMenuIsEnabled, "Is Debug Menu Enabled");

			if (debugMenuIsEnabled != newDebugMenuIsEnabled)
			{
				if (!newDebugMenuIsEnabled)
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

			bool combatGroupsDisabled = scriptingDefinesForTarget.Contains(DISABLE_COMBAT_GROUPS);
			var newCombatGroupsDisabled = GUILayout.Toggle(combatGroupsDisabled, "Are Combat Groups Disabled");

			if (combatGroupsDisabled != newCombatGroupsDisabled)
			{
				if (!newCombatGroupsDisabled)
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



		if (GUILayout.Button("Build Player"))
		{
			BuildPipeline.BuildPlayer(buildOptions);
		}
	}
}
#endif