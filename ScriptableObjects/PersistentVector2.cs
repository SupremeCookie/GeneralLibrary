using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = VECTOR2_SO_NAME, menuName = "_scriptables/Utility/" + VECTOR2_SO_NAME, order = 0)]
public class PersistentVector2 : ScriptableObject
{
	public const string VECTOR2_SO_NAME = "Persistent-Vector2";

	[SerializeField] private Vector2 value;

	public Vector2 GetValue() => value;
}
