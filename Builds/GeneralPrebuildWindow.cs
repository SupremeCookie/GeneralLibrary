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
	public static BuildPlayerOptions buildOptions { get { return IniControl.GeneralScriptableData.BuildOptions; } set { IniControl.GeneralScriptableData.BuildOptions = value; } }

	private void OnEnable()
	{
		ScriptingDefineSymbolsSection.Load();
	}

	private void OnGUI()
	{
		ScriptingDefineSymbolsSection.DrawSection();

		GUILayout.Space(25);

		if (GUILayout.Button("Build Player"))
		{
			BuildPipeline.BuildPlayer(buildOptions);
			Close();
		}
	}
}
#endif