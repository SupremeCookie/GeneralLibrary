using TMPro;
using UnityEditor;
using UnityEngine;

namespace RogueLike
{
	// Note DK: This is more or less fully related to a configurable game component. For in-code we now use LocTerm
	public class LocalizedText : MonoBehaviour
	{
		[SerializeField] private TMP_Text targetText;
		[SerializeField] private LocTermModel locTerm;

		[Space(5)]
		[SerializeField] private bool hasInputKeybindingsInside;
		[SerializeField] private KeybindingLocTermModel gamepadKeyBinding;
		[SerializeField] private KeybindingLocTermModel keyboardBinding;

		[Space(5)]
		[SerializeField] private bool updateEverySoManyFrames = true;
		[SerializeField] private int everySoManyFrames = 10;
		[SerializeField][Readonly] private int updateCount = 0;

		[Space(10)]
		[SerializeField][Readonly] private ControlType lastKnownInputDevice;

		[Space(10)]
		[SerializeField][Readonly] private bool canUpdate = true;

		private void OnEnable()
		{
			TryUpdateInputDeviceAndBinding();
		}

		private void Update()
		{
			if (!canUpdate)
			{
				return;
			}

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

		public void TurnOffUpdate()
		{
			canUpdate = false;
		}

		public void TurnOnUpdate()
		{
			canUpdate = true;
			updateCount = everySoManyFrames;
		}

		public void EmptyText()
		{
			targetText.text = "";
		}

		private void TryUpdateInputDeviceAndBinding()
		{
			bool updateRegularKeybinding = !hasInputKeybindingsInside;
			if (updateRegularKeybinding)
			{
				UpdateKeybindingNormally();
			}
			else
			{
				bool inputControlExists = InputControl.HasInstance;
				if (inputControlExists)
				{
					var inputDevice = InputControl.Instance.GetLastActiveInputDevice();
					if (inputDevice != lastKnownInputDevice)
					{
						lastKnownInputDevice = inputDevice;
						UpdateKeybindingWithInput();
					}
				}
				else
				{
					UpdateKeybindingNormally();
				}
			}
		}

#if UNITY_EDITOR
		private void Reset()
		{
			targetText = GetComponentInChildren<TextMeshProUGUI>();
		}
#endif

		public void DebugUpdate()
		{
			TryUpdateInputDeviceAndBinding();
		}

		private void UpdateKeybindingNormally()
		{
			string localizedText = GetLocalizedText();
			targetText.text = localizedText;
		}

		private void UpdateKeybindingWithInput()
		{
			string localizedText = GetLocalizedText();
			string formatContent = "";

			switch (lastKnownInputDevice)
			{
				case ControlType.GamePad:
				{
					formatContent = gamepadKeyBinding.ToString();
					break;
				}

				case ControlType.None:
				case ControlType.KeyBoardMouse:
				{
					formatContent = keyboardBinding.ToString();
					break;
				}

				default:
				{
					Debug.Assert(false, $"No keybinding defined for ({lastKnownInputDevice})");
					break;
				}
			}

			localizedText = string.Format(localizedText, formatContent);
			targetText.text = localizedText;
		}

		private string GetLocalizedText()
		{
			return locTerm.ToString();
		}
	}
}