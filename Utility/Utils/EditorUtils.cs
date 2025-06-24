using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorUtils
{
#if UNITY_EDITOR
	private static GUIStyle _intFieldStyle;
	private static GUIStyle intFieldStyle
	{
		get
		{
			if (_intFieldStyle == null)
			{
				_intFieldStyle = new GUIStyle(EditorStyles.numberField);
				intFieldStyle.CalcMinMaxWidth(new GUIContent("100"), out float minWidth, out float maxWidth);
				intFieldStyle.fixedWidth = minWidth;
			}

			return _intFieldStyle;
		}
	}

	private static GUIStyle _labelFieldStyle;
	private static GUIStyle labelFieldStyle
	{
		get
		{
			if (_labelFieldStyle == null)
			{
				_labelFieldStyle = new GUIStyle(EditorStyles.boldLabel);
				labelFieldStyle.CalcMinMaxWidth(new GUIContent("Destination Node Target"), out float minWidth, out float maxWidth);
				labelFieldStyle.fixedWidth = minWidth;
			}

			return _labelFieldStyle;
		}
	}


	// min -- incl
	// max -- excl
	public static bool DrawPlusMinusControl(ref int value, int min, int max, string label, Object targetObj)
	{
		int oldValue = value;
		int intFieldValue = value;

		float width = Mathf.Clamp(Screen.width * 0.3f, 50, 100);
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(width));

		GUILayout.Label(label, labelFieldStyle);
		GUILayout.Space(15);

		value = Mathf.Clamp(value, min, max);

		GUI.SetNextControlName("minusButton");
		if (GUILayout.Button("---"))
		{
			value--;
			if (value < min)
				value = max - 1;

			GUI.FocusControl("minusButton");    // Note DK: Moves the focus away from the intField, in case it is selected
			intFieldValue = value;  // Note DK: The 2 values change independently of each other, gotta keep them synced.

			SceneView.RepaintAll();
			EditorUtility.SetDirty(targetObj);
		}

		intFieldValue = EditorGUILayout.IntField(value, intFieldStyle, GUILayout.MaxWidth(intFieldStyle.fixedWidth));   // Note DK: MaxWidth needs to be explicitly set

		GUI.SetNextControlName("plusButton");
		if (GUILayout.Button("+++"))
		{
			value++;
			if (value >= max)
				value = min;

			GUI.FocusControl("plusButton"); // Note DK: Moves the focus away from the intField, in case it is selected
			intFieldValue = value;  // Note DK: The 2 values change independently of each other, gotta keep them synced.

			SceneView.RepaintAll();
			EditorUtility.SetDirty(targetObj);
		}


		GUILayout.EndHorizontal();

		if (value != intFieldValue)
			value = intFieldValue;

		return oldValue != value;
	}





	public static T[] GetAllScriptableObjectInstances<T>() where T : ScriptableObject
	{
		return GetAssetInstances<T>();
	}

	public static ScriptableObject[] GetAllScriptableObjects()
	{
		return GetAssetInstances<ScriptableObject>();
	}

	public static SceneAsset[] GetAllSceneInstances()
	{
		return GetAssetInstances<SceneAsset>();
	}

	public static GameObject[] GetObjectsWithComponent<T>()
	{
		GameObject[] gameObjects = GetAssetInstances<GameObject>();
		return gameObjects.Where(s => s.GetComponent<T>() != null).ToArray();
	}

	public static GameObject[] GetObjectsWithComponent<T>(string folderPath)
	{
		GameObject[] gameObjects = GetAssetInstances<GameObject>(new string[] { folderPath });
		return gameObjects.Where(s => s.GetComponent<T>() != null).ToArray();
	}

	public static T[] GetAllInstancesOfType<T>() where T : UnityEngine.Object
	{
		return GetAssetInstances<T>();
	}

	public static T[] GetAllAssetsInFolder<T>(string folderPath) where T : UnityEngine.Object
	{
		Debug.Log("Loading Assets at folder: " + folderPath);

		T[] assets = GetAssetInstances<T>(new string[] { folderPath });
		Debug.Assert(assets != null, "No assets found at path: " + folderPath);

		return assets;
	}

	public static Object[] GetAllAssetsInFolder(string[] searchInFolders)
	{
		string[] guids = AssetDatabase.FindAssets("", searchInFolders);  //FindAssets uses tags.   https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html

		bool hasAssets = guids != null && guids.Length > 0;
		Debug.Assert(hasAssets, "No assets found at path");

		if (!hasAssets)
			return null;

		Object[] a = new Object[guids.Length];
		for (int i = 0; i < guids.Length; ++i)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			a[i] = AssetDatabase.LoadAssetAtPath<Object>(path);
		}

		return a;
	}

	public static string[] GetPathsOfAssets(Object[] assets)
	{
		Debug.Assert(assets != null && assets.Length > 0, "No Assets");
		var result = new string[assets.Length];

		for (int i = 0; i < assets.Length; ++i)
		{
			result[i] = (AssetDatabase.GetAssetPath(assets[i]));
		}

		return result;
	}

	private static T[] GetAssetInstances<T>() where T : UnityEngine.Object
	{
		return GetAssets<T>("t:" + typeof(T).Name);
	}

	private static T[] GetAssetInstances<T>(string[] searchInFolders) where T : UnityEngine.Object
	{
		return GetAssets<T>("t:" + typeof(T).Name, searchInFolders);
	}

	private static T[] GetAssets<T>(string filter, string[] searchInFolders = null) where T : UnityEngine.Object
	{
		string[] searchInFoldersParam = searchInFolders != null ? searchInFolders : null;

		string[] guids = AssetDatabase.FindAssets(filter, searchInFoldersParam);  //FindAssets uses tags.   https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html

		bool hasAssets = guids != null && guids.Length > 0;
		Debug.Assert(hasAssets, "No assets found at path");

		if (!hasAssets)
			return null;

		T[] a = new T[guids.Length];
		for (int i = 0; i < guids.Length; ++i)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
		}

		return a;
	}
#endif
}
