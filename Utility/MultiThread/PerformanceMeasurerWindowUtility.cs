using System.Collections.Generic;


public static class PerformanceMeasurererWindowUtility
{
#if UNITY_EDITOR
	// Note DK: Should at some point become key based, not index based.
	private const int MaxMeasurerWindowCount = 2;

	public const string MenuItem = "Window/GeneralLibrary/PerformanceMeasurerMetrics";
	public static List<PerformanceMeasurer.MetricMeasurement>[] metricData;

	static PerformanceMeasurererWindowUtility()
	{
		metricData = new List<PerformanceMeasurer.MetricMeasurement>[MaxMeasurerWindowCount];
	}
#endif
}
