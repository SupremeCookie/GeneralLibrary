using UnityEngine;

public partial class ScriptableUtility
{
#if UNITY_EDITOR
	public static void DrawSaveButton(UnityEngine.Object target)
	{
		if (GUILayout.Button("Save Asset"))
		{
			UnityEditor.EditorUtility.SetDirty(target);
			UnityEditor.AssetDatabase.SaveAssets();
			UnityEditor.SceneView.RepaintAll();
		}
	}

	public static void DrawSaveButton_AtStart(UnityEngine.Object target)
	{
		DrawSaveButton(target);

		GUILayout.Space(15);
	}

	public static void DrawSaveButton_AtMiddle(UnityEngine.Object target)
	{
		GUILayout.Space(15);

		DrawSaveButton(target);

		GUILayout.Space(15);
	}

	public static void DrawSaveButton_AtEnd(UnityEngine.Object target)
	{
		GUILayout.Space(15);

		DrawSaveButton(target);
	}
#endif
}