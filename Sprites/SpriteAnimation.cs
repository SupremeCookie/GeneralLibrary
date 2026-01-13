using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

// TODO DK: Rewrite to push the specific Renderer into its own composition, with the timing logic and sprite storage part of the Base
public class SpriteAnimation : MonoBehaviour
{
	[Header("Base Settings")]
	[FormerlySerializedAs("spriteRenderer")]
	[SerializeField] private Renderer _renderer;

	[Space(10)]
	[SerializeField] private bool shouldHaveInitialAnimationDelay = true;
	[SerializeField] private bool shouldAffectColor = true;
	[Space(5)]
	[SerializeField] private bool shouldRunOnce = false;
	[SerializeField] private PersistentFloat delayForNewAnimation;
	[Space(10)]
	[SerializeField] private List<SpriteForAnimation> animatedSprites;
	[SerializeField] private PersistentFloat durationPerSprite;

	public bool doneWithLastSprite { get; private set; } = false;

	public float baseSpriteDuration => durationPerSprite?.GetValue() ?? 0f;

	private float currentDuration = 0f;
	private float durationCurrentSprite = 0f;
	private int currentSpriteIndex = -1;
	private float currentDelayForNewAnimation = 0f;
	private Vector3 baseScale;

	private SpriteRenderer spriteRenderer => _renderer as SpriteRenderer;


	private void OnEnable()
	{
		if (shouldHaveInitialAnimationDelay)
			currentDelayForNewAnimation = Random.Range(0, 2f);

		SetSprite(animatedSprites?[animatedSprites.Count - 1].sprite ?? null);
		baseScale = transform.localScale;
		doneWithLastSprite = false;
	}

	protected virtual void Reset()
	{
		_renderer = GetComponent<SpriteRenderer>();
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

			if (delayForNewAnimation != null)
			{
				currentDelayForNewAnimation = delayForNewAnimation.GetValue();
				currentDelayForNewAnimation += Random.Range(0, 2f);
			}
			else
			{
				currentDelayForNewAnimation = 0f;
			}

			currentSpriteIndex = -1;

			return;
		}

		doneWithLastSprite = false;

		Debug.Assert(durationPerSprite != null, $"No {typeof(PersistentFloat)} setup to determine duration per sprite with, please apply.");
		durationCurrentSprite = durationPerSprite.GetValue() * animatedSprites[currentSpriteIndex].durationMultiplier;

		SetSprite(animatedSprites[currentSpriteIndex].sprite);
		SetScale(animatedSprites[currentSpriteIndex].scale.MultiplyByVec3(baseScale));

		if (shouldAffectColor)
			SetColor(animatedSprites[currentSpriteIndex].color);
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


	protected virtual void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	protected virtual void SetScale(Vector3 scale)
	{
		spriteRenderer.transform.localScale = scale;
	}

	protected virtual void SetColor(Color color)
	{
		spriteRenderer.color = color;
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