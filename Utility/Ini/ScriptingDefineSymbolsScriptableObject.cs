using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = "_scriptables/" + NAME, order = 0)]
public class ScriptingDefineSymbolsScriptableObject : ScriptableObject
{
	public const string NAME = "ScriptingDefinesScriptableObject";

	private const string DEBUG_MENU = "DEBUG_MENU";

	private static ScriptingDefineSymbolsScriptableObject _instance;
	public static ScriptingDefineSymbolsScriptableObject Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = ScriptingDefineSymbolsScriptableObject.LoadScriptableObject();
				Debug.Assert(_instance != null, "GeneralScriptableData instance is null");
			}

			return _instance;
		}
	}

	private static ScriptingDefineSymbolsScriptableObject LoadScriptableObject()
	{
		var scriptableObject = Resources.Load(NAME) as ScriptingDefineSymbolsScriptableObject;
		Debug.AssertFormat(scriptableObject != null, $"ScriptableObject {0} is null, You have to create an instance of {typeof(ScriptingDefineSymbolsScriptableObject)} in your project", NAME);
		return scriptableObject;
	}


	[SerializeField] private ScriptingSymbol[] symbols;

	private void Awake()
	{
		if (symbols.IsNullOrEmpty())
		{
			symbols = new ScriptingSymbol[1];
			symbols[0] = new ScriptingSymbol
			{
				symbol = DEBUG_MENU,
				label = "Is Debug Menu Enabled",
			};
		}
	}

	public ScriptingSymbol[] GetSymbols()
	{
		var result = new ScriptingSymbol[symbols.Length];
		symbols.CopyTo(result, 0);
		return result;
	}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ScriptingDefineSymbolsScriptableObject))]
public class ScriptingDefineSymbolsScriptableObjectEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		ScriptableUtility.DrawSaveButton_AtStart(this);

		base.OnInspectorGUI();
	}
}
#endif