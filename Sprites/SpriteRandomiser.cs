using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomiser : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[Space(10)]
	[SerializeField] private List<Sprite> randomSprites;
	[Header("Settings")]
	[SerializeField] private bool randomiseManually = false;

	private int _randomSpriteIndex = -1;
	public int randomSpriteIndex => _randomSpriteIndex;

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		if (!randomiseManually)
			RandomiseSprite();
	}

	public void RandomiseSprite()
	{
		if (randomSprites.IsNullOrEmpty())
		{
			Debug.LogWarning($"Trying to randomise sprites, but we can't");
			return;
		}

		_randomSpriteIndex = Random.Range(0, randomSprites.Count);
		spriteRenderer.sprite = randomSprites[_randomSpriteIndex];
	}
}
