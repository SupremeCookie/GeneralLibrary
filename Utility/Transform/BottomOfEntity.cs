using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BottomOfEntity : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField]
	private float _additionalYOffset;
#pragma warning restore 0649

	public Vector3 BottomOfEntityPosition { get { return GetBottomOfEntity(); } }

	private Collider2D _collider;

#if !UNITY_EDITOR
	private Vector3 _additionalOffset;
	private Vector3 _colliderOffset;
#endif

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();

#if !UNITY_EDITOR
		_additionalOffset = new Vector3(0, _additionalYOffset, 0);
		_colliderOffset = new Vector3(0, _collider.bounds.size.y * 0.5f, 0);
#endif
	}

	private Vector3 GetBottomOfEntity()
	{
#if UNITY_EDITOR
		if (_collider == null)
		{
			_collider = GetComponent<Collider2D>();
		}

		return transform.position - new Vector3(0, _additionalYOffset, 0) - new Vector3(0, _collider.bounds.size.y * 0.5f, 0);
#else
		return transform.position - _additionalOffset - _colliderOffset;
#endif
	}
}
