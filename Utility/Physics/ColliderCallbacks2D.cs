using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLike
{
	public class ColliderCallbacks2D : MonoBehaviour
	{
		public System.Action onCollisionEnter;
		public System.Action onCollisionExit;

		public System.Action onTriggerEnter;
		public System.Action onTriggerExit;


		private void OnCollisionEnter2D(Collision2D collision)
		{
			onCollisionEnter?.Invoke();
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			onCollisionExit?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			onTriggerEnter?.Invoke();
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			onTriggerExit?.Invoke();
		}
	}
}
