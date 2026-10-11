using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IStatsAchievementsDataIO
{
	T GetValue<T>(string key, T defaultValue);
	void SetValue<T>(string key, T value);
}