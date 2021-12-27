using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
	/// <summary>
	/// This is a light-weight, keyboard only, simple input, mainly for debugging, or initial steps of a project.
	/// All of this will likely be superseded by [rewired]
	/// </summary>
	public static class Input
	{
		private static List<bool> _prevFrameValues;
		private static List<bool> _currFrameValues;


		public static DateTime LastActiveTime { get; private set; }


		static Input()
		{
			_prevFrameValues = new List<bool>((int)GeneralActions.Count + (int)MouseActions.Count);
			_currFrameValues = new List<bool>((int)GeneralActions.Count + (int)MouseActions.Count);
		}


		public static void Update(float deltaSeconds)
		{
			LastActiveTime = DateTime.UtcNow;
			// PollInput
		}



		public static bool IsActionGoingDown(GeneralActions action)
		{
			bool notPrevFrame = !_prevFrameValues[(int)action];
			bool isCurrFrame = _currFrameValues[(int)action];
			return notPrevFrame && isCurrFrame;
		}

		public static bool IsActionHeldDown(GeneralActions action)
		{
			bool isCurrFrame = _currFrameValues[(int)action];
			return isCurrFrame;
		}

		public static bool IsActionGoingUp(GeneralActions action)
		{
			bool isPrevFrame = _prevFrameValues[(int)action];
			bool notCurrFrame = !_currFrameValues[(int)action];
			return isPrevFrame && notCurrFrame;
		}

		public static bool IsActionTriggered(GeneralActions action)
		{
			return IsActionGoingUp(action);
		}




		public static bool IsMouseGoingDown(MouseActions action)
		{
			int intAction = (int)action + (int)GeneralActions.Count;

			bool notPrevFrame = !_prevFrameValues[intAction];
			bool isCurrFrame = _currFrameValues[intAction];
			return notPrevFrame && isCurrFrame;
		}

		public static bool IsMouseHeldDown(MouseActions action)
		{
			int intAction = (int)action + (int)GeneralActions.Count;

			bool isCurrFrame = _currFrameValues[intAction];
			return isCurrFrame;
		}

		public static bool IsMouseGoingUp(MouseActions action)
		{
			int intAction = (int)action + (int)GeneralActions.Count;

			bool isPrevFrame = _prevFrameValues[intAction];
			bool notCurrFrame = !_currFrameValues[intAction];
			return isPrevFrame && notCurrFrame;
		}

		public static bool IsMouseActionTriggered(MouseActions action)
		{
			return IsMouseGoingUp(action);
		}



		public static Vector2 GetMousePos()
		{
			return UnityEngine.Input.mousePosition;
		}

		public static float GetMouseScrollDelta()
		{
			return UnityEngine.Input.mouseScrollDelta.y;
		}
	}
}
