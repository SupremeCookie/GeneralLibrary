using UnityEngine;

namespace RogueLike
{
	public class GizmosDrawMousePoint : GizmosDrawPoint
	{
#if UNITY_EDITOR
		protected override void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			var pos = MainCameraControl.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
			transform.position = pos;

			base.OnDrawGizmos();
		}
#endif
	}
}