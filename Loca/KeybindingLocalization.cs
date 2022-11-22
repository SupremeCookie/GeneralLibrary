using TMPro;
using UnityEngine;

namespace RogueLike
{
	public class KeybindingLocalization : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI targetText;
		[SerializeField] private KeybindingLocTermModel gamepadKeyBinding;
		[SerializeField] private KeybindingLocTermModel keyboardBinding;

		[SerializeField] [Space(5)] private bool updateEverySoManyFrames = true;
		[SerializeField] private int everySoManyFrames = 10;
		[SerializeField] [Readonly] private int updateCount = 0;

		[SerializeField] [Readonly] [Space(10)] private ControlType lastKnownInputDevice;

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

#if UNITY_EDITOR
		private void Reset()
		{
			targetText = GetComponentInChildren<TextMeshProUGUI>();
		}
#endif

		private void UpdateKeybinding()
		{
			switch (lastKnownInputDevice)
			{
				case ControlType.GamePad:
				{
					targetText.text = gamepadKeyBinding.ToString();
					break;
				}

				case ControlType.None:
				case ControlType.KeyBoardMouse:
				{
					targetText.text = keyboardBinding.ToString();
					break;
				}

				default:
				{
					Debug.Assert(false, $"No keybinding defined for ({lastKnownInputDevice})");
					break;
				}
			}
		}
	}
}
