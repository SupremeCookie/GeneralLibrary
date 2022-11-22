﻿using TMPro;
using UnityEngine;

namespace RogueLike
{
	// Note DK: This is more or less fully related to a configurable game component. For in-code we now use LocTerm
	public class LocalizedText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI targetText;
		[SerializeField] private LocTermModel locTerm;

		[Space(5)]
		[SerializeField] private bool hasInputKeybindingsInside;
		[SerializeField] private KeybindingLocTermModel gamepadKeyBinding;
		[SerializeField] private KeybindingLocTermModel keyboardBinding;

		[Space(5)]
		[SerializeField] private bool updateEverySoManyFrames = true;
		[SerializeField] private int everySoManyFrames = 10;
		[SerializeField] [Readonly] private int updateCount = 0;

		[Space(10)]
		[SerializeField] [Readonly] private ControlType lastKnownInputDevice;

		private void Start()
		{
			updateCount = everySoManyFrames;
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
			}
		}

#if UNITY_EDITOR
		private void Reset()
		{
			targetText = GetComponentInChildren<TextMeshProUGUI>();
		}
#endif

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