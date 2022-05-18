using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MiddleOfEntity : MonoBehaviour
{
	public Vector2 MiddleOfEntityPosition { get { return GetMiddleOfEntity(); } }

	private Collider2D _collider;

	private bool _hasInitted = false;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (!_hasInitted)
		{
			_collider = GetComponent<Collider2D>();
			_hasInitted = true;
		}
	}

	private Vector2 GetMiddleOfEntity()
	{
		Init();
		return transform.position + (new Vector3(0, _collider.offset.y * transform.localScale.y));
	}
}
