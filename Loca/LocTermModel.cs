
// TODO DK: Propertydrawer so its all drawn on 1 line
using UnityEngine;

[System.Serializable]
public class LocTermModel
{
	[SerializeField] private string key;
	[SerializeField] private string fallback;   // TODO DK: PropertyDrawer -- Upgrade to multiline if character count is high enough

	public LocTermModel() { }
	public LocTermModel(string key, string fallback) { this.key = key; this.fallback = fallback; }

	public override string ToString()
	{
		return LocTermUtility.GetLoca(key, fallback);
	}
}