using UnityEngine;

namespace RogueLike
{
	[System.Serializable]
	public class ManagedDelay
	{
		[SerializeField] [Readonly] private float _delay;

		public bool IsActive { get; private set; }
		public float Delay { get { return _delay; } set { _delay = value; ActivateDelay(); } }

		public ManagedDelay()
		{
			Delay = 0;
		}

		public ManagedDelay(float delay)
		{
			Delay = delay;
		}

		public void Update(float deltaTime)
		{
			if (IsActive)
			{
				Delay -= deltaTime;
				if (Delay <= 0)
				{
					Delay = 0;
					IsActive = false;
				}
			}
		}

		private void ActivateDelay()
		{
			IsActive = _delay > 0;
		}
	}
}
