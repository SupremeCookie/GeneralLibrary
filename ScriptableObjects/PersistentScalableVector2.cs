using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = SCALABLE_VECTOR2_SO_NAME, menuName = "_scriptables/Utility/" + SCALABLE_VECTOR2_SO_NAME, order = 200)]
public class PersistentScalableVector2 : ScriptableObject
{
	public const string SCALABLE_VECTOR2_SO_NAME = "Persistent-Scalable-Vector2";

	[SerializeField] private Vector2 value;
	[SerializeField] private float scale = 1.0f;

	public Vector2 GetValue() => value * scale;
}
