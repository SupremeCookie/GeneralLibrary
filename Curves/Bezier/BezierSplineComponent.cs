using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Curves
{
	[ExecuteInEditMode]
#if UNITY_EDITOR
	[CanEditMultipleObjects]
#endif
	public class BezierSplineComponent : MonoBehaviour
	{
		private const string PointParent = "PointParent";

		[Range(0.01f, 5f)]
		public float StepSize = 0.01f;
		public bool DrawCurve;
		public bool IsClosed;
		public BezierSplineScriptableObject SplineStorage;
		[Readonly] public BezierSpline Spline;

#pragma warning disable 0649, 0414
		[Range(0.01f, 7.5f)] [HideInInspector] [SerializeField] private float _scaleFactor = 1.0f;
#pragma warning restore 0649, 0414

		private GUIStyle blackText;
		private List<PointObject> _worldPoints;


		private IEnumerator Start()
		{
			while (Spline == null)
			{
				yield return null;
			}

			var existingPoints = Spline.GetPoints();
			if (existingPoints.Count < 2)
			{
				existingPoints.AddRange(new List<BezierSplineChainPoint>
				{
					new BezierSplineChainPoint { PointData = new BezierSplinePointData { LocalPosition = new Vector2(0, 0), },},
					new BezierSplineChainPoint { PointData = new BezierSplinePointData { LocalPosition = new Vector2(1, 0), },},
				});

				Spline.Points = existingPoints;
			}
		}

		public void TryInitPointObjects(bool bypassCountCheck = false)
		{
			var points = Spline.GetPoints();
			if (points.Count == 0)
			{
				return;
			}



			if (_worldPoints != null)
			{
				for (int i = _worldPoints.Count - 1; i >= 0; --i)
				{
					if (_worldPoints[i] == null || _worldPoints[i].Point == null)
					{
						_worldPoints.RemoveAt(i);
					}
				}
			}
			else
			{
				_worldPoints = new List<PointObject>();
			}



			var totalCountOfPoints = points.Count;
			if (!bypassCountCheck)
			{
				if (totalCountOfPoints == (_worldPoints?.Count ?? 0))
				{
					return;
				}
			}



			var pointParent = transform.Find(PointParent)?.gameObject ?? null;
			if (pointParent == null)
			{
				pointParent = new GameObject(PointParent);
				pointParent.transform.SetParent(transform, false);
			}



			var controlPointComponents = pointParent.GetComponentsInChildren<PointComponent>();
			if (controlPointComponents.IsNullOrEmpty() || controlPointComponents.Length < totalCountOfPoints)
			{
				int totalCountNeeded = totalCountOfPoints - (controlPointComponents?.Length ?? 0);
				InstantiatePoints(pointParent, totalCountNeeded);
				controlPointComponents = pointParent.GetComponentsInChildren<PointComponent>();
			}


			_worldPoints.Clear();
			for (int i = 0; i < controlPointComponents.Length; ++i)
			{
				_worldPoints.Add(new PointObject() { Point = controlPointComponents[i], });
			}



			for (int i = 0; i < totalCountOfPoints; ++i)
			{
				var pointData = points.Count > i ? points[i] : null;
				if (pointData == null)
				{
					_worldPoints[i].Point.gameObject.SetActive(false);
					continue;
				}

				if (_worldPoints[i].Point.RecalculateDistancesCallback == null)
				{
					_worldPoints[i].Point.RecalculateDistancesCallback = () =>
					{
						GetDataFromWorldPointsBack();
						Spline.RecalculateDistance();
					};
				}

				_worldPoints[i].Point.gameObject.SetActive(true);
				_worldPoints[i].Point.gameObject.transform.localPosition = pointData.PointData.LocalPosition;
				_worldPoints[i].Point.PointData = pointData.PointData;
			}
		}

		private void InstantiatePoints(GameObject pointParent, int countNeeded)
		{
			for (int i = 0; i < countNeeded; ++i)
			{
				var newGO = new GameObject("Point");
				newGO.transform.SetParent(pointParent.transform, false);

				var point = newGO.AddComponent<PointComponent>();
				_worldPoints.Add(new PointObject { Point = point, });
			}
		}


		public void RemoveWorldObject(GUID id)
		{
			for (int i = 0; i < _worldPoints.Count; ++i)
			{
				var currentPoint = _worldPoints[i];
				if (currentPoint.Point.PointData.ID == id)
				{
					if (!Application.isPlaying)
					{
						DestroyImmediate(currentPoint.Point.gameObject);
					}
					else
					{
						Destroy(currentPoint.Point.gameObject);
					}

					_worldPoints.RemoveAt(i);
					break;
				}
			}
		}


		private void GetDataFromWorldPointsBack()
		{
			if (_worldPoints != null)
			{
				var points = Spline.GetPoints();
				for (int i = 0; i < _worldPoints.Count; ++i)
				{
					points[i].PointData.LocalPosition = _worldPoints[i].Point.PointData.LocalPosition;
					points[i].PointData.ControlPoints = _worldPoints[i].Point.PointData.ControlPoints.Copy();
				}

				Spline.Points = points;
			}
		}


		private void OnValidate()
		{
			if (Spline != null)
			{
				if (Spline.IsClosed != IsClosed)
				{
					Spline.IsClosed = IsClosed;
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (Spline == null)
			{
				return;
			}

			var points = Spline.GetPoints();
			if (points.IsNullOrEmpty())
			{
				return;
			}

			var pointParent = transform.Find(PointParent)?.gameObject ?? null;
			if (pointParent == null)
			{
				return;
			}

			Vector2 pointParentPos = pointParent.transform.localPosition;

			TryInitPointObjects();


			Gizmos.color = EditorConstants.BetweenPointsRay;
			for (int i = 1; i < points.Count; ++i)
			{
				var currPointData = points[i].PointData;
				var prevPointData = points[i - 1].PointData;
				Gizmos.DrawRay(pointParentPos + currPointData.LocalPosition + (Vector2)transform.position, prevPointData.LocalPosition - currPointData.LocalPosition);
			}


#if UNITY_EDITOR
			if (blackText != null)
			{
				for (int i = 0; i < points.Count; ++i)
				{
					UnityEditor.Handles.color = EditorConstants.Text;
					UnityEditor.Handles.Label(pointParentPos + points[i].PointData.LocalPosition + (Vector2)transform.position + new Vector2(0.05f, 0.75f), i.ToString(), blackText);
				}
			}
			else
			{
				blackText = new GUIStyle();
				blackText.normal.textColor = Color.black;
			}
#endif


			if (DrawCurve)
			{
				//string distancesString = "";
				Gizmos.color = EditorConstants.Curve;
				var splinePoints = Spline.GetSpline(StepSize);

				for (int i = 0; i < splinePoints.Length; ++i)
				{
					//float distanceForString = 0f;
					//if (i > 0)
					//{
					//	distanceForString = (splinePoints[i] - splinePoints[i - 1]).magnitude;
					//}

					//distancesString += $"({i} - {distanceForString})";
					//if (i < splinePoints.Length - 1 && i > 0)
					//{
					//	distancesString += " || ";
					//}



					Gizmos.DrawWireSphere(pointParentPos + splinePoints[i].WorldPos + (Vector2)transform.position, .125f);

					if (i > 0)
					{
						Gizmos.DrawRay(pointParentPos + splinePoints[i].WorldPos + (Vector2)transform.position, splinePoints[i - 1].WorldPos - splinePoints[i].WorldPos);
					}
				}

				//Debug.Log("DistancesString: " + distancesString);
			}
		}
	}


#if UNITY_EDITOR
	[CustomEditor(typeof(BezierSplineComponent))]
	public class BezierSplineComponentEditor : Editor
	{
		private SerializedProperty scaleFactorProp;

		private void OnEnable()
		{
			scaleFactorProp = serializedObject.FindProperty("_scaleFactor");
			if (scaleFactorProp.floatValue.IsCloseTo(0.0f, 0.00001f))
			{
				scaleFactorProp.floatValue = 1.0f;
				serializedObject.ApplyModifiedProperties();
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			var casted = target as BezierSplineComponent;
			var currentPoints = casted.Spline.GetPoints();

			if (GUILayout.Button("Add Point"))
			{
				currentPoints.Add(new BezierSplineChainPoint());
				casted.Spline.Points = currentPoints;
				casted.TryInitPointObjects();
				UnityEditor.SceneView.RepaintAll();
			}


			if (GUILayout.Button("Remove Last Point"))
			{
				Undo.RecordObject(target, "-Undo- Remove Last Point");
				int indexToRemove = currentPoints.Count - 1;
				casted.RemoveWorldObject(currentPoints[indexToRemove].PointData.ID);

				currentPoints.RemoveAt(currentPoints.Count - 1);
				casted.Spline.Points = currentPoints;
				UnityEditor.SceneView.RepaintAll();
			}


			if (GUILayout.Button("Save Spline"))
			{
				casted.SplineStorage.Spline = casted.Spline.Copy();
				UnityEditor.SceneView.RepaintAll();
			}

			if (GUILayout.Button("Load Spline From Scriptable"))
			{
				Undo.RecordObject(casted, "-Undo- Load Spline From Scriptable");            // Note DK: For some reason this won't actually work. But atleast the ctrl z action will be caught.
				casted.Spline.Points = casted.SplineStorage.Spline.GetPoints();
				casted.TryInitPointObjects(bypassCountCheck: true);
				UnityEditor.SceneView.RepaintAll();
			}

			if (GUILayout.Button("Recalculate Distance"))
			{
				casted.Spline.RecalculateDistance();
				UnityEditor.SceneView.RepaintAll();
			}

			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(scaleFactorProp);
			GUILayout.EndHorizontal();


			if (GUILayout.Button("Scale spline"))
			{
				var pointComps = casted.GetComponentsInChildren<PointComponent>();

				Undo.RecordObject(target, "-Undo- Scale Spline");

				casted.Spline.Scale(scaleFactorProp.floatValue);
				EditorUtility.SetDirty(target);


				if (pointComps != null)
				{
					var points = casted.Spline.GetPoints();
					for (int i = 0; i < pointComps.Length; ++i) { pointComps[i]?.InjectData(points[i].PointData); }
				}

				UnityEditor.SceneView.RepaintAll();
			}


			GUILayout.Space(15);

			if (GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
			}

			base.OnInspectorGUI();
		}
	}
#endif
}
