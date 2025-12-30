using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = BOOL_SO_NAME, menuName = "_scriptables/Utility/" + BOOL_SO_NAME, order = 0)]
public class PersistentBool : ScriptableObject
{
	public const string BOOL_SO_NAME = "Persistent-Bool";

	[SerializeField] private bool value;

	public bool GetValue() => value;
	public bool SetValue(bool value) => this.value = value;
}
