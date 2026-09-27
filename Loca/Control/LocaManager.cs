using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class LocaManager : SingletonMonoBehaviour<LocaManager>
{
	[Header("Debug")]
	[SerializeField] private LanguageID debugLanguage = LanguageID.English;
	[SerializeField, Readonly] private LanguageID currentLanguage = LanguageID.English;

	[Header("References")]
	public List<LocaDB> locaDBs;

	private Dictionary<LanguageID, LocaDB> quickLookupDBs = new Dictionary<LanguageID, LocaDB>();


	public void Start()
	{
		for (int i = 0; i < locaDBs.Count; ++i)
		{
			locaDBs[i].InitialiseCache();
			quickLookupDBs.Add(locaDBs[i].language, locaDBs[i]);

			Log($"Initialised loca db for [{locaDBs[i].language}],  added a Quick Lookup db: {quickLookupDBs.Count}");
		}
	}


	public string GetValueCurrentLanguage(string key, string fallback)
	{
		if (!quickLookupDBs.ContainsKey(currentLanguage))
		{
			return StaticCleanString(fallback);
		}

		var db = quickLookupDBs[currentLanguage];
		return StaticCleanString(db.GetValue(key, fallback));
	}

	public string GetValue(string key, string fallback, LanguageID language)
	{
		if (quickLookupDBs.ContainsKey(language))
		{
			return StaticCleanString(quickLookupDBs[language].GetValue(key, fallback));
		}

		return fallback;
	}


	public void UpdateLanguage(LanguageID language)
	{
		Log($"Updating to language: {language}");

		currentLanguage = language;
	}


	public static string StaticCleanString(string value)
	{
		value = value.Replace("\\n", System.Environment.NewLine);
		return value;
	}


#if UNITY_EDITOR
	public void DrawControls()
	{
		if (GUILayout.Button("Update To Debug Language"))
		{
			UpdateLanguage(debugLanguage);
		}
	}
#endif

	private void Log(string message)
	{
		UnityEngine.Debug.Log($"[Loca Manager] {message}");
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LocaManager))]
public class LocaManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		(target as LocaManager).DrawControls();
	}
}
#endif