using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum LanguageID
{
	English,
	Dutch,
	French,
	German,
	Spanish,
	Portugese,
	Italian,
	Russian,
	Korean,
	Japanese,
	Chinese_Simplified,
	Chinese_Traditional,


	Count,   // Keep last

	None    // Only true false value
}


public static class LanguageIDExtensionMethods
{
	public static string ToSteamLanguageAPIShortCode(this LanguageID languageID)        // These are steam codes
	{
		switch (languageID)
		{
			case LanguageID.Dutch: { return "dutch"; }
			case LanguageID.French: { return "french"; }
			case LanguageID.German: { return "german"; }
			case LanguageID.Spanish: { return "spanish"; }
			case LanguageID.Portugese: { return "brazilian"; }
			case LanguageID.Italian: { return "italian"; }
			case LanguageID.Russian: { return "russian"; }
			case LanguageID.Korean: { return "koreana"; }
			case LanguageID.Japanese: { return "japanese"; }
			case LanguageID.Chinese_Simplified: { return "schinese"; }
			case LanguageID.Chinese_Traditional: { return "tchinese"; }
			case LanguageID.None: { return ""; }


			default:
			case LanguageID.English:
				return "english";
		}
	}
}

public static class LanguageIDUtility
{
	public static LanguageID Parse(string input)
	{
		switch (input)
		{
			case "dutch": { return LanguageID.Dutch; }
			case "french": { return LanguageID.French; }
			case "german": { return LanguageID.German; }
			case "spanish": { return LanguageID.Spanish; }
			case "brazilian": { return LanguageID.Portugese; }
			case "italian": { return LanguageID.Italian; }
			case "russian": { return LanguageID.Russian; }
			case "koreana": { return LanguageID.Korean; }
			case "japanese": { return LanguageID.Japanese; }
			case "schinese": { return LanguageID.Chinese_Simplified; }
			case "tchinese": { return LanguageID.Chinese_Traditional; }
			case "": { return LanguageID.None; }


			default:
			case "english":
				return LanguageID.English;
		}
	}
}