// Note DK: The locterms can be used to automatically grab the right back-end to then get the right translated values
using UnityEngine;

public static class KeybindingLocTerm
{
	public static string GetLoca(string keyBindingKey, string fallback, ControlType controlType)
	{
		Debug.Assert(!string.IsNullOrEmpty(keyBindingKey), $"An empty key has been passed on, fallback value will be returned: {fallback}");

		// TODO DK: Once loca is in, make something here.
		fallback = fallback.Replace("\\n", System.Environment.NewLine);
		return fallback;
	}
}


// TODO DK: Propertydrawer so its all drawn on 1 line
[System.Serializable]
public class KeybindingLocTermModel
{
	[SerializeField] private string key;
	[SerializeField] private string fallback;   // TODO DK: Upgrade to multiline if character count is high enough
	[SerializeField] private ControlType controlType = ControlType.KeyBoardMouse;

	public KeybindingLocTermModel() { }
	public KeybindingLocTermModel(string key, string fallback, ControlType controlType) { this.key = key; this.fallback = fallback; this.controlType = controlType; }

	public override string ToString()
	{
		return KeybindingLocTerm.GetLoca(key, fallback, controlType);
	}

#if UNITY_EDITOR
	public void SetControlType(ControlType controlType)
	{
		this.controlType = controlType;
	}
#endif
}
