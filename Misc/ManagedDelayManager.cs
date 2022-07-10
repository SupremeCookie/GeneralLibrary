using System.Collections.Generic;

namespace RogueLike
{
	public class ManagedDelayManager
	{
		private Dictionary<string, ManagedDelay> _managedDelays;

		public ManagedDelayManager()
		{
			_managedDelays = new Dictionary<string, ManagedDelay>();
		}


		public void Update(float deltaTime)
		{
			if (_managedDelays != null)
			{
				foreach (var item in _managedDelays)
				{
					item.Value.Update(deltaTime);
				}
			}
		}

		/// <summary>
		/// Checks the stored ManagedDelayManageModels for one with the corresponding name.
		/// If not found, returns false.
		/// </summary>
		public bool IsActive(string delayName)
		{
			bool hasKey = _managedDelays.ContainsKey(delayName);
			if (hasKey)
			{
				var delay = _managedDelays[delayName];

				return delay.IsActive;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Checks the stored ManagedDelays for one with the corresponding name.
		/// If not found, creates ones with the given name and delay.
		/// </summary>
		public void SetDelay(string delayName, float delay)
		{
			bool hasKey = _managedDelays.ContainsKey(delayName);

			if (!hasKey)
			{
				_managedDelays.Add(delayName, new ManagedDelay(delay));
			}
			else
			{
				var delayObj = _managedDelays[delayName];
				delayObj.Delay = delay;
			}
		}

		/// <summary>
		/// Checks the stored ManagedDelays for one with the corresponding name.
		/// If not found, returns -1.
		/// </summary>
		public float GetCurrentDelay(string delayName)
		{
			bool hasKey = _managedDelays.ContainsKey(delayName);

			if (!hasKey)
			{
				return -1;
			}
			else
			{
				return _managedDelays[delayName].Delay;
			}
		}

		public bool DoesDelayExist(string delayName)
		{
			bool hasKey = _managedDelays.ContainsKey(delayName);

			return hasKey;
		}


		public void Reset()
		{
			if (_managedDelays != null)
			{
				_managedDelays.Clear();
			}
			else
			{
				_managedDelays = new Dictionary<string, ManagedDelay>();
			}
		}
	}
}
