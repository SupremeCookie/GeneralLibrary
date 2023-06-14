using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

public class PerformanceMeasurer
{
#if UNITY_EDITOR
	private class Measurement : IComparable
	{
		public string name;
		public long timeTicks;

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}

			var otherData = obj as Measurement;
			if (otherData == null)
			{
				throw new System.ArgumentException($"Object is not a {typeof(Measurement).ToString()}");
			}

			return this.timeTicks.CompareTo(otherData.timeTicks);
		}
	}

	public class MetricMeasurement
	{
		public string name;
		public double timeTicks;
		public double durationSincePrevious;
	}


	private long utcTicks;
	private List<Measurement> measurements;

	public PerformanceMeasurer()
	{
		Init();
	}

	public void Init()
	{
		utcTicks = DateTime.UtcNow.Ticks;
		measurements = new List<Measurement>();
	}

	public void Clear()
	{
		utcTicks = long.MinValue;
		measurements.Clear();
	}

	public int Count()
	{
		return measurements?.Count ?? 0;
	}


	public void StoreEntry(string id)
	{
		measurements.Add(new Measurement
		{
			timeTicks = DateTime.UtcNow.Ticks - utcTicks,
			name = id,
		});
	}

	public List<MetricMeasurement> GetMetrics()
	{
		UnityEngine.Debug.Assert(!measurements.IsNullOrEmpty(), $"Can't return metrics, we have no measurements");
		var result = new List<MetricMeasurement>(measurements.Count);

		for (int i = 0; i < measurements.Count; ++i)
		{
			var previousTimeTicks = i == 0 ? 0 : measurements[i - 1].timeTicks;
			var currentMeasurement = measurements[i];

			var timeSpanTimeTicks = TimeSpan.FromTicks(currentMeasurement.timeTicks);
			var timeSpanDifferenceTicks = TimeSpan.FromTicks(currentMeasurement.timeTicks - previousTimeTicks);

			result.Add(new MetricMeasurement
			{
				name = currentMeasurement.name,
				timeTicks = timeSpanTimeTicks.TotalSeconds,
				durationSincePrevious = timeSpanDifferenceTicks.TotalSeconds,
			});
		}

		return result;
	}

	// We can support multiple windows
	public static void ShowEditorMetrics(List<MetricMeasurement> metrics, int windowIndex = 0)
	{
		Debug.Assert(PerformanceMeasurererWindowUtility.metricData.Length > windowIndex, $"The window we are trying to set metrics for does not exist, " +
			$"max count: {PerformanceMeasurererWindowUtility.metricData.Length},  current windowIndex: {windowIndex}");

		PerformanceMeasurererWindowUtility.metricData[windowIndex] = metrics;
		EditorApplication.ExecuteMenuItem($"{PerformanceMeasurererWindowUtility.MenuItem}_{windowIndex}");
	}
#endif
}
