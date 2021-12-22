using UnityEngine;

namespace Curves
{
	[CreateAssetMenu(fileName = "BezierSpline", menuName = "_scriptables/Bezier Spline", order = 10)]
	public class BezierSplineScriptableObject : ScriptableObject
	{
		public BezierSpline Spline;
	}

#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(BezierSplineScriptableObject))]
	public class BezierSplineScriptableObjectEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			ScriptableUtility.DrawSaveButton_AtStart(target);

			base.OnInspectorGUI();
		}
	}
#endif
}
