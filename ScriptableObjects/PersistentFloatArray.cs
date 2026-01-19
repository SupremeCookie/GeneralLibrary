using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = FLOAT_SO_NAME, menuName = "_scriptables/Utility/" + FLOAT_SO_NAME, order = 100)]
public class PersistentFloatArray : ScriptableObject
{
	public const string FLOAT_SO_NAME = "Persistent-FloatArray";

	[SerializeField] private float[] value;

	public float[] GetValue() => value;
}