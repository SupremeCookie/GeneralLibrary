﻿#if UNITY_EDITOR
using UnityEditor;

public class ScriptingDefineSymbolsWindow : EditorWindow
{
	[MenuItem("DKat/ScriptingDefineSymbols", priority = 10)]
	private static void OpenWindow()
	{
		ScriptingDefineSymbolsWindow window = (ScriptingDefineSymbolsWindow)EditorWindow.GetWindow(typeof(ScriptingDefineSymbolsWindow));

		ScriptingDefineSymbolsSection.ResetHasLoaded();

		window.Show();
		window.minSize = new UnityEngine.Vector2(300, 100);
	}

	private void OnGUI()
	{
		if (!ScriptingDefineSymbolsSection.HasLoaded)
		{
			ScriptingDefineSymbolsSection.Load();
			return;
		}

		ScriptingDefineSymbolsSection.DrawSection();
	}
}
#endif