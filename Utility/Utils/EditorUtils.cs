using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

public class EditorUtils
{
#if UNITY_EDITOR
	public static System.Type GetType(string TypeName)
	{
		// Try Type.GetType() first. This will work with types defined by the Mono runtime, etc.
		var type = System.Type.GetType(TypeName);

		// If it worked, then we're done here
		if (type != null)
		{
			return type;
		}

		// Get the name of the assembly (Assumption is that we are using fully-qualified type names)
		var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

		// Attempt to load the indicated Assembly
		var assembly = Assembly.Load(assemblyName); // LoadWithPartialName(		<< TODO: Gotta test if this still works.
		Debug.Assert(assembly != null, $"Can't find ({assemblyName}) using Assembly.LoadWithPartialName");

		Debug.Log($"<b>AnimationEditorWindow</b> -- Found assembly: ({assembly.FullName})");

		// Ask that assembly to return the proper Type
		return assembly.GetType(TypeName);
	}

	public static T[] GetAllScriptableObjectInstances<T>() where T : ScriptableObject
	{
		return GetAssetInstances<T>();
	}

	public static ScriptableObject[] GetAllScriptableObjects()
	{
		return GetAssetInstances<ScriptableObject>();
	}

	public static AnimationClip[] GetAllAnimationClipInstances()
	{
		return GetAssetInstances<AnimationClip>();
	}

	public static Animator[] GetAllAnimatorInstances()
	{
		return GetAssetInstances<Animator>();
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
