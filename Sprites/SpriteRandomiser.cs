using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomiser : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[Space(10)]
	[SerializeField] private List<Sprite> randomSprites;

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		RandomiseSprite();
	}

	private void RandomiseSprite()
	{
		if (randomSprites.IsNullOrEmpty())
		{
			Debug.LogWarning($"Trying to randomise sprites, but we can't");
			return;
		}

		spriteRenderer.sprite = randomSprites[Random.Range(0, randomSprites.Count)];
	}
}
