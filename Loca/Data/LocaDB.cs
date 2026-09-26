using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = SO_NAME, menuName = "_scriptables/Loca/" + SO_NAME, order = 0)]
public class LocaDB : CustomSO
{
	public const string SO_NAME = "Loca-DB";


	public LanguageID language;
	public List<LocTermModel> locTerms = new List<LocTermModel>();


	public void InitialiseCache()
	{
		// Fill a dictionary for quick lookup
	}
}