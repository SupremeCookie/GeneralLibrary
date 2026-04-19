using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GlobalTimeScale : SingletonMonoBehaviour<GlobalTimeScale>
{
	private const float DEFAULT = 1.0f;
	private const float INVALID = 9999.0f;

	[SerializeField, Readonly] private List<TimeScale> global_ActiveTimeScales = new List<TimeScale>();
	[SerializeField, Readonly] private List<TimeScale> gameplay_ActiveTimeScales = new List<TimeScale>();
	[SerializeField, Readonly] private List<TimeScale> tutorial_ActiveTimeScales = new List<TimeScale>();

	public float GetActiveTimescale(TimescaleLevel level = TimescaleLevel.Gameplay)
	{
		float lowestScale = GetLowestScaleForHigherLevels(level);
		float currentLevelTimescale = GetTimescaleForLevel(level);

		float result = Mathf.Min(lowestScale, currentLevelTimescale);
		if (result.IsCloseTo(INVALID, 1))
			return DEFAULT;

		return result;
	}

	private float GetLowestScaleForHigherLevels(TimescaleLevel level)
	{
		float lowestScale = INVALID;

		int startPoint = (int)level;
		if (startPoint == 0)
			return lowestScale;

		for (int i = startPoint - 1; i >= 0; --i)
		{
			lowestScale = Mathf.Min(lowestScale, GetTimescaleForLevel((TimescaleLevel)i));
		}

		return lowestScale;
	}

	private float GetTimescaleForLevel(TimescaleLevel level)
	{
		float lowestScale = INVALID;

		// TODO DK: Make some kind of, once a frame we can cache this value type thing.
		List<TimeScale> scales = GetLevelScales(level);
		if (scales.IsNullOrEmpty())
			return INVALID;

		for (int i = 0; i < scales.Count; ++i)
			lowestScale = Mathf.Min(scales[i].timescale, lowestScale);

		// We don't do negative timescales here.
		return Mathf.Max(lowestScale, 0f);
	}


	public void RegisterTimescale(string name, float value, TimescaleLevel scaleLevel, out TimeScale resultScale)
	{
		resultScale = new TimeScale(value, name);

		bool hasBeenRegisteredAlready = CheckRegistration(resultScale, scaleLevel, out var existingScale);
		if (hasBeenRegisteredAlready)
		{
			resultScale = existingScale;
			return;
		}

		RegisterTimescaleInternal(resultScale, scaleLevel);
	}

	public void RemoveTimescale(string name, TimescaleLevel scaleLevel, bool isAllowedToFail = false)
	{
		int indexOfScale = GetIndexOfTimescale(name, scaleLevel);
		Debug.Assert(isAllowedToFail || indexOfScale >= 0, $"Trying to remove ({name} - {scaleLevel}) timescale, but it doesn't exist. Found index: {indexOfScale}");

		if (indexOfScale >= 0)
		{
			var scales = GetLevelScales(scaleLevel);
			scales.RemoveAt(indexOfScale);
		}
	}

	private void RegisterTimescaleInternal(TimeScale scale, TimescaleLevel level)
	{
		switch (level)
		{
			case TimescaleLevel.Gameplay:
			{
				gameplay_ActiveTimeScales.Add(scale);
				break;
			}
			case TimescaleLevel.Global:
			{
				global_ActiveTimeScales.Add(scale);
				break;
			}
			case TimescaleLevel.Tutorial:
			{
				tutorial_ActiveTimeScales.Add(scale);
				break;
			}

			default:
			{
				Debug.LogError($"No case made for level: {level}, please add");
				break;
			}
		}
	}


	public bool HasTimescale(string name, TimescaleLevel level = TimescaleLevel.Gameplay)
	{
		int indexOfExisting = GetIndexOfTimescale(name, level);
		return indexOfExisting >= 0;
	}

	private bool CheckRegistration(TimeScale scale, TimescaleLevel level, out TimeScale existingScale)
	{
		existingScale = null;
		var scales = GetLevelScales(level, errorOnNotFound: false);
		if (scales.IsNullOrEmpty())
			return false;

		int indexOfExisting = GetIndexOfTimescale(scale.id, level);
		if (indexOfExisting < 0)
			return false;

		existingScale = scales[indexOfExisting];
		return true;
	}

	private int GetIndexOfTimescale(string id, TimescaleLevel level)
	{
		var scales = GetLevelScales(level, errorOnNotFound: true);

		string lookupID = id.ToLowerInvariant();
		for (int i = 0; i < scales.Count; ++i)
		{
			var otherId = scales[i].id;
			if (otherId.Length == lookupID.Length && otherId.ToLowerInvariant().Equals(lookupID))     // Note DK: Slightly cheaper lookup, as only similarly sized ids are equal-checked
			{
				return i;
			}
		}

		return -1;
	}

	private List<TimeScale> GetLevelScales(TimescaleLevel level, bool errorOnNotFound = false)
	{
		var result = gameplay_ActiveTimeScales;
		switch (level)
		{
			case TimescaleLevel.Gameplay:
				break;

			case TimescaleLevel.Global:
				result = global_ActiveTimeScales;
				break;

			case TimescaleLevel.Tutorial:
				result = tutorial_ActiveTimeScales;
				break;

			default:
			{
				if (errorOnNotFound)
				{
					Debug.LogError($"No case made for level: {level}, please add");
				}

				break;
			}
		}

		return result;
	}


#if UNITY_EDITOR
	public void Draw_DebugControls()
	{

	}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(GlobalTimeScale))]
public class GlobalTimeScaleEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		(target as GlobalTimeScale).Draw_DebugControls();
	}
}
#endif