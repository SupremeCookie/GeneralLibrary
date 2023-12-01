﻿#if UNITY_EDITOR
using RogueLike;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocaEditorUtility
{
	[MenuItem("DKat/Loca/Update All Loca Components")]
	private static void UpdateAllLocaComponents()
	{
		var allLocalizedTexts = Object.FindObjectsOfType(typeof(LocalizedText), includeInactive: true) as LocalizedText[];
		if (allLocalizedTexts != null)
		{
			for (int i = 0; i < allLocalizedTexts.Length; ++i)
			{
				allLocalizedTexts[i].DebugUpdate();
			}

			SceneView.RepaintAll();
		}
	}
}
#endif