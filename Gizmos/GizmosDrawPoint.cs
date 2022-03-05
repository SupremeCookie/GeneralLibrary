using UnityEngine;

public enum GizmoType
{
	Cube,
	WireCube,
	Sphere,
	WireSphere,
};

public class GizmosDrawPoint : MonoBehaviour
{
#if UNITY_EDITOR
	public float GizmosSize;
	public Color GizmosColor;
	public GizmoType GizmoType;

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = GizmosColor;

		switch (GizmoType)
		{
			case GizmoType.WireSphere:
				Gizmos.DrawWireSphere(transform.position, GizmosSize);
				break;

			case GizmoType.Sphere:
				Gizmos.DrawSphere(transform.position, GizmosSize);
				break;

			case GizmoType.Cube:
				Gizmos.DrawCube(transform.position, Vector3.one * GizmosSize);
				break;

			case GizmoType.WireCube:
				Gizmos.DrawWireCube(transform.position, Vector3.one * GizmosSize);
				break;
		}
	}
#endif
}
