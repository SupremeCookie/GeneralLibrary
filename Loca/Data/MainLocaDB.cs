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
	[System.Serializable]
	public class KeyMapModel
	{
		public LanguageID language;
		public string languageKey;
	}

	[System.Serializable]
	public class KeyLanguageString
	{
		public string key;
		public List<LanguageValue> values;
	}

	[System.Serializable]
	public class LanguageValue
	{
		public LanguageID language;
		public string value;
	}

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



	[Header("References")]
	public UnityEngine.TextAsset csvObject;

	[Header("Settings")]
	public string startOfGameLocaKey = "In Game";
	public string splitCharacter = "||";

	[Header("Language Key Mapping")]
	public List<KeyMapModel> regularMaps = new List<KeyMapModel>();
	public List<KeyMapModel> correctionMaps = new List<KeyMapModel>();

	[Header("Results")]
	public List<KeyLanguageString> allKeysAndValues = new List<KeyLanguageString>();

	[Header("DBs")]
	public List<LocaDB> dbs;


	private List<string> debugTexts = new List<string>();
	private Dictionary<LanguageID, int> regularKeyColumnIndices;
	private Dictionary<LanguageID, int> correctionKeyColumnIndices;


	private void ProcessCSVObject()
	{
		var text = csvObject.text;
		var lines = text.Split("\n");
		if (lines.IsNullOrEmpty())
		{
			debugTexts.Add($"Splitting the csvObject's text didn't result in anything");
			return;
		}

		debugTexts.Add($"Got [{lines.Length}] lines of text");

		int locaStartsAtIndex = -1;
		for (int i = 0; i < lines.Length; ++i)
		{
			// Read till we find our In Game key, then from that point on start storing shit per line
			// Make the header bar, read out indices from that
			// Then start making a class that has the "Line Key", and then for each dynamic key, store the value.
			bool hasKeyWeNeed = lines[i].IndexOf(startOfGameLocaKey) >= 0;
			if (hasKeyWeNeed)
			{
				locaStartsAtIndex = i;
				break;
			}
		}

		if (locaStartsAtIndex < 0)
		{
			debugTexts.Add($"Cannae do loca, we haven't found the right index. Looking for key: [{startOfGameLocaKey}]");
			return;
		}


		var headersLine = lines[locaStartsAtIndex];
		int trueLocaValueStartIndex = locaStartsAtIndex + 1;
		debugTexts.Add($"HeadersLine:  {headersLine}");
		debugTexts.Add($"Starting our loca keys from index: {trueLocaValueStartIndex}");

		ProcessHeadersLine(headersLine);

		allKeysAndValues = new List<KeyLanguageString>();
		for (int i = trueLocaValueStartIndex; i < lines.Length; ++i)
		{
			ProcessRegularLine(lines[i]);
		}

		debugTexts.Add($"Got {allKeysAndValues?.Count ?? -1} keyed values");

		EditorUtility.SetDirty(this);
	}


	private void ProcessHeadersLine(string line)
	{
		regularKeyColumnIndices = new Dictionary<LanguageID, int>();
		correctionKeyColumnIndices = new Dictionary<LanguageID, int>();

		var splitLine = line.Split(splitCharacter);
		debugTexts.Add($"Split the headerslines: {splitLine?.Length ?? -1}");

		for (int i = 0; i < splitLine.Length; ++i)
		{
			var value = splitLine[i];
			if (string.IsNullOrEmpty(value))
				continue;

			int indexOfRegularLanguages = -1;
			int indexOfCorrectionLanguages = -1;

			for (int k = 0; indexOfRegularLanguages < 0 && k < regularMaps.Count; ++k)
			{
				if (regularMaps[k].languageKey.Equals(value))
					indexOfRegularLanguages = k;
			}

			if (indexOfRegularLanguages < 0)
			{
				for (int k = 0; indexOfCorrectionLanguages < 0 && k < correctionMaps.Count; ++k)
				{
					if (correctionMaps[k].languageKey.Equals(value))
						indexOfCorrectionLanguages = k;
				}
			}


			if (indexOfRegularLanguages >= 0)
				regularKeyColumnIndices.Add(regularMaps[indexOfRegularLanguages].language, i);

			if (indexOfCorrectionLanguages >= 0)
				correctionKeyColumnIndices.Add(correctionMaps[indexOfCorrectionLanguages].language, i);
		}

		// Display in debug text
		if (regularKeyColumnIndices.Count > 0)
		{
			string aggregatedRegular = regularKeyColumnIndices.Select(s => $"[{s.Key}, {s.Value}]").Aggregate((s, x) => $"{s}, {x}");
			debugTexts.Add($"Regular Language Columns:  {aggregatedRegular}");
		}

		if (correctionKeyColumnIndices.Count > 0)
		{
			string aggregatedCorrection = correctionKeyColumnIndices.Select(s => $"[{s.Key}, {s.Value}]").Aggregate((s, x) => $"{s}, {x}");
			debugTexts.Add($"Correction Language Columns:  {aggregatedCorrection}");
		}
	}

	private void ProcessRegularLine(string lineText)
	{
		var splitLine = lineText.Split(splitCharacter);
		if (splitLine.IsNullOrEmpty())
			return;

		var entry = new KeyLanguageString();
		entry.key = splitLine[0];
		if (string.IsNullOrEmpty(entry.key))
			entry.key = splitLine[1];   // Add as is needed

		entry.values = new List<LanguageValue>((int)LanguageID.Count);

		foreach (var kvp in regularKeyColumnIndices)
		{
			var key = kvp.Key;
			var index = kvp.Value;

			var baseString = CleanString(splitLine[index]);
			entry.values.Add(new LanguageValue
			{
				language = key,
				value = baseString,
			});
		}

		foreach (var kvp in correctionKeyColumnIndices)
		{
			var key = kvp.Key;
			var index = kvp.Value;

			var correctionString = CleanString(splitLine[index]);
			if (!string.IsNullOrEmpty(correctionString))
			{
				for (int i = 0; i < entry.values.Count; ++i)
				{
					if (entry.values[i].language == key)
						entry.values[i].value = correctionString;
				}
			}
		}

		allKeysAndValues.Add(entry);
	}

	private string CleanString(string input)
	{
		return input.Replace("\\\\n", "\\n");
	}



	private void FillDBs()
	{
		if (allKeysAndValues.IsNullOrEmpty())
		{
			debugTexts.Add("Can't fill dbs, as there is no content available");
			return;
		}

		if (dbs.IsNullOrEmpty())
		{
			debugTexts.Add("No DBs to fill up");
			return;
		}


		for (int i = 0; i < dbs.Count; ++i)
		{
			dbs[i].locTerms = new List<LocTermModel>();
		}


		// Store all loc term keys
		for (int i = 0; i < allKeysAndValues.Count; ++i)
		{
			var entry = allKeysAndValues[i];
			var values = entry.values;

			for (int z = 0; z < values.Count; ++z)
			{
				var v = values[z];
				for (int k = 0; k < dbs.Count; ++k)
				{
					if (v.language == dbs[k].language)
						dbs[k].locTerms.Add(new LocTermModel(entry.key, v.value));
				}
			}
		}

		for (int i = 0; i < dbs.Count; ++i)
		{
			debugTexts.Add($"Added {dbs[i].locTerms?.Count ?? -1} keys for language: {dbs[i].language}");
		}


		EditorUtility.SetDirty(this);
		for (int i = 0; i < dbs.Count; ++i)
			EditorUtility.SetDirty(dbs[i]);
	}



#if UNITY_EDITOR
	private bool showTextAreaForObjectTextCSV = false;

	public void DrawDebugControls()
	{
		string content = "";
		if (debugTexts.HasContent())
		{
			content = debugTexts.Aggregate((s, x) => $"{s}\n{x}");
		}

		EditorStyles.textField.wordWrap = true;
		EditorGUILayout.TextArea(content);
		GUILayout.Space(15);

		if (GUILayout.Button("Process Main Loca"))
		{
			debugTexts = new List<string>();
			ProcessCSVObject();
		}

		GUILayout.Space(5);

		if (GUILayout.Button("Fill DBs"))
		{
			debugTexts = new List<string>();
			FillDBs();
		}


		GUILayout.Space(50);

		showTextAreaForObjectTextCSV = GUILayout.Toggle(showTextAreaForObjectTextCSV, "Show source?");
		if (showTextAreaForObjectTextCSV)
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