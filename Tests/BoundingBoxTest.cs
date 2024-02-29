using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BBoxTestModel
{
	public float Width;
	public float Height;
}

[ExecuteInEditMode]
public class BoundingBoxTest : MonoBehaviour
{
	public List<BBoxTestModel> BBoxes;

	private List<GameObject> BBoxCenters;
	private List<int> bboxIndexOverlapping = new List<int>();
	private List<MinMaxRectangle> bboxOverlappingAreas = new List<MinMaxRectangle>();

	private void OnDrawGizmos()
	{
		if (BBoxes == null || BBoxes.Count == 0)
			return;

		CreateEnoughCenters();

		ConstructBBoxes();
		CheckForOverlap();

		DrawBBoxes();
	}

	private void CreateEnoughCenters()
	{
		if (BBoxCenters == null)
		{
			BBoxCenters = new List<GameObject>(BBoxes.Count);
		}

		BBoxCenters.Clear();

		var centersComponents = GetComponentsInChildren<GizmosDrawPoint>(true);
		if (centersComponents.Length > 0)
		{
			for (int i = 0; i < centersComponents.Length; ++i)
			{
				BBoxCenters.Add(centersComponents[i].gameObject);
			}
		}

		int requestedToAdd = BBoxes.Count - centersComponents.Length;
		if (requestedToAdd > 0)
		{
			CreateBBoxCenters(requestedToAdd);
		}
		else if (requestedToAdd < 0)
		{
			DeleteBBoxCenters(requestedToAdd);
		}


		for (int i = 0; i < BBoxCenters.Count; ++i)
		{
			if (BBoxCenters[i].gameObject != null)
				BBoxCenters[i].SetActive(true);
		}
	}

	private void CreateBBoxCenters(int count)
	{
		for (int i = 0; i < count; ++i)
		{
			var newGO = new GameObject("BBoxCenter");
			newGO.transform.SetParent(transform, false);
			newGO.transform.localPosition = new Vector3();

#if UNITY_EDITOR
			var drawGizmo = newGO.AddComponent<GizmosDrawPoint>();
			drawGizmo.gizmosColor = Color.green;
			drawGizmo.gizmosSize = 0.25f;
			drawGizmo.gizmoType = GizmoType.Cube;
#endif

			BBoxCenters.Add(newGO);
		}
	}

	private void DeleteBBoxCenters(int count)
	{
		var bboxCenters = GetComponentsInChildren<GizmosDrawPoint>(true);
		for (int i = 0; i < count; ++i)
		{
			bboxCenters[(bboxCenters.Length - (1 + i))].gameObject.SetActive(false);
		}
	}


	private List<MinMaxRectangle> actualBBoxes;
	private void ConstructBBoxes()
	{
		if (BBoxCenters.Count == 0)
		{
			return;
		}

		actualBBoxes = new List<MinMaxRectangle>();
		for (int i = 0; i < BBoxes.Count; ++i)
		{
			var current = BBoxes[i];
			float halfWidth = current.Width * 0.5f;
			float halfHeight = current.Height * 0.5f;

			var pos = BBoxCenters[i].transform.position;

			actualBBoxes.Add(new MinMaxRectangle
			{
				max = pos + new Vector3(halfWidth, halfHeight),
				min = pos + new Vector3(-halfWidth, -halfHeight),
			});
		}
	}

	private void CheckForOverlap()
	{
		bboxIndexOverlapping.Clear();
		bboxOverlappingAreas.Clear();

		for (int i = 0; i < actualBBoxes.Count; ++i)
		{
			var current = actualBBoxes[i];

			for (int k = 0; k < actualBBoxes.Count; ++k)
			{
				if (k == i)
					continue;

				var other = actualBBoxes[k];
				bool isOverlap = current.IsOverlapping(other);

				if (isOverlap)
				{
					var overlapArea = current.GetOverlappingArea(other);
					bboxOverlappingAreas.Add(overlapArea);
					bboxIndexOverlapping.Add(i);
				}
			}
		}
	}

	private void DrawBBoxes()
	{
		if (BBoxCenters.Count == 0)
		{
			return;
		}

		for (int i = 0; i < BBoxes.Count; ++i)
		{
			var current = actualBBoxes[i];
			var relatedCenter = BBoxCenters[i];

			var color = bboxIndexOverlapping.Contains(i) ? Color.red : Color.blue;
			Gizmos.color = color;

			DrawBBox(current);
		}

		for (int i = 0; i < bboxOverlappingAreas.Count; ++i)
		{
			var current = bboxOverlappingAreas[i];

			Gizmos.color = Color.yellow;

			DrawBBox(current);
		}
	}

	private void DrawBBox(MinMaxRectangle bbox)
	{
		var positions = new Vector2[]
		{
				bbox.max,
				new Vector2(bbox.max.x, bbox.min.y),
				bbox.min,
				new Vector2(bbox.min.x, bbox.max.y),
		};

		for (int k = 0; k < positions.Length; ++k)
		{
			Gizmos.DrawSphere(positions[k], 0.25f);
			Gizmos.DrawLine(positions[k], positions[(k + 1) % positions.Length]);
		}
	}
}