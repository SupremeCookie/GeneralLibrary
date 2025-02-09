using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] private List<SpriteForAnimation> animatedSprites;
	[SerializeField] private PersistentFloat durationPerSprite;
	[SerializeField] private PersistentFloat delayForNewAnimation;

	private float currentDuration = 0f;
	private float durationCurrentSprite = 0f;
	private int currentSpriteIndex = -1;
	private float currentDelayForNewAnimation = 0f;

	private void OnEnable()
	{
		currentDelayForNewAnimation = Random.Range(0, 2f);
		spriteRenderer.sprite = null;
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

	private void PickNewSprite()
	{
		Debug.Assert(!animatedSprites.IsNullOrEmpty(), $"There are no sprites to pick from and animate");

		currentSpriteIndex++;
		if (currentSpriteIndex >= animatedSprites.Count)
		{
			currentDelayForNewAnimation = delayForNewAnimation.GetValue();
			currentDelayForNewAnimation += Random.Range(0, 2f);
			currentSpriteIndex = -1;
			spriteRenderer.sprite = null;
			return;
		}

		durationCurrentSprite = durationPerSprite.GetValue() * animatedSprites[currentSpriteIndex].durationMultiplier;

		spriteRenderer.sprite = animatedSprites[currentSpriteIndex].sprite;
		spriteRenderer.transform.localScale = animatedSprites[currentSpriteIndex].scale;
		spriteRenderer.color = animatedSprites[currentSpriteIndex].color;
	}
}
