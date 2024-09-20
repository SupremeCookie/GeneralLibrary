using UnityEngine;

public class GizmosDrawCollider : MonoBehaviour
{
	[SerializeField] private Color gizmosColor = Color.white;

#if UNITY_EDITOR
	private Collider gizmoCollider;
	private MeshCollider meshCollider;

	private void Start() { }

	private void OnDrawGizmos()
	{
		if (!this.enabled)
		{
			return;
		}

		if (gizmoCollider == null)
		{
			gizmoCollider = GetComponentInChildren<Collider>();
			return;
		}

		Matrix4x4 currentMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		if (gizmoCollider is MeshCollider)
		{
			if (meshCollider == null)
			{
				meshCollider = gizmoCollider as MeshCollider;
			}
			else
			{
				Gizmos.color = gizmosColor;
				Gizmos.DrawMesh(meshCollider.sharedMesh);
			}
		}

		Gizmos.matrix = currentMatrix;
	}
#endif
}
