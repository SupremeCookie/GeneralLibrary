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

		GUILayout.BeginHorizontal();
		float width = Screen.width * 0.6f;
		width = width < 300 ? 300 : width;

		GUILayout.Space(width * 0.33f);
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(width));

		DrawChecklist();

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		GUILayout.Space(25);

		if (GUILayout.Button("Build Player"))
		{
			BuildPipeline.BuildPlayer(buildOptions);
			Close();
		}
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

		const int width = 300;
		const int height = 20;
		Texture2D newBackground = new Texture2D(width, height);

		Color pixelColor = new Color(0.2f, 0.6f, 1f);
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

		headerInternal.normal.background = newBackground;
		headerInternal.alignment = TextAnchor.MiddleCenter;

		headerInternal.fontSize += 5;
	}
}
#endif