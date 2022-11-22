// Note DK: The locterms can be used to automatically grab the right back-end to then get the right translated values
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
	[SerializeField] private string fallback;   // TODO DK: Upgrade to multiline if character count is high enough

	public override string ToString()
	{
		return LocTerm.GetLoca(key, fallback);
	}
}