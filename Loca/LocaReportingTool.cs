using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
		{
			UpdateFallback(model);
			return;
		}

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

	private void UpdateFallback(LocTermModel model)
	{
		// We intrinsically demand the model to be registered
		var foundModel = FindRegisteredModel(model, out int index);
		Debug.Assert(foundModel, $"We expected the model to be present, it is not");

		registeredLocterms[index].UpdateFallback(model.fallbackInternal);
	}


#if UNITY_EDITOR
	public void DrawDebugControls()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		GUILayout.BeginVertical();

		if (GUILayout.Button("Clear"))
		{
			registeredLocterms = new List<LocTermModel>();
		}

		GUILayout.Space(15);

		if (GUILayout.Button("Sort ABC"))
		{
			var values = registeredLocterms
				.OrderBy(s => s.keyInternal)
				.ToList();

			registeredLocterms = values;

			EditorUtility.SetDirty(this);
		}

		GUILayout.EndVertical();

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Space(15);


		{
			GUILayout.BeginHorizontal();

			var keys = registeredLocterms
				.Select(s => s.keyInternal)
				.ToList();
			var keysString = string.Join(Environment.NewLine, keys);

			GUILayout.TextArea(keysString);


			var values = registeredLocterms
				.Select(s => s.fallbackInternal)
				.ToList();
			var valuesString = string.Join(Environment.NewLine, values);

			GUILayout.TextArea(valuesString);

			GUILayout.EndHorizontal();
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