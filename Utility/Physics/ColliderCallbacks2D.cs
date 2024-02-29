using UnityEngine;

namespace RogueLike
{
	public class ColliderCallbacks2D : MonoBehaviour
	{
		[SerializeField] private LayerMask validLayer;

		public System.Action onCollisionEnter;
		public System.Action onCollisionExit;

		public System.Action onTriggerEnter;
		public System.Action onTriggerExit;


		private void OnCollisionEnter2D(Collision2D collision)
		{
			//Debug.Log($"Collide with layer: {collision.gameObject},  {collision.gameObject.layer},  {LayerMask.LayerToName(collision.gameObject.layer)},    validLayer: {validLayer},  {validLayer.Contains(collision.gameObject.layer)}");

			if (validLayer.Contains(collision.gameObject.layer))
				onCollisionEnter?.Invoke();
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			//Debug.Log($"Collide Exit with layer: {collision.gameObject},  {collision.gameObject.layer},  {LayerMask.LayerToName(collision.gameObject.layer)},    validLayer: {validLayer},  {validLayer.Contains(collision.gameObject.layer)}");

			if (validLayer.Contains(collision.gameObject.layer))
				onCollisionExit?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			//Debug.Log($"Trigger Enter with layer: {collision.gameObject},  {collision.gameObject.layer},  {LayerMask.LayerToName(collision.gameObject.layer)},    validLayer: {validLayer},  {validLayer.Contains(collision.gameObject.layer)}");

			if (validLayer.Contains(collision.gameObject.layer))
				onTriggerEnter?.Invoke();
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			//Debug.Log($"Trigger Exit with layer: {collision.gameObject},  {collision.gameObject.layer},  {LayerMask.LayerToName(collision.gameObject.layer)},    validLayer: {validLayer},  {validLayer.Contains(collision.gameObject.layer)}");

			if (validLayer.Contains(collision.gameObject.layer))
				onTriggerExit?.Invoke();
		}
	}
}
