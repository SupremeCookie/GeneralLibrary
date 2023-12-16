using System.Collections;
using System.Collections.Generic;
using RogueLike;
using UnityEngine;

public class GizmosDrawCamera : MonoBehaviour
{
#if UNITY_EDITOR
	private Camera gizmoCamera;

	void Start() { }

	private void OnDrawGizmos()
	{
		if (!this.enabled)
		{
			return;
		}

		if ((MainCameraControl.HasInstance || MainCameraControl.Instance.Camera == null) && gizmoCamera == null)
		{
			TryCachingEditorCamera();
			return;
		}

		if (MainCameraControl.HasInstance)
		{
			gizmoCamera = MainCameraControl.Instance.Camera;
		}

		var bounds = gizmoCamera.OrthographicWorldBounds();
		var points = new Vector2[]
		{
			bounds.min,
			new Vector2(bounds.min.x, bounds.max.y),
			bounds.max,
			new Vector2(bounds.max.x, bounds.min.y),
		};

		Vector2 positionOffset = transform.position + -bounds.center;   // Note DK: Gotta take into account misalligned centers
		Gizmos.color = DebugUtility.GetDebugColor(transform.GetSiblingIndex());
		for (int i = 0; i < points.Length; ++i)
		{
			Gizmos.DrawLine(positionOffset + points[i], positionOffset + points[(i + 1) % points.Length]);
		}
	}

	private void TryCachingEditorCamera()
	{
		gizmoCamera = Camera.main;
	}
#endif
}
