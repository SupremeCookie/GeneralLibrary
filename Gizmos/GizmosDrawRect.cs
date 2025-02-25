using UnityEngine;

public class GizmosDrawRect : MonoBehaviour
{
	public Vector3 gizmosSize;
	public Color gizmosColor = Color.white;
	[Space(5)]
	public Vector3 offset;
	[Space(10)]
	public float hollowThickness = 0.5f;
	[Header("Hollow XZ")]
	public bool drawHollowXZPlane = false;
	public bool shouldDrawCrosshairXZPlane = false;
	[Header("Hollow XY")]
	public bool drawHollowXYPlane = false;
	public bool shouldDrawCrosshairXYPlane = false;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);

		if (shouldDrawCrosshairXZPlane || shouldDrawCrosshairXYPlane)
			DrawCrosshair();

		Gizmos.color = gizmosColor;

		if (!drawHollowXZPlane && !drawHollowXYPlane)
			DrawOpague();
		else
			DrawHollow();
	}

	private void DrawCrosshair()
	{
		Gizmos.color = new Color(0.9f, 0.9f, 0.9f, 0.1f);
		const float thickness = 0.05f;

		if (shouldDrawCrosshairXZPlane)
		{
			Gizmos.DrawCube(offset, gizmosSize.MultiplyByVec3(new Vector3(1.0f, 0.0f, 0.0f)) + new Vector3(0, 0, thickness));
			Gizmos.DrawCube(offset, gizmosSize.MultiplyByVec3(new Vector3(0.0f, 0.0f, 1.0f)) + new Vector3(thickness, 0, 0));
		}
		else if (shouldDrawCrosshairXYPlane)
		{
			Gizmos.DrawCube(offset, gizmosSize.MultiplyByVec3(new Vector3(1.0f, 0.0f, 0.0f)) + new Vector3(0, thickness, 0));
			Gizmos.DrawCube(offset, gizmosSize.MultiplyByVec3(new Vector3(0.0f, 1.0f, 0.0f)) + new Vector3(thickness, 0, 0));
		}
	}

	private void DrawOpague()
	{
		Gizmos.DrawCube(transform.position + offset, gizmosSize);
	}

	private void DrawHollow()
	{
		Vector2[] corners = new Vector2[]
		{
			new Vector2(-gizmosSize.x * 0.5f, -gizmosSize.z * 0.5f),
			new Vector2(-gizmosSize.x * 0.5f, gizmosSize.z * 0.5f),
			new Vector2(gizmosSize.x * 0.5f, gizmosSize.z * 0.5f),
			new Vector2(gizmosSize.x * 0.5f, -gizmosSize.z * 0.5f),
		};

		if (drawHollowXZPlane)
		{
			Vector2[] midPoints = new Vector2[]
			{
				Utility.Lerp(corners[0], corners[1], 0.5f) + new Vector2(-hollowThickness * 0.5f, 0),
				Utility.Lerp(corners[1], corners[2], 0.5f) + new Vector2(0, hollowThickness * 0.5f),
				Utility.Lerp(corners[2], corners[3], 0.5f) + new Vector2(hollowThickness * 0.5f, 0),
				Utility.Lerp(corners[3], corners[0], 0.5f) + new Vector2(0, -hollowThickness * 0.5f),
			};

			Vector2[] thicknesses = new Vector2[]
			{
				(corners[1] - corners[0]) + new Vector2(hollowThickness, 0),
				(corners[2] - corners[1]) + new Vector2(0, hollowThickness),
				(corners[3] - corners[2]) + new Vector2(hollowThickness, 0),
				(corners[0] - corners[3]) + new Vector2(0, hollowThickness),
			};

			for (int i = 0; i < midPoints.Length; ++i)
			{
				Gizmos.DrawCube(midPoints[i].XYVectorToXZ() + offset, thicknesses[i].XYVectorToXZ());
			}
		}
		else if (drawHollowXYPlane)
		{
			Vector2[] midPoints = new Vector2[]
			{
				Utility.Lerp(corners[0], corners[1], 0.5f) + new Vector2(-hollowThickness * 0.5f, 0),
				Utility.Lerp(corners[1], corners[2], 0.5f) + new Vector2(0, hollowThickness * 0.5f),
				Utility.Lerp(corners[2], corners[3], 0.5f) + new Vector2(hollowThickness * 0.5f, 0),
				Utility.Lerp(corners[3], corners[0], 0.5f) + new Vector2(0, -hollowThickness * 0.5f),
			};

			Vector2[] thicknesses = new Vector2[]
			{
				(corners[1] - corners[0]) + new Vector2(hollowThickness, 0),
				(corners[2] - corners[1]) + new Vector2(0, hollowThickness),
				(corners[3] - corners[2]) + new Vector2(hollowThickness, 0),
				(corners[0] - corners[3]) + new Vector2(0, hollowThickness),
			};

			for (int i = 0; i < midPoints.Length; ++i)
			{
				Gizmos.DrawCube((Vector3)midPoints[i] + offset, thicknesses[i]);
			}
		}
	}
#endif
}
