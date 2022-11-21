// Note DK: The locterms can be used to automatically grab the right back-end to then get the right translated values
using UnityEngine;

public static class KeybindingLocTerm
{
	public static string GetLoca(string keyBindingKey, string fallback)
	{
		Debug.Assert(!string.IsNullOrEmpty(keyBindingKey), $"An empty key has been passed on, fallback value will be returned: {fallback}");

		// TODO DK: Once loca is in, make something here.
		return fallback;
	}
}


// TODO DK: Propertydrawer so its all drawn on 1 line
[System.Serializable]
public class KeybindingLocTermModel
{
	[SerializeField] private string key;
	[SerializeField] private string fallback;

	public override string ToString()
	{
		return KeybindingLocTerm.GetLoca(key, fallback);
	}
}
