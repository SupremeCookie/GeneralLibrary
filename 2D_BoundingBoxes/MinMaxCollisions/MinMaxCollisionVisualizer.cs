using System.Collections.Generic;
using UnityEngine;

namespace MinMaxCollisions
{
	public class MinMaxCollisionVisualizer : MonoBehaviour
	{
		private const float gridCellSize = 0.15f;

		private readonly Color[] bboxColors2 = new Color[]
		{
			new Color(1.0f,  0.0f,   0f),
			new Color(1.0f,  1f,   0f),
			new Color(1f,  0.0f,   1f),
			new Color(0f, 0.0f, 1.0f),
			new Color(0f, 1f, 1.0f),
			new Color(0f, 1f, 0f),
		};


		public IntVector2 GridSize;
		public Vector2 BoundingBoxSize;
		[Range(0, 25)] public int CellsToRemove = 0;


		private bool hasManuallyChangedGrid = false;

		private GameObject gridCenter;
		private GameObject boundingBoxCenter;

		private MinMax bbox;

		private List<Vector2> gridPoints;

		private List<MinMax> cellBBoxes;
		private List<MinMax> overlapBBoxes;

		private bool isFullyOverlapping;

		public void OnDrawGizmos()
		{
			isFullyOverlapping = false;

			TryToInitializeGameObjects();

			if (!hasManuallyChangedGrid)
			{
				TryToInitializeGrid();
			}


			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(gridCenter.transform.position, Vector3.one * 0.1f);

			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(boundingBoxCenter.transform.position, Vector3.one * 0.125f);

			TestInclusion();
			TestInclusionArea();

			DisplayGrid();
			DisplayGridBBoxes();
			DisplayInclusion();
			DisplayBoundingBox();
		}


		private void TryToInitializeGameObjects()
		{
			if (gridCenter == null)
			{
				var gridTransform = transform.Find("GridCenter");
				if (gridTransform == null)
				{
					gridCenter = new GameObject("GridCenter");
					gridCenter.transform.SetParent(transform, false);
				}
				else
				{
					gridCenter = gridTransform.gameObject;
				}
			}

			if (boundingBoxCenter == null)
			{
				var boundingBoxTransform = transform.Find("BoundingBoxCenter");
				if (boundingBoxTransform == null)
				{
					boundingBoxCenter = new GameObject("BoundingBoxCenter");
					boundingBoxCenter.transform.SetParent(transform, false);
				}
				else
				{
					boundingBoxCenter = boundingBoxTransform.gameObject;
				}
			}
		}

		private void TryToInitializeGrid()
		{
			int count = GridSize.x * GridSize.y;
			bool reinitializeGrid = gridPoints == null || gridPoints.Count != count;
			if (reinitializeGrid)
			{
				gridPoints = new List<Vector2>(count);

				float cellSize = gridCellSize;
				Vector2 centerPosOffset = new Vector2(GridSize.x - 1, GridSize.y - 1) * cellSize * 0.5f;    // Note DK: -1 on both gridsizes, as we don't care about the node count, we care about the total width they encompass from center to center.

				for (int x = 0; x < GridSize.x; ++x)
				{
					for (int y = 0; y < GridSize.y; ++y)
					{
						Vector2 cellPoint = new Vector2(x, y) * cellSize;
						Vector2 offsetCellPoint = cellPoint - centerPosOffset;

						gridPoints.Add(offsetCellPoint);
					}
				}
			}
		}

		private void TestInclusion()
		{
			if (cellBBoxes.IsNullOrEmpty())
			{
				return;
			}

			if (overlapBBoxes != null) { overlapBBoxes.Clear(); }
			else { overlapBBoxes = new List<MinMax>(); }

			var testBBox = new MinMax();
			testBBox.Min = bbox.Min;
			testBBox.Max = bbox.Max;
			testBBox.AddOffset(boundingBoxCenter.transform.position);

			for (int i = 0; i < cellBBoxes.Count; ++i)
			{
				var cellBBox = cellBBoxes[i];
				bool hasOverlap = cellBBox.IsOverlapping(testBBox);

				if (hasOverlap)
				{
					var overlapRegion = cellBBox.GetOverlappingArea(testBBox);
					overlapBBoxes.Add(overlapRegion);
				}
			}
		}

