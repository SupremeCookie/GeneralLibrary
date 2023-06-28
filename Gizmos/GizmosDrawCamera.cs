using System.Collections;
using System.Collections.Generic;
using RogueLike;
using UnityEngine;

public class GizmosDrawCamera : MonoBehaviour
{
#if UNITY_EDITOR
	void Start() { }

	private void OnDrawGizmos()
	{
		if (!this.enabled)
		{
			return;
		}

		if (MainCameraControl.Instance == null || MainCameraControl.Instance.Camera == null)
		{
			return;
		}

		var bounds = MainCameraControl.Instance.Camera.OrthographicWorldBounds();
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
#endif
}
