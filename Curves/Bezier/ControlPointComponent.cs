using UnityEngine;

namespace Curves
{
	public class ControlPointComponent : MonoBehaviour
	{
		public int Index;
		public BezierControlPointData PointData;
		public System.Action OnUpdateData;

#if UNITY_EDITOR
		private PointComponent _parent;
#endif

		private void Start() { }

		public void Init()
		{
			transform.localPosition = PointData.LocalPosition;
		}



#if UNITY_EDITOR
		public void OnDrawGizmos()
		{
			if (!gameObject.activeSelf)
			{
				return;
			}

			if (PointData == null)
			{
				return;
			}

			if (_parent == null)
			{
				_parent = GetComponentInParent<PointComponent>();
			}

			Gizmos.color = EditorConstants.ControlPoint[Index];
			Gizmos.DrawSphere(transform.position, 0.2f);

			if (UnityEditor.Selection.activeGameObject == gameObject)
			{
				UpdateData();
				transform.localPosition = PointData.LocalPosition;
			}
		}
#endif

		private void UpdateData()
		{
			bool shouldUpdate = !PointData.LocalPosition.IsCloseTo(transform.localPosition);
			if (shouldUpdate)
			{
				PointData.LocalPosition = transform.localPosition;
				OnUpdateData.Invoke();
			}
		}
	}
}
