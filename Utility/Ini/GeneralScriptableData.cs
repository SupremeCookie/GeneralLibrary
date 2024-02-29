using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = GENERAL_SCRIPTABLE_NAME, menuName = "_scriptables/" + GENERAL_SCRIPTABLE_NAME, order = 0)]
public class GeneralScriptableData : ScriptableObject
{
	public const string GENERAL_SCRIPTABLE_NAME = "GeneralData";

	private static GeneralScriptableData _instance;
	public static GeneralScriptableData Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GeneralScriptableData.LoadScriptableObject();
				Debug.Assert(_instance != null, "GeneralScriptableData instance is null");
			}

			return _instance;
		}
	}

#if UNITY_EDITOR
	public SerializableBuildPlayerOptions BuildOptions;
#endif

	private static GeneralScriptableData LoadScriptableObject()
	{
		var scriptableObject = Resources.Load(GENERAL_SCRIPTABLE_NAME) as GeneralScriptableData;
		Debug.AssertFormat(scriptableObject != null, "ScriptableObject {0} is null", GENERAL_SCRIPTABLE_NAME);
		return scriptableObject;
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Data/" + GENERAL_SCRIPTABLE_NAME + " %#g")]
#endif
	public static void OpenScriptable()
	{
#if UNITY_EDITOR
		var scriptableObject = Instance;
		UnityEditor.Selection.activeObject = scriptableObject;
#endif
	}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(GeneralScriptableData))]
public class GeneralScriptableDataEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		var castedTarget = (GeneralScriptableData)target;

		ScriptableUtility.DrawSaveButton_AtStart(target);

		base.OnInspectorGUI();

		UnityEditor.EditorGUILayout.Space();
		UnityEditor.EditorGUILayout.Space();
	}
}
#endif

