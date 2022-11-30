using System.Collections.Generic;
using UnityEngine;

public class Vector2PointsGizmo : MonoBehaviour
{
	public bool ManualUpdate = false;
	public bool ClosePoints = false;
	public bool DrawLines = true;
	public Color color = Color.white;
	public float size = .125f;
	public GizmoType gizmoType = GizmoType.WireSphere;
	[SerializeField] public List<List<Vector2>> Data = new List<List<Vector2>>();

	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		var splinePoints = Data;

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
						case GizmoType.WireCube: { Gizmos.DrawWireCube(listOfPoints[k] + pos, UnityEngine.Vector3.one * size); break; }
						case GizmoType.Cube: { Gizmos.DrawCube(listOfPoints[k] + pos, UnityEngine.Vector3.one * size); break; }
					}

					if (k > 0 && DrawLines)
					{
						Gizmos.DrawRay(listOfPoints[k] + pos, listOfPoints[k - 1] - listOfPoints[k]);
					}
				}

				if (ClosePoints && DrawLines)
				{
					Gizmos.DrawRay(listOfPoints[0] + pos, listOfPoints[listOfPoints.Count - 1] - listOfPoints[0]);
				}
			}
		}
	}
}