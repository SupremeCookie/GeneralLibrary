using UnityEngine;

[UnityEngine.Scripting.Preserve]
[CreateAssetMenu(fileName = VECTOR3_SO_NAME, menuName = "_scriptables/Utility/" + VECTOR3_SO_NAME, order = 15)]
public class PersistentVector3 : ScriptableObject
{
	public const string VECTOR3_SO_NAME = "Persistent-Vector3";

	[SerializeField] private Vector3 value;

	public Vector3 GetValue() => value;
}
