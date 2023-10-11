using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class ScriptingDefineSymbolsWindow : EditorWindow
{
	[MenuItem("Tools/ScriptingDefineSymbols")]
	private static void OpenWindow()
	{
		ScriptingDefineSymbolsWindow window = (ScriptingDefineSymbolsWindow)EditorWindow.GetWindow(typeof(ScriptingDefineSymbolsWindow));
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
