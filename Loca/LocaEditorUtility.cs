#if UNITY_EDITOR
using RogueLike;
using UnityEditor;
using UnityEngine;

public class LocaEditorUtility
{
	[MenuItem("DKat/Loca/Update All Loca Components")]
	private static void UpdateAllLocaComponents()
	{
#if UNITY_2019
		var allLocalizedTexts = Resources.FindObjectsOfTypeAll(typeof(LocalizedText)) as LocalizedText[];
#else
		var allLocalizedTexts = Object.FindObjectsOfType(typeof(LocalizedText), includeInactive: true) as LocalizedText[];
#endif

		if (allLocalizedTexts != null)
		{
			for (int i = 0; i < allLocalizedTexts.Length; ++i)
			{
#if UNITY_2019
				if(EditorUtility.IsPersistent(allLocalizedTexts[i].gameObject.transform.root.gameObject) && !(allLocalizedTexts[i].gameObject.hideFlags == HideFlags.NotEditable || allLocalizedTexts[i].gameObject.hideFlags == HideFlags.HideAndDontSave))
#endif

				allLocalizedTexts[i].UpdateManually();
			}

			SceneView.RepaintAll();
		}
	}
}
#endif