// Note DK: The locterms can be used to automatically grab the right back-end to then get the right translated values
using UnityEngine;

public class LocTerm
{
	public static string GetLoca(string termKey, string fallback)
	{
		Debug.Assert(!string.IsNullOrEmpty(termKey), $"An empty key has been passed on, fallback value will be returned: {fallback}");

		// TODO DK: Once loca is in, make something here.
		return fallback;
	}
}
