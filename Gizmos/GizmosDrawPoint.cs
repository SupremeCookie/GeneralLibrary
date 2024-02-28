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
	public float gizmosSize;
	public Color gizmosColor;
	public GizmoType gizmoType;

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;

		switch (gizmoType)
		{
			case GizmoType.WireSphere:
				Gizmos.DrawWireSphere(transform.position, gizmosSize);
				break;

			case GizmoType.Sphere:
				Gizmos.DrawSphere(transform.position, gizmosSize);
				break;

			case GizmoType.Cube:
				Gizmos.DrawCube(transform.position, Vector3.one * gizmosSize);
				break;

			case GizmoType.WireCube:
				Gizmos.DrawWireCube(transform.position, Vector3.one * gizmosSize);
				break;
		}
	}
#endif
}
