using UnityEngine;

public class GizmosDrawRect : MonoBehaviour
{
	public Vector3 gizmosSize;
	public Color gizmosColor = Color.white;
	[Space(5)]
	public Vector3 offset;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawCube(transform.position + offset, gizmosSize);
	}
#endif
}
