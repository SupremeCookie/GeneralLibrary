using UnityEngine;

[CreateAssetMenu(fileName = NAME, menuName = "_scriptables/" + NAME, order = 0)]
public class ScriptingDefineSymbolsScriptableObject : ScriptableObject
{
	public const string NAME = "ScriptingDefinesScriptableObject";

	private const string DEBUG_MENU = "DEBUG_MENU";
	private const string DEMO_BUILD = "DEMO_BUILD";

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
		Debug.AssertFormat(scriptableObject != null, "ScriptableObject {0} is null, You have to create an instance of {1} in your project", NAME, typeof(ScriptingDefineSymbolsScriptableObject));
		return scriptableObject;
	}


	[SerializeField] private ScriptingSymbol[] symbols;

	private void Awake()
	{
		InitSymbols();
	}

	private void Reset()
	{
		InitSymbols();
	}

	private void InitSymbols()
	{
		if (symbols.IsNullOrEmpty())
		{
			symbols = new ScriptingSymbol[2];

			symbols[0] = new ScriptingSymbol
			{
				symbol = DEBUG_MENU,
				label = "Is Debug Menu Enabled",
			};

			symbols[1] = new ScriptingSymbol
			{
				symbol = DEMO_BUILD,
				label = "Is this a Demo Build",
			};
		}

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif

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