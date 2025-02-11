using System.Collections.Generic;
using UnityEngine;

public class Vector2PointsGizmo : MonoBehaviour
{
	public bool manualUpdate = false;
	public bool closePoints = false;
	public bool drawLines = true;
	public Color color = Color.white;
	public float size = .125f;
	public GizmoType gizmoType = GizmoType.WireSphere;

	public List<List<Vector2>> data = new List<List<Vector2>>();

	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		var splinePoints = data;

		Vector2 pos = transform.position;

		for (int i = 0; i < splinePoints.Count; ++i)
		{
			var listOfPoints = splinePoints[i];
			if (!listOfPoints.IsNullOrEmpty())
			{
				for (int k = 0; k < listOfPoints.Count; ++k)
				{
					switch (gizmoType)
					{
						case GizmoType.WireSphere: { Gizmos.DrawWireSphere(listOfPoints[k] + pos, size); break; }
						case GizmoType.Sphere: { Gizmos.DrawSphere(listOfPoints[k] + pos, size); break; }
						case GizmoType.WireCube: { Gizmos.DrawWireCube(listOfPoints[k] + pos, Vector3.one * size); break; }
						case GizmoType.Cube: { Gizmos.DrawCube(listOfPoints[k] + pos, Vector3.one * size); break; }
					}

					if (k > 0 && drawLines)
					{
						Gizmos.DrawRay(listOfPoints[k] + pos, listOfPoints[k - 1] - listOfPoints[k]);
					}
				}

				if (closePoints && drawLines)
				{
					Gizmos.DrawRay(listOfPoints[0] + pos, listOfPoints[listOfPoints.Count - 1] - listOfPoints[0]);
				}
			}
		}
	}
}