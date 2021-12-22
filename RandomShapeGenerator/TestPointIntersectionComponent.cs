using Curves;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RandomShapeGenerator
{
	[ExecuteInEditMode]
	public class TestPointIntersectionComponent : MonoBehaviour
	{
		public BezierSplineScriptableObject ShapeStruct;
		public RandomShape[] RandomShapes;
		public float StepSize = 0.3f;
		public float ShapeXJump = 5.0f;

		[SerializeField] private List<Vector2PointsGizmo> _pointDisplayers;

		public void CleanShapes()
		{
			RandomShapes = new RandomShape[0];

			_pointDisplayers = GetComponentsInChildren<Vector2PointsGizmo>().ToList();

			if (_pointDisplayers.Count > 0)
			{
				for (int i = _pointDisplayers.Count - 1; i >= 0; --i)
				{
					if (_pointDisplayers[i] == null)
					{
						continue;
					}

					if (Application.isPlaying)
					{
						GameObject.Destroy(_pointDisplayers[i].gameObject);
					}
					else
					{
						GameObject.DestroyImmediate(_pointDisplayers[i].gameObject);
					}
				}
			}

			_pointDisplayers = new List<Vector2PointsGizmo>();

			_boundingBoxes = new List<MinMax>();
			_cellPositions = new List<List<Vector2>>();
			_acceptedPoints = new List<Tuple<int, List<Vector2>>>();
			_declinedPoints = new List<Tuple<int, List<Vector2>>>();
		}

		public void GenerateShapes()
		{
			Debug.Assert(ShapeStruct != null, "ShapeStruct is null");


			int shapeCount = 2;
			RandomShapes = new RandomShape[shapeCount];

			for (int i = 0; i < shapeCount; ++i)
			{
				RandomShapes[i] = RandomShapeGenerator.Generate(
					new ShapeStructure { ShapeSpline = ShapeStruct.Spline },
#if RogueLike
					IniControl.LevelGenData.LevelGen_PointsForRandomShapes,
					IniControl.LevelGenData.LevelGen_JitterRange,
					IniControl.LevelGenData.LevelGen_JitterCenterPosRange
#else               // Note DK: Gotta make sure these values are right, for non-roguelike's. Definitely need to get some general IniControl stuff.
					8,
					new Vector2(0, 5),
					new Vector2(0, 5)
#endif
					);
			}


			_pointDisplayers = GetComponentsInChildren<Vector2PointsGizmo>().ToList();
			if (_pointDisplayers == null || _pointDisplayers.Count < shapeCount)
			{
				int startIndex = _pointDisplayers.Count;
				var compsToAdd = shapeCount - _pointDisplayers.Count;
				for (int i = 0; i < compsToAdd; ++i)
				{
					var newGO = new GameObject("PointDisplay");
					newGO.transform.SetParent(transform, false);
					newGO.transform.position = new Vector3((startIndex + i) * ShapeXJump, 0, 0);

					var pointDisplay = newGO.AddComponent<Vector2PointsGizmo>();
					_pointDisplayers.Add(pointDisplay);
				}

				// TODO DK: remove the left over objects.
			}

			for (int i = 0; i < _pointDisplayers.Count; ++i)
			{
				var currentDisp = _pointDisplayers[i];
				var currentShape = RandomShapes[i];

				currentDisp.ClosePoints = true;
				currentDisp.color = Color.white;
				currentDisp.Data = new System.Collections.Generic.List<System.Collections.Generic.List<Vector2>> { currentShape.Positions };
			}


			GenerateBoundingBoxes();
			GenerateCellPoints();
		}

		private List<MinMax> _boundingBoxes;
		private List<List<Vector2>> _cellPositions;
		private void GenerateBoundingBoxes()
		{
			_boundingBoxes = new List<MinMax>();

			for (int i = 0; i < RandomShapes.Length; ++i)
			{
				var minMax = new MinMax();

				var points = RandomShapes[i].Positions;
				for (int p = 0; p < points.Count; ++p)
				{
					var point = points[p];
					minMax.TryStoreNewMinOrMax(point);
				}

				_boundingBoxes.Add(minMax);
			}
		}

		private void GenerateCellPoints()
		{
			_cellPositions = new List<List<Vector2>>();

			for (int i = 0; i < _boundingBoxes.Count; ++i)
			{
				var currentBBox = _boundingBoxes[i];
				var min = currentBBox.Min;

				var width = currentBBox.Width;
				var height = currentBBox.Height;

				var widthCount = Mathf.FloorToInt(width / StepSize);
				var heightCount = Mathf.FloorToInt(height / StepSize);

				var resampledWidthStep = width / widthCount;
				var resampledHeightStep = height / heightCount;


				List<Vector2> positions = new List<Vector2>(widthCount * heightCount);
				Vector2 startPos = new Vector2(resampledWidthStep * 0.5f, resampledHeightStep * 0.5f);

				for (int x = 0; x < widthCount; ++x)
				{
					for (int y = 0; y < heightCount; ++y)
					{
						positions.Add(new Vector2(startPos.x + (resampledWidthStep * x) + min.x, startPos.y + (resampledHeightStep * y) + min.y));
					}
				}

				_cellPositions.Add(positions);
			}
		}

		private List<Tuple<int, List<Vector2>>> _acceptedPoints;
		private List<Tuple<int, List<Vector2>>> _declinedPoints;

		public void CalculateInclusion()
		{
			_acceptedPoints = new List<Tuple<int, List<Vector2>>>();
			_declinedPoints = new List<Tuple<int, List<Vector2>>>();

			for (int i = 0; i < _cellPositions.Count; ++i)
			{
				var currentShape = RandomShapes[i];
				var currentColl = _cellPositions[i];

				var acceptedPoints = new List<Vector2>(currentColl.Count);
				var declinedPoints = new List<Vector2>(currentColl.Count);

				for (int p = 0; p < currentColl.Count; ++p)
				{
					var point = currentColl[p];
					bool isInShape = currentShape.IsInShape(point);

					if (isInShape)
					{
						acceptedPoints.Add(point);
					}
					else
					{
						declinedPoints.Add(point);
					}
				}

				_acceptedPoints.Add(new Tuple<int, List<Vector2>>(i, acceptedPoints));
				_declinedPoints.Add(new Tuple<int, List<Vector2>>(i, declinedPoints));
			}
		}

		private int maxGizmoMode = 3;
		private int currentGizmoMode = 0;
		public void IncrementGizmoMode()
		{
			currentGizmoMode++;
			currentGizmoMode %= maxGizmoMode;

#if UNITY_EDITOR
			SceneView.RepaintAll();
#endif

			UpdateText();
		}

		private void UpdateText()
		{
			if (_text == null)
			{
				_text = GetComponentInChildren<TMPro.TextMeshPro>();
			}

			_text.text = "Mode:  " + currentGizmoMode.ToString();
		}


		private TMPro.TextMeshPro _text;
		private void OnDrawGizmos()
		{
			UpdateText();

			switch (currentGizmoMode)
			{
				case 0:
				{
					DrawBoundingBoxes();
					break;
				}

				case 1:
				{
					DrawBoundingBoxes();
					DrawCellPoints();
					break;
				}

				case 2:
				{
					DrawDeclined();
					DrawAccepted();
					break;
				}
			}
		}

		private void DrawBoundingBoxes()
		{
			if (_boundingBoxes != null)
			{
				for (int i = 0; i < _boundingBoxes.Count; ++i)
				{
					var current = _boundingBoxes[i];
					var min = current.Min;
					var max = current.Max;

					var positions = new List<Vector2>()
					{
						new Vector2(min.x, min.y),
						new Vector2(min.x, max.y),
						new Vector2(max.x, max.y),
						new Vector2(max.x, min.y),
					};


					Vector2 displayerCenter = _pointDisplayers[i].transform.position;

					Gizmos.color = Color.blue;
					for (int k = 0; k < positions.Count; ++k)
					{
						Gizmos.DrawSphere(positions[k] + displayerCenter, 0.2f);
						Gizmos.DrawLine(positions[k] + displayerCenter, positions[(k + 1) % positions.Count] + displayerCenter);
					}
				}
			}
		}

		private void DrawCellPoints()
		{
			if (_cellPositions != null)
			{
				for (int i = 0; i < _cellPositions.Count; ++i)
				{
					var currentColl = _cellPositions[i];

					Vector2 displayCenter = _pointDisplayers[i].transform.position;

					Gizmos.color = Color.red;
					for (int k = 0; k < currentColl.Count; ++k)
					{
						Gizmos.DrawWireSphere(currentColl[k] + displayCenter, 0.15f);
					}
				}
			}
		}

		private void DrawDeclined()
		{
			if (_declinedPoints != null)
			{
				Gizmos.color = Color.red;
				for (int i = 0; i < _declinedPoints.Count; ++i)
				{
					var currentColl = _declinedPoints[i];
					for (int k = 0; k < currentColl.Second.Count; ++k)
					{
						Gizmos.DrawWireSphere(new Vector2(currentColl.First * ShapeXJump, 0) + currentColl.Second[k], 0.15f);
					}
				}
			}
		}

		private void DrawAccepted()
		{
			if (_acceptedPoints != null)
			{
				Gizmos.color = Color.green;
				for (int i = 0; i < _acceptedPoints.Count; ++i)
				{
					var currentColl = _acceptedPoints[i];
					for (int k = 0; k < currentColl.Second.Count; ++k)
					{
						Gizmos.DrawWireSphere(new Vector2(currentColl.First * ShapeXJump, 0) + currentColl.Second[k], 0.15f);
					}
				}
			}
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(TestPointIntersectionComponent))]
	public class TestPointIntersectionComponentEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var casted = target as TestPointIntersectionComponent;

			if (GUILayout.Button("Clean RandomShapes"))
			{
				casted.CleanShapes();
				SceneView.RepaintAll();
			}

			GUILayout.Space(5);

			if (GUILayout.Button("Generate RandomShapes"))
			{
				casted.GenerateShapes();
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Calculate Inclusion"))
			{
				casted.CalculateInclusion();
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Increment Gizmo Mode"))
			{
				casted.IncrementGizmoMode();
				SceneView.RepaintAll();
			}

			GUILayout.Space(15);

			base.OnInspectorGUI();
		}
	}
#endif
}
