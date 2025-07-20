using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = INT_SO_NAME, menuName = "_scriptables/Utility/" + INT_SO_NAME, order = 0)]
public class PersistentInt : ScriptableObject
{
	public const string INT_SO_NAME = "Persistent-Int";

	[SerializeField] private int value;

	public int GetValue() => value;
}