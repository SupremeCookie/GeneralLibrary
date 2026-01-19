using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = FLOAT_SO_NAME, menuName = "_scriptables/Utility/" + FLOAT_SO_NAME, order = 0)]
public class PersistentFloat : ScriptableObject
{
	public const string FLOAT_SO_NAME = "Persistent-Float";

	[SerializeField] private float value;

	public float GetValue() => value;
	public float GetSquaredValue() => value * value;
}