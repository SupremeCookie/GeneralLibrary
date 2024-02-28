using UnityEngine;

public static class CameraExtensions
{
	public static Bounds OrthographicWorldBounds(this Camera camera)
	{
		var res = Screen.currentResolution;
		float screenAspect = res.width / (float)res.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds
		(
			/* center */ camera.transform.position,
			/* size   */ new Vector3(cameraHeight * screenAspect, cameraHeight, 0)
		);

		return bounds;
	}
}
