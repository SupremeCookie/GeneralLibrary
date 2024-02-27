#if UNITY_EDITOR
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
	public static BuildPlayerOptions buildOptions { get { return IniControl.GeneralScriptableData.BuildOptions; } set { IniControl.GeneralScriptableData.BuildOptions = value; } }

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

		bool emptyBool = false;
		GUILayout.Toggle(emptyBool, $"Is ShouldResetFavors targeting the right version?");
	}



	private GUIStyle header { get { if (headerInternal == null) { LoadStyles(); } return headerInternal; } }
	private GUIStyle headerInternal;

	private void LoadStyles()
	{
		headerInternal = new GUIStyle(GUI.skin.label);

		headerInternal.normal.background = EditorWindowUtility.CreateHeaderBackground(new Color(0.2f, 0.6f, 1f), 300, 20);
		headerInternal.alignment = TextAnchor.MiddleCenter;

		headerInternal.fontSize += 5;
	}
}
#endif