using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GizmosDrawConnectingLine : MonoBehaviour
{
	public float thickness = 2.3f;
	public Color gizmosColor = new Color(1, 1, 1, 1);

	public bool onlyDisplayIfSelected = false;

	[Space(15)]
	public Transform target;

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		if (onlyDisplayIfSelected)
		{
			if (!UnityEditor.Selection.gameObjects.Contains(gameObject))
				return;
		}

		if (target == null)
			return;

		Handles.color = gizmosColor;
		Handles.DrawLine(transform.position, target.position, Handles.lineThickness * thickness);
	}
#endif
}
