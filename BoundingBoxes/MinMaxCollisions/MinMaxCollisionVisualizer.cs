using System.Collections.Generic;
using UnityEngine;

public class MinMaxCollisionVisualizer : MonoBehaviour
{
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


	private bool _hasManuallyChangedGrid = false;

	private GameObject _gridCenter;
	private GameObject _boundingBoxCenter;

	private MinMax bbox;

	private List<Vector2> _gridPoints;

	private List<MinMax> _cellBBoxes;
	private List<MinMax> _overlapBBoxes;

	private bool _isFullyOverlapping;

	public void OnDrawGizmos()
	{
		_isFullyOverlapping = false;

		TryToInitializeGameObjects();

		if (!_hasManuallyChangedGrid)
		{
			TryToInitializeGrid();
		}


		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(_gridCenter.transform.position, Vector3.one * 0.1f);

		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(_boundingBoxCenter.transform.position, Vector3.one * 0.125f);


		TestInclusion();
		TestInclusionArea();


		DisplayGrid();
		DisplayGridBBoxes();
		DisplayInclusion();
		DisplayBoundingBox();
	}


	private void TryToInitializeGameObjects()
	{
		if (_gridCenter == null)
		{
			var gridTransform = transform.Find("GridCenter");
			if (gridTransform == null)
			{
				_gridCenter = new GameObject("GridCenter");
				_gridCenter.transform.SetParent(transform, false);
			}
			else
			{
				_gridCenter = gridTransform.gameObject;
			}
		}

		if (_boundingBoxCenter == null)
		{
			var boundingBoxTransform = transform.Find("BoundingBoxCenter");
			if (boundingBoxTransform == null)
			{
				_boundingBoxCenter = new GameObject("BoundingBoxCenter");
				_boundingBoxCenter.transform.SetParent(transform, false);
			}
			else
			{
				_boundingBoxCenter = boundingBoxTransform.gameObject;
			}
		}
	}

	private void TryToInitializeGrid()
	{
		int count = GridSize.x * GridSize.y;
		bool reinitializeGrid = _gridPoints == null || _gridPoints.Count != count;
		if (reinitializeGrid)
		{
			_gridPoints = new List<Vector2>(count);

			float cellSize = MinMaxCollisions.Constants.GridCellSize;
			Vector2 centerPosOffset = new Vector2(GridSize.x - 1, GridSize.y - 1) * cellSize * 0.5f;    // Note DK: -1 on both gridsizes, as we don't care about the node count, we care about the total width they encompass from center to center.

			for (int x = 0; x < GridSize.x; ++x)
			{
				for (int y = 0; y < GridSize.y; ++y)
				{
					Vector2 cellPoint = new Vector2(x, y) * cellSize;
					Vector2 offsetCellPoint = cellPoint - centerPosOffset;

					_gridPoints.Add(offsetCellPoint);
				}
			}
		}
	}

	private void TestInclusion()
	{
		if (_cellBBoxes.IsNullOrEmpty())
		{
			return;
		}

		if (_overlapBBoxes != null) { _overlapBBoxes.Clear(); }
		else { _overlapBBoxes = new List<MinMax>(); }

		var testBBox = new MinMax();
		testBBox.Min = bbox.Min;
		testBBox.Max = bbox.Max;
		testBBox.AddOffset(_boundingBoxCenter.transform.position);

		for (int i = 0; i < _cellBBoxes.Count; ++i)
		{
			var cellBBox = _cellBBoxes[i];
			bool hasOverlap = cellBBox.IsOverlapping(testBBox);

			if (hasOverlap)
			{
				var overlapRegion = cellBBox.GetOverlappingArea(testBBox);
				_overlapBBoxes.Add(overlapRegion);
			}
		}
	}

	private void TestInclusionArea()
	{
		if (_overlapBBoxes.IsNullOrEmpty())
		{
			return;
		}

		_isFullyOverlapping = MinMaxCollisionUtility.AreSurfaceAreasTheSame(bbox, in _overlapBBoxes);
	}


	private void DisplayGrid()
	{
		if (_gridPoints != null && _gridPoints.Count > 0)
		{
			float cellSize = MinMaxCollisions.Constants.GridCellSize;

			Gizmos.color = Color.cyan;

			for (int i = 0; i < _gridPoints.Count; ++i)
			{
				Gizmos.DrawWireSphere(_gridCenter.transform.position + (Vector3)_gridPoints[i], cellSize * 0.5f);    // * 0.5 as its the radius
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

			var bboxPos = _boundingBoxCenter.transform.position;

			Color color = _isFullyOverlapping ? Color.green : Color.magenta;
			bbox.DrawGizmos(bboxPos, drawCross: true, color: color);
		}
	}

	private void DisplayGridBBoxes()
	{
		if (_cellBBoxes != null)
		{
			for (int i = 0; i < _cellBBoxes.Count; ++i)
			{
				var current = _cellBBoxes[i];
				current.DrawGizmos(Vector3.zero, drawFill: true, color: bboxColors2[i % bboxColors2.Length]);
			}
		}
	}

	private void DisplayInclusion()
	{
		if (_overlapBBoxes != null)
		{
			for (int i = 0; i < _overlapBBoxes.Count; ++i)
			{
				var current = _overlapBBoxes[i];
				current.DrawGizmos(Vector3.zero, drawFill: true, color: Color.yellow);
			}
		}
	}




	// Note DK: First idea is to convert the grid cell points into a bunch of bounding boxes, with no holes.
	// Then we can test our own bbox against them, if the total surface of overlaps is like a 99.9% match with our bbox, then its encased.  
	// (encased here meaning, that our bbox is entirely within the surface area of the cells)
	public void ConvertGridToBBoxes()
	{
		float cellSize = MinMaxCollisions.Constants.GridCellSize;
		_cellBBoxes = BoundingBoxConverter.Convert(_gridPoints, cellSize);

		for (int i = 0; i < _cellBBoxes.Count; ++i)
		{
			_cellBBoxes[i].AddOffset(_gridCenter.transform.position);
		}
	}

	public void RemoveSomeCells()
	{
		ResetGrid();

		_hasManuallyChangedGrid = true;

		for (int i = 0; i < CellsToRemove; ++i)
		{
			var randomIndex = UnityEngine.Random.Range(0, _gridPoints.Count);
			_gridPoints.RemoveAt(randomIndex);
		}
	}

	public void ResetGrid()
	{
		_hasManuallyChangedGrid = false;
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