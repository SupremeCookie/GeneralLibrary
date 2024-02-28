using TMPro;
using UnityEditor;
using UnityEngine;

namespace RogueLike
{
	public class LocalizedText : MonoBehaviour
	{
		[SerializeField] private TMP_Text targetText;
		[SerializeField] private LocTermModel locTerm;

		[Space(5)]
		[SerializeField] private bool updateEverySoManyFrames = true;
		[SerializeField] private int everySoManyFrames = 10;
		[SerializeField][Readonly] private int updateCount = 0;

		[Space(10)]
		[SerializeField][Readonly] private bool canUpdate = true;

		private void OnEnable()
		{
			UpdateTextToLoca();
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


			UpdateTextToLoca();
		}

#if UNITY_EDITOR
		private void Reset()
		{
			targetText = GetComponentInChildren<TextMeshProUGUI>();
		}
#endif


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

		public void SetText(string text)
		{
			targetText.text = text;
		}


		public void UpdateManually()
		{
			UpdateTextToLoca();
		}

		private void UpdateTextToLoca()
		{
			string localizedText = GetLocalizedText();
			targetText.text = localizedText;
		}

		private string GetLocalizedText()
		{
			return locTerm.ToString();
		}
	}
}