using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = SO_NAME, menuName = "_scriptables/Loca/" + SO_NAME, order = 0)]
public class LocaDB : CustomSO
{
	[System.Serializable]
	public class KeyedString
	{
		public string key;
		public string value;

		public KeyedString() { }
		public KeyedString(string key, string value)
		{
			this.key = key;
			this.value = value;
		}
	}

	public const string SO_NAME = "Loca-DB";


	public LanguageID language;
	public List<KeyedString> locTerms = new List<KeyedString>();

	private Dictionary<string, KeyedString> cache = new Dictionary<string, KeyedString>();


	public void InitialiseCache()
	{
		cache = new Dictionary<string, KeyedString>();
		if (locTerms.Count == 0)
			return;

		for (int i = 0; i < locTerms.Count; ++i)
		{
			cache.Add(locTerms[i].key, new KeyedString(locTerms[i].key, locTerms[i].value));
		}
	}

	public string GetValue(string key, string fallback)
	{
		if (cache.ContainsKey(key))
			return cache[key].value;

		Debug.Log($"[Loca DB]  No value made for key: {key}", this);
		return fallback;
	}
}