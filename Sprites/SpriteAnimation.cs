using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SpriteForAnimation
{
	public Sprite sprite;
	public Vector3 scale = Vector3.one;
	public Color color = Color.white;
	public float durationMultiplier = 1.0f;
}

public class SpriteAnimation : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	[Space(10)]
	[SerializeField] private bool shouldHaveInitialAnimationDelay = true;
	[Space(5)]
	[SerializeField] private bool shouldRunOnce = false;
	[SerializeField] private PersistentFloat delayForNewAnimation;
	[Space(10)]
	[SerializeField] private List<SpriteForAnimation> animatedSprites;
	[SerializeField] private PersistentFloat durationPerSprite;

	public bool doneWithLastSprite { get; private set; } = false;

	public float duration => durationPerSprite?.GetValue() ?? 0f;

	private float currentDuration = 0f;
	private float durationCurrentSprite = 0f;
	private int currentSpriteIndex = -1;
	private float currentDelayForNewAnimation = 0f;
	private Vector3 baseScale;

	private void OnEnable()
	{
		if (shouldHaveInitialAnimationDelay)
			currentDelayForNewAnimation = Random.Range(0, 2f);

		spriteRenderer.sprite = animatedSprites?[animatedSprites.Count - 1].sprite ?? null;
		baseScale = transform.localScale;
	}

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (currentDelayForNewAnimation > 0f)
		{
			currentDelayForNewAnimation -= Time.deltaTime;
			return;
		}

		currentDuration += Time.deltaTime;

		if (currentDuration > durationCurrentSprite || currentSpriteIndex < 0)
		{
			PickNewSprite();
			currentDuration = 0f;
		}
	}


	public List<SpriteForAnimation> GetSprites()
	{
		return new List<SpriteForAnimation>(animatedSprites);
	}


	private void PickNewSprite()
	{
		Debug.Assert(!animatedSprites.IsNullOrEmpty(), $"There are no sprites to pick from and animate");

		currentSpriteIndex++;
		if (currentSpriteIndex >= animatedSprites.Count)
		{
			doneWithLastSprite = true;

			if (shouldRunOnce)
			{
				currentSpriteIndex = animatedSprites.Count;     // To combat the integer increase
				return;
			}

			currentDelayForNewAnimation = delayForNewAnimation.GetValue();
			currentDelayForNewAnimation += Random.Range(0, 2f);
			currentSpriteIndex = -1;

			return;
		}

		doneWithLastSprite = false;

		durationCurrentSprite = durationPerSprite.GetValue() * animatedSprites[currentSpriteIndex].durationMultiplier;

		spriteRenderer.sprite = animatedSprites[currentSpriteIndex].sprite;
		spriteRenderer.transform.localScale = animatedSprites[currentSpriteIndex].scale.MultiplyByVec3(baseScale);
		spriteRenderer.color = animatedSprites[currentSpriteIndex].color;
	}

	public void SetAnimatedSpritesToDefault()
	{
		if (animatedSprites.IsNullOrEmpty())
			return;

		for (int i = 0; i < animatedSprites.Count; ++i)
		{
			animatedSprites[i].durationMultiplier = 1.0f;
			animatedSprites[i].color = Color.white;
			animatedSprites[i].scale = Vector3.one;
		}

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(SpriteAnimation))]
public class SpriteAnimationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		if (GUILayout.Button($"Reset Animated Sprites To Default"))
		{
			(target as SpriteAnimation).SetAnimatedSpritesToDefault();
		}

		if (GUILayout.Button($"Set dirty"))
		{
			UnityEditor.EditorUtility.SetDirty(target);
		}
	}
}
#endif