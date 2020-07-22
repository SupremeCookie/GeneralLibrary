using UnityEngine;

public class CoroutinesHelper
{
	public class WaitForSecondsRT : CustomYieldInstruction
	{
		private float _time;

		public override bool keepWaiting
		{
			get { return (_time -= Time.unscaledDeltaTime) > 0; }
		}

		public WaitForSecondsRT(float aWaitTime)
		{
			_time = aWaitTime;
		}

		public WaitForSecondsRT NewTime(float aTime)
		{
			_time = aTime;
			return this;
		}
	}
}