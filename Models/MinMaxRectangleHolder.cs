using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxRectangleHolder : MonoBehaviour
{
	public enum RotationType
	{
		XY,
		XZ,
	}

	[SerializeField] private MinMaxRectangle rect;
	[Space(10)]
	[SerializeField] private Color color;
	[SerializeField] private RotationType rotationPlane = RotationType.XZ;

	public MinMaxRectangle GetRectangleCopy()
	{
		Vector2 offset = transform.position;
		if (rotationPlane == RotationType.XZ)
			offset.y = transform.position.z;

		var result = new MinMaxRectangle(rect);
		result.AddOffset(offset);
		return result;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = color;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);

		Vector2[] points = new Vector2[4]
		{
			rect.min,
			new Vector2(rect.min.x, rect.max.y),
			rect.max,
			new Vector2(rect.max.x, rect.min.y),
		};

		if (rotationPlane == RotationType.XZ)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.DrawSphere(points[i].XYVectorToXZ(), 0.5f);
				Gizmos.DrawLine(points[i].XYVectorToXZ(), points[(i + 1) % points.Length].XYVectorToXZ());
			}
		}
		else
		{
			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.DrawSphere(points[i], 0.5f);
				Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
			}
		}
	}
}
