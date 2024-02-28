using UnityEngine;

public class LocTerm
{
	public static string GetLoca(string termKey, string fallback)
	{
		Debug.Assert(!string.IsNullOrEmpty(termKey), $"An empty key has been passed on, fallback value will be returned: {fallback}");

		// TODO DK: Once loca is in, make something here.
		fallback = fallback.Replace("\\n", System.Environment.NewLine);
		return fallback;
	}
}


// TODO DK: Propertydrawer so its all drawn on 1 line
[System.Serializable]
public class LocTermModel
{
	[SerializeField] private string key;
	[SerializeField] private string fallback;   // TODO DK: PropertyDrawer -- Upgrade to multiline if character count is high enough

	public LocTermModel() { }
	public LocTermModel(string key, string fallback) { this.key = key; this.fallback = fallback; }

	public override string ToString()
	{
		return LocTerm.GetLoca(key, fallback);
	}
}