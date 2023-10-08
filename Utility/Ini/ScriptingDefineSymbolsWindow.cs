using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class ScriptingDefineSymbolsWindow : EditorWindow
{
	private bool hasInitialised = false;

	[MenuItem("Tools/ScriptingDefineSymbols")]
	private static void OpenWindow()
	{
		ScriptingDefineSymbolsWindow window = (ScriptingDefineSymbolsWindow)EditorWindow.GetWindow(typeof(ScriptingDefineSymbolsWindow));
		window.Show();
		window.minSize = new UnityEngine.Vector2(300, 100);
	}

	private void OnGUI()
	{
		if (!hasInitialised)
		{
			ScriptingDefineSymbolsSection.Load();
			hasInitialised = true;
		}

		ScriptingDefineSymbolsSection.DrawSection();
	}
}
