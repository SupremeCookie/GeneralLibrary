﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static partial class Utility
{
	public static bool IsInPrefabMode(this GameObject gameObject)
	{
		if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
			return true;

		return false;
	}

	public static GameObject CreateInstance(GameObject origin)
	{
		GameObject instance = null;

#if UNITY_EDITOR
		if (PrefabUtility.IsPartOfAnyPrefab(origin))
			instance = PrefabUtility.InstantiatePrefab(origin) as GameObject;
		else
#endif
			instance = GameObject.Instantiate(origin);

		return instance;
	}
}