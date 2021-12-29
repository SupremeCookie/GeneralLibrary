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

		private static Vector2 _prevMousePos;
		private static Vector2 _currMousePos;


		public static DateTime LastActiveTime { get; private set; }


		static Input()
		{
			_prevFrameValues = new List<bool>((int)GeneralActions.Count + (int)MouseActions.Count).Populate();
			_currFrameValues = new List<bool>((int)GeneralActions.Count + (int)MouseActions.Count).Populate();
		}


		public static void Update(float deltaSeconds)
		{
			LastActiveTime = DateTime.UtcNow;


			for (int i = 0; i < _currFrameValues.Count; ++i)
			{
				_prevFrameValues[i] = _currFrameValues[i];
			}

			_prevMousePos = _currMousePos;


			_currFrameValues[(int)GeneralActions.Up] = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow);
			_currFrameValues[(int)GeneralActions.Down] = UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow);
			_currFrameValues[(int)GeneralActions.Left] = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow);
			_currFrameValues[(int)GeneralActions.Right] = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow);
			_currFrameValues[(int)GeneralActions.RotateLeft] = UnityEngine.Input.GetKey(KeyCode.Q);
			_currFrameValues[(int)GeneralActions.RotateRight] = UnityEngine.Input.GetKey(KeyCode.E);
			_currFrameValues[(int)GeneralActions.Rotate] = UnityEngine.Input.GetKey(KeyCode.R);
			_currFrameValues[(int)GeneralActions.Alternative] = UnityEngine.Input.GetKey(KeyCode.LeftShift);


			int generalActionsOffset = (int)GeneralActions.Count;
			_currFrameValues[generalActionsOffset + (int)MouseActions.Click_Left] = UnityEngine.Input.GetMouseButton(0);
			_currFrameValues[generalActionsOffset + (int)MouseActions.Click_Right] = UnityEngine.Input.GetMouseButton(1);
			_currFrameValues[generalActionsOffset + (int)MouseActions.Click_Middle] = UnityEngine.Input.GetMouseButton(2);


			_currMousePos = UnityEngine.Input.mousePosition;
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

		public static Vector2 GetMousePosDelta()
		{
			return _currMousePos - _prevMousePos;
		}

		public static float GetMouseScrollDelta()
		{
			return UnityEngine.Input.mouseScrollDelta.y;
		}
	}
}
