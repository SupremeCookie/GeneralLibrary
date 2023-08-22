using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLike
{
	public class KeybindingLocImage : MonoBehaviour
	{
		[SerializeField][Space(5)] private bool updateEverySoManyFrames = true;
		[SerializeField] private int everySoManyFrames = 10;
		[SerializeField][Readonly] private int updateCount = 0;

		[SerializeField][Readonly][Space(10)] private ControlType lastKnownInputDevice;

		private void OnEnable()
		{
			TryUpdateInputDeviceAndBinding();
		}

		private void Update()
		{
			if (updateEverySoManyFrames)
			{
				if (updateCount < everySoManyFrames)
				{
					++updateCount;
					return;
				}

				updateCount = 0;
			}

			TryUpdateInputDeviceAndBinding();
		}

		private void TryUpdateInputDeviceAndBinding()
		{
			bool inputControlExists = InputControl.HasInstance;
			if (inputControlExists)
			{
				var inputDevice = InputControl.Instance.GetLastActiveInputDevice();
				if (inputDevice != lastKnownInputDevice)
				{
					lastKnownInputDevice = inputDevice;
					UpdateKeybinding();
				}
			}
		}

		private void UpdateKeybinding()
		{
			switch (lastKnownInputDevice)
			{
				case ControlType.GamePad:
				{
					UpdateKeybinding_GamePad();
					break;
				}

				case ControlType.None:
				case ControlType.KeyBoardMouse:
				{
					UpdateKeybinding_KBM();
					break;
				}

				default:
				{
					Debug.Assert(false, $"No keybinding defined for ({lastKnownInputDevice})");
					break;
				}
			}
		}

		protected virtual void UpdateKeybinding_GamePad()
		{

		}

		protected virtual void UpdateKeybinding_KBM()
		{

		}
	}
}
