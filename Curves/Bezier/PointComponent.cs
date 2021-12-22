using UnityEngine;

namespace Curves
{
	public class PointComponent : MonoBehaviour
	{
		public BezierSplinePointData PointData;
		public System.Action RecalculateDistancesCallback;

		private ControlPointComponent[] _controlPoints;

		private void Start() { }

		public void UpdateData()
		{
			var oldTransform = transform.localPosition;
			var oldPos = PointData.LocalPosition;

			bool dataGotChanged = !PointData.LocalPosition.IsCloseTo(transform.localPosition);
			PointData.LocalPosition = transform.localPosition;

			if (dataGotChanged)
			{
				Debug.Log($"Update Data! Data got changed!:   trans:({oldTransform}),   oldpos:({oldPos})");
				RecalculateDistancesCallback();
			}
		}

		public void InjectData(BezierSplinePointData pointData)
		{
			PointData = pointData;
			transform.localPosition = pointData.LocalPosition;
			TryInitControlPoints();
		}


		private void TryInitControlPoints()
		{
			if (_controlPoints.IsNullOrEmpty())
			{
				_controlPoints = new ControlPointComponent[2];
				var childComponents = GetComponentsInChildren<ControlPointComponent>();
				if (childComponents == null)
				{
					for (int i = 0; i < 2; ++i)
					{
						var newGO = new GameObject("ControlPoint");
						newGO.transform.SetParent(transform, false);
						var controlPoint = newGO.AddComponent<ControlPointComponent>();
						_controlPoints[i] = controlPoint;
					}
				}
				else
				{
					if (childComponents.Length < 2)
					{
						int countNeeded = 2 - childComponents.Length;

						for (int i = 0; i < countNeeded; ++i)
						{
							var newGO = new GameObject("ControlPoint");
							newGO.transform.SetParent(transform, false);
							var controlPoint = newGO.AddComponent<ControlPointComponent>();
							_controlPoints[i] = controlPoint;
						}
					}
					else
					{
						_controlPoints = childComponents;
					}
				}
			}


			if (PointData.ControlPoints.IsNullOrEmpty())
			{
				if (_controlPoints != null)
				{
					for (int i = 0; i < _controlPoints.Length; ++i)
					{
						PointData.ControlPoints.Add(_controlPoints[i].PointData);
					}
				}
			}


			if (!PointData.ControlPoints.IsNullOrEmpty())
			{
				for (int i = 0; i < PointData.ControlPoints.Count; ++i)
				{
					if (_controlPoints[i].PointData != PointData.ControlPoints[i])
					{
						_controlPoints[i].PointData = PointData.ControlPoints[i];
						_controlPoints[i].Init();
					}

					if (_controlPoints[i].OnUpdateData == null)
					{
						int currentIndex = i;
						_controlPoints[i].OnUpdateData += () => UpdateControlPoints(currentIndex);
					}
				}
			}
		}

		private void UpdateControlPoints(int currentIndex)
		{
			var currentPoint = _controlPoints[currentIndex].PointData;
			var delta = -currentPoint.LocalPosition;

			var otherIndex = currentIndex == 0 ? 1 : 0;
			_controlPoints[otherIndex].PointData.LocalPosition = delta;
			_controlPoints[otherIndex].gameObject.transform.localPosition = delta;

			RecalculateDistancesCallback();
		}



		//private bool _shouldUpdate = true;
		//public bool ShouldUpdate { get; set; } = true;
		public void OnDrawGizmos()
		{
			if (/*!ShouldUpdate || */!gameObject.activeSelf)
			{
				//_shouldUpdate = false;
				return;
			}

			//if (_shouldUpdate)
			{
				TryInitControlPoints();


				for (int i = 0; i < _controlPoints.Length; ++i)
				{
					Gizmos.color = EditorConstants.ControlPointsRay;

					if (_controlPoints[i].PointData != null)
					{
						Gizmos.DrawRay(transform.position, _controlPoints[i].PointData.LocalPosition);
					}

					_controlPoints[i].Index = i;
				}


				Gizmos.color = EditorConstants.Point;
				Gizmos.DrawSphere((Vector2)transform.position, 0.25f);


				UpdateData();
			}
			//else   // Note DK: This differs the activation of the updateData till the next drawgizmos. So the gameobjects have some time to settle.
			//{
			//	_shouldUpdate = true;
			//}
		}
	}
}
