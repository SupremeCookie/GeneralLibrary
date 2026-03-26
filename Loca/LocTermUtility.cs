using UnityEngine;

public class LocTermUtility
{
	public static string GetLoca(string termKey, string fallback)
	{
#if UNITY_EDITOR
		LocaReportingTool.Instance.RegisterLocterm(new LocTermModel(termKey, fallback));
#endif

		Debug.Assert(!string.IsNullOrEmpty(termKey), $"An empty key has been passed on, fallback value will be returned: {fallback}");

		// TODO DK: Once loca is in, make something here.
		fallback = fallback.Replace("\\n", System.Environment.NewLine);
		return fallback;
	}
}