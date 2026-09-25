using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = SO_NAME, menuName = "_scriptables/Loca/" + SO_NAME, order = 0)]
public class MainLocaDB : CustomSO
{
	protected static MainLocaDB pInstance;

	public const string SO_NAME = "Main Loca DB";

	public static MainLocaDB Instance
	{
		get
		{
			if (pInstance == null)
			{
				SetPInstance();
			}

			return pInstance as MainLocaDB;
		}
	}

	protected static void SetPInstance()
	{
		Debug.Log($"[MainLocaDB] SetPInstance");
		pInstance = CustomSO.LoadScriptableObject<MainLocaDB>("ScriptableObjects/Loca/" + SO_NAME);
		Debug.Assert(pInstance != null, "[MainLocaDB] instance is null");
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Data/" + SO_NAME)]
	public static void OpenScriptable()
	{
		OpenScriptableObject(Instance);
	}
#endif



	public UnityEngine.TextAsset csvObject;


	private List<string> debugTexts = new List<string>();


	private void ProcessCSVObject()
	{
		var text = csvObject.text;
		var lines = text.Split("\n");
		if (lines.IsNullOrEmpty())
		{
			debugTexts.Add($"Splitting the csvObject's text didn't result in anything");
			return;
		}

		for (int i = 0; i < lines.Length; ++i)
		{
			// Read till we find our In Game key, then from that point on start storing shit per line
			// Make the header bar, read out indices from that
			// Then start making a class that has the "Line Key", and then for each dynamic key, store the value.
		}


		debugTexts.Add($"Got [{lines.Length}] lines of text");
	}



#if UNITY_EDITOR
	public void DrawDebugControls()
	{
		string content = "";
		if (debugTexts.HasContent())
		{
			content = debugTexts.Aggregate((s, x) => $"{s}, {x}\n");
		}

		EditorGUILayout.TextArea(content);
		GUILayout.Space(15);

		if (GUILayout.Button("Process Main Loca"))
		{
			debugTexts = new List<string>();
			ProcessCSVObject();
		}


		GUILayout.Space(50);
		GUILayout.TextArea(csvObject?.text);
	}
#endif
}



#if UNITY_EDITOR
[CustomEditor(typeof(MainLocaDB))]
public class MainLocaDBEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		(target as MainLocaDB).DrawDebugControls();
	}
}
#endif