using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class LocaManager : SingletonMonoBehaviour<LocaManager>
{
	// Add a reference to each Language DB
	// Add a reference to game options, to get current language
	// Have this be the throughput for language and everything

	[Header("Debug")]
	[SerializeField] private LanguageID debugLanguage = LanguageID.English;

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
		return StaticCleanString(fallback);
	}


	public static string StaticCleanString(string value)
	{
		value = value.Replace("\\n", System.Environment.NewLine);
		return value;
	}


	private void Log(string message)
	{
		UnityEngine.Debug.Log($"[Loca Manager] {message}");
	}
}