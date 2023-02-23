using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PerformanceMeasurerWindow : EditorWindow
{
	private static List<PerformanceMeasurer.MetricMeasurement> metricData => PerformanceMeasurererWindowUtility.metricData;

	private static GUIStyle _mainEntry;
	private static GUIStyle mainEntry { get { if (_mainEntry == null) { InitStyles(); } return _mainEntry; } }
	private static GUIStyle _warningEntry;
	private static GUIStyle warningEntry { get { if (_warningEntry == null) { InitStyles(); } return _warningEntry; } }
	private static GUIStyle _megaWarningEntry;
	private static GUIStyle megaWarningEntry { get { if (_megaWarningEntry == null) { InitStyles(); } return _megaWarningEntry; } }

	[MenuItem(PerformanceMeasurererWindowUtility.MenuItem)]
	private static void MenuItem()
	{
		ShowWindowInternal();
	}

	public static void ShowWindow(List<PerformanceMeasurer.MetricMeasurement> metricData)
	{
		PerformanceMeasurererWindowUtility.metricData = metricData;
		ShowWindowInternal();
	}

	private static void ShowWindowInternal()
	{
		InitStyles();

		PerformanceMeasurerWindow window = (PerformanceMeasurerWindow)EditorWindow.GetWindow(typeof(PerformanceMeasurerWindow));
		window.Show();
	}

	private static void InitStyles()
	{
		void setBackground(ref GUIStyle style, Color col)
		{
			var tex = new Texture2D(1, 1);
			tex.SetPixel(0, 0, col);
			tex.Apply();
			style.normal.background = tex;
		}

		_mainEntry = new GUIStyle(EditorStyles.helpBox);
		_warningEntry = new GUIStyle(mainEntry);
		_megaWarningEntry = new GUIStyle(mainEntry);

		var color = new Color(82f / 255f, 82f / 255f, 82f / 255f);
		setBackground(ref _mainEntry, color);
		color = new Color(101f / 255f, 96f / 255f, 26f / 255f);
		setBackground(ref _warningEntry, color);
		color = new Color(101f / 255f, 34f / 255f, 26f / 255f);
		setBackground(ref _megaWarningEntry, color);
	}

	private void OnGUI()
	{
		GUILayout.Label("Performance Measurer Metrics", EditorStyles.boldLabel);
		GUILayout.Space(5);

		bool hasData = !metricData.IsNullOrEmpty();
		if (!hasData)
		{
			GUILayout.Label("No metric data available, please generate some first", EditorStyles.largeLabel);
		}
		else
		{
			DrawMetricData();
		}
	}

	private void DrawMetricData()
	{
		EditorGUILayout.BeginVertical(EditorStyles.helpBox);

		DrawHeading();

		for (int i = 0; i < metricData.Count; ++i)
		{
			DrawEntry(metricData[i]);
		}

		EditorGUILayout.EndHorizontal();
	}

	private void DrawHeading()
	{
		EditorGUILayout.BeginHorizontal();
		var columns = GetColumnPositions();

		GUILayout.Label("Name", GUILayout.MaxWidth(columns[0]));
		GUILayout.Label("Duration since Previous", GUILayout.MaxWidth(columns[1]));
		GUILayout.Label("Storage Time", GUILayout.MaxWidth(columns[2]));

		EditorGUILayout.EndHorizontal();
	}

	private void DrawEntry(PerformanceMeasurer.MetricMeasurement entry)
	{
		var drawAsWarning = entry.durationSincePrevious > 1.0f;
		var drawAsMegaWarning = entry.durationSincePrevious > 5.0f;

		GUIStyle chosenStyle = mainEntry;
		if (drawAsMegaWarning) { chosenStyle = megaWarningEntry; }
		else if (drawAsWarning) { chosenStyle = warningEntry; }


		EditorGUILayout.BeginHorizontal(chosenStyle);
		var columns = GetColumnPositions();

		GUILayout.Label($"{entry.name}", GUILayout.MaxWidth(columns[0]));
		GUILayout.Label($"{entry.durationSincePrevious}", GUILayout.MaxWidth(columns[1]));
		GUILayout.Label($"{entry.timeTicks}", GUILayout.MaxWidth(columns[2]));

		EditorGUILayout.EndHorizontal();
	}

	private float[] GetColumnPositions()
	{
		var availableSize = GetAvailableSize();
		//Debug.Log($"AvailableSize: {availableSize}");

		float firstSpace = availableSize * 0.4f;
		float secondSpace = (availableSize - firstSpace) * 0.5f;
		float thirdSpace = secondSpace;

		return new float[]
		{
			firstSpace,
			secondSpace,
			thirdSpace,
		};
	}

	private float cachedSize = 0f;
	private float GetAvailableSize()
	{
		cachedSize = Screen.width;
		return cachedSize;
	}
}
#endif