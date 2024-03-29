﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
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
	public static BuildPlayerOptions buildOptions { get { return GlobalConfig.GeneralScriptableData.BuildOptions; } set { GlobalConfig.GeneralScriptableData.BuildOptions = value; } }

	private GUIStyle header { get { if (headerInternal == null) { LoadStyles(); } return headerInternal; } }
	private GUIStyle headerInternal;

	private void LoadStyles()
	{
		headerInternal = EditorWindowUtility.CreateHeaderStyle();
	}

	private void OnGUI()
	{
		if (!ScriptingDefineSymbolsSection.HasLoaded)
		{
			ScriptingDefineSymbolsSection.Load();
			return;
		}

		ScriptingDefineSymbolsSection.DrawSection();

		GUILayout.Space(25);

		GUILayout.BeginHorizontal();
		float width = Screen.width * 0.8f;
		width = width < 300 ? 300 : width;

		GUILayout.Space(width * 0.12f);
		GUILayout.BeginVertical();

		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));
		DrawChecklist();
		GUILayout.EndVertical();

		GUILayout.Space(50);

		string label = "Make a Build";
#if DEMO_BUILD
		label = "Make a (DEMO) Build";
#endif

		bool canBuild = !EditorApplication.isCompiling;

		EditorGUI.BeginDisabledGroup(!canBuild);
		if (!canBuild)
		{
			GUILayout.Label($"Cannot build whilst Unity is compiling");
		}

		EditorWindowUtility.DrawButton(label, width, () =>
		{
			BuildPipeline.BuildPlayer(buildOptions);
			Close();
		});

		EditorGUI.EndDisabledGroup();

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private void DrawChecklist()
	{
		GUILayout.Label("General Checklist", header);

		GUILayout.Label($"Current Game Version: {Application.version}");

		// Maybe load some kind of checklist structure from general scriptable?
	}
}
#endif