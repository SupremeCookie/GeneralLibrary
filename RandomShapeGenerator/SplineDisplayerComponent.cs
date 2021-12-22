using Curves;
using TMPro;
using UnityEngine;

namespace RandomShapeGenerator
{
	public class SplineDisplayerComponent : MonoBehaviour
	{
		[SerializeField] private TextMeshPro _text;
		[SerializeField] private BezierSplineComponent _spline;

		public BezierSplineComponent Spline { get { return _spline; } }

		private bool _hasInitted;

		public void Start()
		{
			if (!_hasInitted)
			{
				InitComponent();
			}
		}

		private void InitComponent()
		{
			var textGO = new GameObject("Text");
			textGO.transform.SetParent(transform, false);
			_text = textGO.AddComponent<TextMeshPro>();

			var textRect = textGO.GetComponent<RectTransform>();
			textRect.sizeDelta = new Vector2(35, textRect.sizeDelta.y);
			textRect.anchoredPosition = textRect.anchoredPosition - new Vector2(4.5f, 0.0f);
			textRect.localScale = Vector3.one * 0.25f;


			var splineCompGo = new GameObject("Spline");
			splineCompGo.transform.SetParent(transform, false);
			_spline = splineCompGo.AddComponent<BezierSplineComponent>();


			_hasInitted = true;
		}


		public void InjectData(NamedBezierSpline spline)
		{
			if (!_hasInitted)
			{
				InitComponent();
			}


			_text.text = spline.Name;
			_spline.Spline = spline.Spline;
			_spline.SplineStorage = spline.SplineScriptableObject;
			_spline.DrawCurve = true;
			_spline.IsClosed = true;
			_spline.StepSize = 0.35f;
		}
	}
}
