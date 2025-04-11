using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = SO_NAME, menuName = "_scriptables/Utility/" + SO_NAME, order = 0)]
public class PersistentColor : ScriptableObject
{
	public const string SO_NAME = "Persistent-Color";

	[SerializeField] private Color value;

	public Color GetValue() => value;
	public void SetValue(Color value) => this.value = value;
}
