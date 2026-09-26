using UnityEngine;

public class LocTermUtility
{
	public static string GetLoca(string termKey, string fallback)
	{
#if UNITY_EDITOR
		LocaReportingTool.Instance.RegisterLocterm(new LocTermModel(termKey, fallback));
#endif

		Debug.Assert(!string.IsNullOrEmpty(termKey), $"An empty key has been passed on, fallback value will be returned: {(string.IsNullOrEmpty(fallback) ? "empty-string" : fallback)}");

		// TODO DK: Once loca is in, make something here.
		if (!LocaManager.HasInstance)
		{
			Debug.LogError($"No instance of LocaManager could be found, returning fallback");
			return LocaManager.StaticCleanString(fallback);
		}

		return LocaManager.Instance.GetValueCurrentLanguage(termKey, fallback);
	}
}