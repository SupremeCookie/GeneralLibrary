using UnityEngine;

public class GizmosDrawCollider2D : MonoBehaviour
{
	public float gizmosSize;
	public bool drawCross;

#if UNITY_EDITOR
	private Collider2D _collider;

	private BoxCollider2D _boxCollider;
	private CircleCollider2D _circleCollider;
	private PolygonCollider2D _polyGonCollider;
	private CompositeCollider2D _compositeCollider;

	private Color _circleColor;

	private void Start() { }

	private void OnDrawGizmos()
	{
		if (!this.enabled)
		{
			return;
		}

		if (_collider == null)
		{
			_collider = GetComponentInChildren<Collider2D>();
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

				Vector2 pos = _collider.transform.position;
				var size = _boxCollider.size;
				var offset = _boxCollider.offset;
				float halfWidth = size.x * 0.5f;
				float halfHeight = size.y * 0.5f;

				int maxIndex = 4;

				Vector2[] positions = new Vector2[]
				{
					pos + new Vector2(-halfWidth, -halfHeight) + offset,
					pos + new Vector2(-halfWidth, halfHeight)+ offset,
					pos + new Vector2(halfWidth, halfHeight)+ offset,
					pos + new Vector2(halfWidth, -halfHeight)+ offset,
					new Vector2(),
					new Vector2(),
					new Vector2(),
				};

				if (drawCross)
				{
					maxIndex = 7;
					positions[4] = positions[1];
					positions[5] = positions[2];
					positions[6] = positions[0];
				}

				for (int i = 0; i < maxIndex; ++i)
				{
					Gizmos.color = colors[i % colors.Length];
					Gizmos.DrawLine(positions[i], positions[(i + 1) % maxIndex]);
				}

				for (int i = 0; i < maxIndex; ++i)
				{
					Gizmos.color = colors[i % colors.Length];
					Gizmos.DrawSphere(positions[i], gizmosSize);
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
				Vector2 pos = _collider.transform.position;

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
						Gizmos.DrawLine(path[k] + pos, path[(k + 1) % path.Length] + pos);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawSphere(path[k] + pos, gizmosSize);
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
				Vector2 pos = _collider.transform.position;

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
						Gizmos.DrawLine(path[k] + pos, path[(k + 1) % path.Length] + pos);
					}

					for (int k = 0; k < path.Length; ++k)
					{
						Gizmos.DrawSphere(path[k] + pos, gizmosSize);
					}
				}
			}
		}

		if (_collider is CircleCollider2D)
		{
			if (_circleCollider == null)
			{
				_circleCollider = _collider as CircleCollider2D;
				MakeRandomColor(UnityEngine.Random.Range(0, int.MaxValue));
				_circleColor = Gizmos.color;
			}
			else
			{
				Gizmos.color = _circleColor;

				Gizmos.DrawWireSphere(transform.position + (Vector3)_circleCollider.offset, _circleCollider.radius);
			}
		}
	}

	// TODO DK: Use HLSL colouring here, it makes for nicer gradients. Then convert it back to RGB
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
