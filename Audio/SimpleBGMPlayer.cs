using ImGuiNET;
using UnityEngine;

public class SimpleBGMPlayer : MonoBehaviour, IInitialisable, IUpdatable
{
#if DEBUG_MENU
	public const string DrawGuiLabel = "SimpleBGMPlayer";
#endif

	[SerializeField] private bool shouldStartAutomatically = true;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] audioClips;

	private int currentAudio;

	public bool IsInitialised { get; private set; }

	private void Start()
	{
		if (!shouldStartAutomatically)
		{
			return;
		}

		Init();
	}

	public void Init()
	{
		if (IsInitialised)
		{
			return;
		}

		currentAudio = 0;

		StartAudio(currentAudio);

#if DEBUG_MENU
		DKatGamesImgui.Subscribe(DrawGuiLabel, OnDrawGUI);
#endif

		IsInitialised = true;
	}

	public void UpdateComponent(float deltaSeconds, float unscaledDeltaSeconds)
	{
		if (!IsInitialised)
		{
			return;
		}

		if (!audioSource.isPlaying)
		{
			currentAudio++;
			currentAudio %= audioClips.Length;

			StartAudio(currentAudio);
		}
	}

	private void StartAudio(int clip)
	{
		audioSource.clip = audioClips[clip];
		audioSource.Play();
	}

#if DEBUG_MENU
	private void OnDrawGUI()
	{
		ImGui.Text($"Current audio clip: ({currentAudio})");

		float currentTime = audioSource.time;
		ImGui.Text($"Current time: ({currentTime})");
	}
#endif
}
