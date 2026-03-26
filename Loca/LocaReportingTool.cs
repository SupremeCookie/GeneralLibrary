using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = SO_NAME, menuName = "_scriptables/" + SO_NAME, order = 0)]
public class LocaReportingTool : CustomSO
{
	protected static LocaReportingTool pInstance;

	public const string SO_NAME = "LocaReportingTool";

	public static LocaReportingTool Instance
	{
		get
		{
			if (pInstance == null)
			{
				SetPInstance();
			}

			return pInstance as LocaReportingTool;
		}
	}

	protected static void SetPInstance()
	{
		Debug.Log($"[LocaReportingTool] SetPInstance");
		pInstance = CustomSO.LoadScriptableObject<LocaReportingTool>("ScriptableObjects/Loca/" + SO_NAME);
		Debug.Assert(pInstance != null, "[LocaReportingTool] instance is null");
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Data/" + SO_NAME)]
	public static void OpenScriptable()
	{
		OpenScriptableObject(Instance);
	}
#endif



	[SerializeField] private List<LocTermModel> registeredLocterms;

	public void RegisterLocterm(LocTermModel model)
	{
		bool hasAlreadyRegistered = FindRegisteredModel(model, out var index);
		if (hasAlreadyRegistered)
			return;

		registeredLocterms.Add(model);

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}

	private bool FindRegisteredModel(LocTermModel model, out int index)
	{
		if (registeredLocterms.IsNullOrEmpty())
		{
			index = -1;
			return false;
		}

		index = -1;
		for (int i = 0; i < registeredLocterms.Count; ++i)
		{
			if (registeredLocterms[i].keyInternal.Equals(model.keyInternal))
			{
				index = i;
				break;
			}
		}

		return index >= 0;
	}


#if UNITY_EDITOR
	public void DrawDebugControls()
	{
		if (GUILayout.Button("Clear"))
		{
			registeredLocterms = new List<LocTermModel>();
		}
	}
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(LocaReportingTool))]
public class LocaReportingToolEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		(target as LocaReportingTool).DrawDebugControls();
	}
}
#endif