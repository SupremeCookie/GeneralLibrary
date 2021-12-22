using UnityEngine;

public class GizmosGridDrawer : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		Gizmos.DrawLine(transform.position + (Vector3.left * 100), transform.position + (Vector3.right * 100));
		Gizmos.DrawLine(transform.position + (Vector3.up * 100), transform.position + (Vector3.down * 100));
	}
}
