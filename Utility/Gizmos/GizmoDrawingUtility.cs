using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class GizmoUtility
{
	public static void DrawCircle(Vector3 position, float radius, int resolution = 16)
	{
		var oldMatrix = Gizmos.matrix;

		const int outlineResolution = 16;
		Vector3[] circleOutlinePoints = new Vector3[outlineResolution];

		const float fullOutline = Mathf.PI * 2f;
		const float perPointAngle = fullOutline / outlineResolution;

		for (int i = 0; i < outlineResolution; ++i)
		{
			circleOutlinePoints[i] = new Vector3(Mathf.Cos(perPointAngle * i), 0, Mathf.Sin(perPointAngle * i)) * radius;
		}

		var tsrMatrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
		Gizmos.matrix = tsrMatrix;

		for (int i = 0; i < outlineResolution; ++i)
		{
			Gizmos.DrawLine(circleOutlinePoints[i], circleOutlinePoints[(i + 1) % circleOutlinePoints.Length]);
		}


		Gizmos.matrix = oldMatrix;
	}
}