		private void TestInclusionArea()
		{
			if (overlapBBoxes.IsNullOrEmpty())
			{
				return;
			}

			isFullyOverlapping = MinMaxCollisionUtility.AreSurfaceAreasTheSame(bbox, in overlapBBoxes);
		}


		private void DisplayGrid()
		{
			if (gridPoints != null && gridPoints.Count > 0)
			{
				float cellSize = gridCellSize;

				Gizmos.color = Color.cyan;

				for (int i = 0; i < gridPoints.Count; ++i)
				{
					Gizmos.DrawWireSphere(gridCenter.transform.position + (Vector3)gridPoints[i], cellSize * 0.5f);    // * 0.5 as its the radius
				}
			}
		}

		private void DisplayBoundingBox()
		{
			bool drawBBox = BoundingBoxSize.sqrMagnitude > 0.1f;
			if (drawBBox)
			{
				if (bbox == null || !bbox.Width.IsCloseTo(BoundingBoxSize.x, 0.001f) || !bbox.Height.IsCloseTo(BoundingBoxSize.y, 0.001f))
				{
					bbox = new MinMax();
					bbox.Min = new Vector2(BoundingBoxSize.x, BoundingBoxSize.y) * -0.5f;
					bbox.Max = new Vector2(BoundingBoxSize.x, BoundingBoxSize.y) * 0.5f;
				}

				var bboxPos = boundingBoxCenter.transform.position;

				Color color = isFullyOverlapping ? Color.green : Color.magenta;
				bbox.DrawGizmos(bboxPos, drawCross: true, color: color);
			}
		}

		private void DisplayGridBBoxes()
		{
			if (cellBBoxes != null)
			{
				for (int i = 0; i < cellBBoxes.Count; ++i)
				{
					var current = cellBBoxes[i];
					current.DrawGizmos(Vector3.zero, drawFill: true, color: bboxColors2[i % bboxColors2.Length]);
				}
			}
		}

		private void DisplayInclusion()
		{
			if (overlapBBoxes != null)
			{
				for (int i = 0; i < overlapBBoxes.Count; ++i)
				{
					var current = overlapBBoxes[i];
					current.DrawGizmos(Vector3.zero, drawFill: true, color: Color.yellow);
				}
			}
		}




		// Note DK: First idea is to convert the grid cell points into a bunch of bounding boxes, with no holes.
		// Then we can test our own bbox against them, if the total surface of overlaps is like a 99.9% match with our bbox, then its encased.  
		// (encased here meaning, that our bbox is entirely within the surface area of the cells)
		public void ConvertGridToBBoxes()
		{
			float cellSize = gridCellSize;
			cellBBoxes = BoundingBoxConverter.Convert(gridPoints, cellSize);

			for (int i = 0; i < cellBBoxes.Count; ++i)
			{
				cellBBoxes[i].AddOffset(gridCenter.transform.position);
			}
		}

		public void RemoveSomeCells()
		{
			ResetGrid();

			hasManuallyChangedGrid = true;

			for (int i = 0; i < CellsToRemove; ++i)
			{
				var randomIndex = UnityEngine.Random.Range(0, gridPoints.Count);
				gridPoints.RemoveAt(randomIndex);
			}
		}

		public void ResetGrid()
		{
			hasManuallyChangedGrid = false;
			TryToInitializeGrid();
			ConvertGridToBBoxes();
		}
	}

#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(MinMaxCollisionVisualizer))]
	public class MinMaxCollisionVisualizerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(10);

			if (GUILayout.Button("Convert Grid to BBoxes"))
			{
				((MinMaxCollisionVisualizer)target).ConvertGridToBBoxes();

				UnityEditor.SceneView.RepaintAll();
			}

			if (GUILayout.Button("Remove Random Cells"))
			{
				((MinMaxCollisionVisualizer)target).RemoveSomeCells();

				UnityEditor.SceneView.RepaintAll();
			}

			if (GUILayout.Button("Reset grid"))
			{
				((MinMaxCollisionVisualizer)target).ResetGrid();

				UnityEditor.SceneView.RepaintAll();
			}
		}
	}
#endif
}