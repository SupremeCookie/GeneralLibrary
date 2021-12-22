using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class DisplayChildBoundingBox : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private MinMax _boundingBox;
	[SerializeField] private MinMax _size;

	public void OnValidate()
	{
		ReloadChildren();
		RecalculateBoundingBox();
	}

	public void Update()
	{
		OnValidate();
	}

	private Collider2D[] _childColliders;
	private void ReloadChildren()
	{
		_childColliders = GetComponentsInChildren<Collider2D>(false);
	}

	private void RecalculateBoundingBox()
	{
		_boundingBox = new MinMax();
		_size = new MinMax();

		for (int i = 0; i < _childColliders.Length; ++i)
		{
			var current = _childColliders[i];
			if (current is BoxCollider2D)
			{
				var boxColl = current as BoxCollider2D;
				_boundingBox.TryStoreNewMinOrMax(boxColl.transform.localPosition
					+ new Vector3(boxColl.size.x * 0.5f + boxColl.offset.x, boxColl.size.y * 0.5f + boxColl.offset.y));

				_boundingBox.TryStoreNewMinOrMax(boxColl.transform.localPosition
					+ new Vector3(boxColl.size.x * -0.5f + boxColl.offset.x, boxColl.size.y * -0.5f + boxColl.offset.y));
			}
			else
			{
				Debug.LogWarning("No implementation written yet for type: (" + current.GetType() + ")");
			}
		}

		_size.Min = new Vector2(0, 0);
		_size.Max = new Vector2(_boundingBox.Width, _boundingBox.Height);
	}


	private void OnDrawGizmos()
	{
		if (_boundingBox != null)
		{
			Gizmos.color = Color.red;

			var positions = new Vector3[]
			{
				transform.position + new Vector3(_boundingBox.Min.x, _boundingBox.Min.y),
				transform.position + new Vector3(_boundingBox.Min.x, _boundingBox.Max.y),
				transform.position + new Vector3(_boundingBox.Max.x, _boundingBox.Max.y),
				transform.position + new Vector3(_boundingBox.Max.x, _boundingBox.Min.y),
			};

			for (int i = 0; i < positions.Length; ++i)
			{
				Gizmos.DrawCube(positions[i], Vector3.one * 0.05f);

				Gizmos.DrawLine(positions[i], positions[(i + 1) % 4]);
			}
		}
	}
#endif
}