using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = GENERAL_SCRIPTABLE_NAME, menuName = "_scriptables/" + GENERAL_SCRIPTABLE_NAME, order = 0)]
public class GeneralScriptableData : CustomSO
{
	protected static GeneralScriptableData pInstance;

	public const string GENERAL_SCRIPTABLE_NAME = "GeneralData";

	public static GeneralScriptableData Instance
	{
		get
		{
			if (pInstance == null)
			{
				pInstance = CustomSO.LoadScriptableObject<GeneralScriptableData>(GENERAL_SCRIPTABLE_NAME);
				Debug.Assert(pInstance != null, "GeneralScriptableData instance is null");
			}

			return pInstance as GeneralScriptableData;
		}
	}

#if UNITY_EDITOR
	public SerializableBuildPlayerOptions BuildOptions;
#endif

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Data/" + GENERAL_SCRIPTABLE_NAME + " %#g")]
	public static void OpenScriptable()
	{
		OpenScriptableObject(pInstance);
	}
#endif
}

