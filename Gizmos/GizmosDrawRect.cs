﻿using UnityEngine;

public class GizmosDrawRect : MonoBehaviour
{
	public Vector3 gizmosSize;
	public Color gizmosColor;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawCube(transform.position, gizmosSize);
	}
#endif
}
