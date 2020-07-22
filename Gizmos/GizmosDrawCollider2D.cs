using UnityEngine;

public class GizmosDrawCollider2D : MonoBehaviour
{
#if UNITY_EDITOR
	private void Start() { }

	public float GizmosSize;

	private Collider2D _collider;

	private BoxCollider2D _boxCollider;
	private PolygonCollider2D _polyGonCollider;
	private CompositeCollider2D _compositeCollider;

	private void OnDrawGizmos()
	{
		if (!this.enabled)
		{
			return;
		}

		Vector2 position = transform.position;

		if (_collider == null)
		{
			_collider = GetComponent<Collider2D>();
			return;
		}

		if (_collider is BoxCollider2D)
		{
			if (_boxCollider == null)
			{
				_boxCollider = _collider as BoxCollider2D;
			}
			else
			{
				Color[] colors = new Color[]
				{
					new Color(102f/255f, 163f/255f, 255f/255f, 0.4f),
					new Color(0f/255f, 255f/255f, 255f/255f, 0.4f),
					new Color(102f/255f, 255f/255f, 153f/255f, 0.4f),
					new Color(102f/255f, 163f/255f, 255f/255f, 0.4f),
				};

				Vector2 pos = transform.position;
				var size = _boxCollider.size;
				var offset = _boxCollider.offset;
				float halfWidth = size.x * 0.5f;
				float halfHeight = size.y * 0.5f;

				Vector2[] positions = new Vector2[]
				{
					pos + new Vector2(-halfWidth, -halfHeight) + offset,
					pos + new Vector2(-halfWidth, halfHeight)+ offset,
					pos + new Vector2(halfWidth, halfHeight)+ offset,
					pos + new Vector2(halfWidth, -halfHeight)+ offset,
				};

				for (int i = 0; i < positions.Length; ++i)
				{
					Gizmos.color = colors[i];
					Gizmos.DrawLine(positions[i], positions[(i + 1) % positions.Length]);
				}

				for (int i = 0; i < positions.Length; ++i)
				{
					Gizmos.color = colors[i];
					Gizmos.DrawSphere(positions[i], GizmosSize);
				}
			}
		}

		if (_collider is PolygonCollider2D)
		{
			if (_polyGonCollider == null)
			{
				_polyGonCollider = _collider as PolygonCollider2D;
			}
			else
			{
				var pathCounts = _polyGonCollider.pathCount;
				for (int i = 0; i < pathCounts; ++i)
				{
					MakeRandomColor(i + gameObject.transform.GetSiblingIndex());

					var path = _polyGonCollider.GetPath(i);
					for (int m = 0; m < path.Length; ++m)
					{
						path[m] = path[m].MultiplyByVec2(transform.localScale);
						path[m] = path[m].RotateVectorAroundVector(Vector2.zero, transform.rotation.eulerAngles.z);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawLine(path[k] + position, path[(k + 1) % path.Length] + position);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawSphere(path[k] + position, GizmosSize);
					}
				}
			}
		}

		if (_collider is CompositeCollider2D)
		{
			if (_compositeCollider == null)
			{
				_compositeCollider = _collider as CompositeCollider2D;
			}
			else
			{
				var pathCounts = _compositeCollider.pathCount;
				for (int i = 0; i < pathCounts; ++i)
				{
					MakeRandomColor(i + gameObject.transform.GetSiblingIndex());

					var pointsInPath = _compositeCollider.GetPathPointCount(i);
					var path = new Vector2[pointsInPath];
					_compositeCollider.GetPath(i, path);
					for (int m = 0; m < path.Length; ++m)
					{
						path[m] = path[m].MultiplyByVec2(transform.localScale);
						path[m] = path[m].RotateVectorAroundVector(Vector2.zero, transform.rotation.eulerAngles.z);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawLine(path[k], path[(k + 1) % path.Length]);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawSphere(path[k], GizmosSize);
					}
				}
			}
		}
	}

	private void MakeRandomColor(int seed)
	{
		Random.InitState(seed);

		int rand = Random.Range(0, 3);
		float red = 0;
		float green = 0;
		float blue = 0;

		if (rand == 0)
		{
			red = 192f;
		}
		else if (rand == 1)
		{
			green = 192f;
		}
		else if (rand == 2)
		{
			blue = 192f;
		}

		rand = Random.Range(0, 3);
		if (rand == 0)
		{
			green = 128f;
		}
		else if (rand == 1)
		{
			blue = 128f;
		}
		else if (rand == 2)
		{
			red = 128f;
		}

		rand = Random.Range(0, 3);
		if (rand == 0)
		{
			blue += 64f;
		}
		else if (rand == 1)
		{
			red += 64f;
		}
		else if (rand == 2)
		{
			green += 64f;
		}

		float totalAmount = red + green + blue;
		if (totalAmount < 255f)
		{
			red = Mathf.Min(255f, red + 64f);
			blue = Mathf.Min(255f, blue + 64f);
			green = Mathf.Min(255f, green + 64f);
		}

		var color = new Color(red / 255f, green / 255f, blue / 255f);

		Gizmos.color = color;
	}
#endif
}
