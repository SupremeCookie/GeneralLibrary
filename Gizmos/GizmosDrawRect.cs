using UnityEngine;

public class GizmosDrawRect : MonoBehaviour
{
#if UNITY_EDITOR
	public Vector3 GizmosSize;
	public Color GizmosColor;

	private void OnDrawGizmos()
	{
		Gizmos.color = GizmosColor;
		Gizmos.DrawCube(transform.position, GizmosSize);
	}
#endif
}
