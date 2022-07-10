using UnityEngine;

public class SimpleAudioManager : MonoBehaviour, IInitialisable, IUpdatable
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] audioClips;

	private int currentAudio;

	public bool IsInitialised { get; set; }

	private void Start()
	{
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
		DrawGUI.Instance.OnDrawGUI += OnDrawGUI;
#endif

		IsInitialised = true;
	}

	public void UpdateComponent(float deltaSeconds)
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
		if (DrawGUI.CreateGroup("Audio", out var group))
		{
			group.DrawLabel($"Current audio clip: ({currentAudio})");

			float currentTime = audioSource.time;
			group.DrawLabel($"Current time: ({currentTime})");
		}
	}
#endif
}
