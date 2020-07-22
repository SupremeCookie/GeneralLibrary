#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class MenuItemEditorUtils
{
	[MenuItem("Data/Save All ScriptableObjects")]
	public static void SaveAllScriptableObjects()
	{
		var scriptables = EditorUtils.GetAllScriptableObjects();

		if (scriptables == null || scriptables.Length == 0)
		{
			Debug.Log("No Scriptable Objects in the project. Or none found");
			return;
		}

		for (int i = 0; i < scriptables.Length; ++i)
		{
			EditorUtility.SetDirty(scriptables[i]);
		}

		AssetDatabase.SaveAssets();
		Debug.LogFormat("<b>Save All ScriptableObjects, count: ({0})</b>", scriptables.Length);
	}
}
#endif