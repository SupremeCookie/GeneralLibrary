using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MiddleOfEntity : MonoBehaviour
{
	public Vector2 MiddleOfEntityPosition { get { return GetMiddleOfEntity(); } }

	private Collider2D _collider;

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	private Vector2 GetMiddleOfEntity()
	{
		return transform.position + (new Vector3(0, _collider.offset.y * transform.localScale.y));
	}
}
