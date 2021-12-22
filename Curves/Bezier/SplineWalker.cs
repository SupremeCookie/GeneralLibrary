using System.Collections;
using UnityEngine;

namespace Curves
{
	public class SplineWalker : MonoBehaviour
	{
		public float DecelerationSpeed;
		public bool UseDecelerationCalculation;

		public BezierSplineComponent AccelCurve;
		private float time = 0f;

		private Vector3 startPos;

		public IEnumerator Start()
		{
			startPos = transform.position;
			var newgo = new GameObject("StartPoint");
			newgo.transform.position = startPos;

#if UNITY_EDITOR
			var drawPoint = newgo.AddComponent<GizmosDrawPoint>();
			drawPoint.GizmosSize = 0.2f;
			drawPoint.GizmoType = GizmoType.Cube;
			drawPoint.GizmosColor = Color.cyan;
#endif

			currentPoint = startPos;


			yield return new WaitForSeconds(2);

			shouldUpdate = true;
		}

		private Vector3 currentPoint;
		private bool madeEndPoint = false;
		private GameObject endPos;
		private bool shouldUpdate = false;
		private bool shouldStoreAdditionIndicator = false;
		private GameObject additionIndicator;

		public void Update()
		{
			if (!shouldUpdate)
			{
				return;
			}

			if (time >= 1.0f)
			{
				if (!madeEndPoint)
				{
					var newgo = new GameObject("EndPoint");
					newgo.transform.position = transform.position;

#if UNITY_EDITOR
					var drawPoint = newgo.AddComponent<GizmosDrawPoint>();
					drawPoint.GizmosSize = 0.2f;
					drawPoint.GizmoType = GizmoType.Cube;
					drawPoint.GizmosColor = Color.cyan;
#endif

					madeEndPoint = true;
					endPos = newgo;
				}
				else
				{
					endPos.transform.position = transform.position;
				}

				shouldStoreAdditionIndicator = true;

				currentPoint = startPos;
				transform.position = startPos;
				time = 0f;
				return;
			}

			float speedModifier = 0.1f;
			if (!UseDecelerationCalculation)
			{
				float addition = AccelCurve.Spline.Evaluate(time * AccelCurve.Spline.TotalDistance).y * speedModifier;
				currentPoint = new Vector3(currentPoint.x + addition, startPos.y, startPos.z);
				transform.position = currentPoint;
			}
			else
			{
				float addition = 1.0f * speedModifier;

				if (time >= 0.5f)
				{
					var initialVelocity = 1.0f;
					var finalVelocity = 0.0f;
					addition = (finalVelocity - (initialVelocity * initialVelocity)) / (DecelerationSpeed * 2.0f);                    // Normally you'd go final velocity squared - initial velocity squared separated by double acceleration
					var finalDistance = addition;

					addition *= speedModifier;

					addition *= initialVelocity - (((time - 0.5f) * 2f) * initialVelocity);

					addition = Mathf.Abs(addition);


					if (shouldStoreAdditionIndicator)
					{
						shouldStoreAdditionIndicator = false;

						if (additionIndicator == null)
						{
							var newgo = new GameObject("EndPoint");

#if UNITY_EDITOR
							var drawPoint = newgo.AddComponent<GizmosDrawPoint>();
							drawPoint.GizmosSize = 0.2f;
							drawPoint.GizmoType = GizmoType.Cube;
							drawPoint.GizmosColor = Color.cyan;
#endif

							madeEndPoint = true;
							additionIndicator = newgo;
						}

						additionIndicator.transform.position = transform.position + new Vector3(Mathf.Abs(finalDistance), -1.0f, 0);
					}
				}

				currentPoint = new Vector3(currentPoint.x + addition, startPos.y, startPos.z);
				transform.position = currentPoint;
			}

			time += Time.deltaTime;
			time = Mathf.Min(time, 1.0f);
		}
	}
}
