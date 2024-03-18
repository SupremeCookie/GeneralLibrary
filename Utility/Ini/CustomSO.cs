using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CustomSO : ScriptableObject
{
	protected static T LoadScriptableObject<T>(string name) where T : CustomSO
	{
		var scriptableObject = Resources.Load(name) as T;
		Debug.AssertFormat(scriptableObject != null, "ScriptableObject {0} is null", name);
		return scriptableObject;
	}

#if UNITY_EDITOR
	public static void OpenScriptableObject(CustomSO instance)
	{
		var scriptableObject = instance;
		UnityEditor.Selection.activeObject = scriptableObject;
#endif
	}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(CustomSO))]
public class CustomSODataEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		var castedTarget = (CustomSO)target;

		ScriptableUtility.DrawSaveButton_AtStart(target);

		base.OnInspectorGUI();

		UnityEditor.EditorGUILayout.Space();
		UnityEditor.EditorGUILayout.Space();
	}
}
#endif