using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SpriteForAnimation_Stepper
{
	public Sprite sprite;
	public Vector3 scale = Vector3.one;
	public Color color = Color.white;
}

// TODO DK: Rewrite to push the specific Renderer into its own composition, with the timing logic and sprite storage part of the Base
public class SpriteAnimation_Stepper : MonoBehaviour
{
	[Header("Base Settings")]
	[SerializeField] private Renderer _renderer;

	[Space(10)]
	[SerializeField] private bool shouldAffectColor = true;
	[SerializeField] private bool shouldRunOnce = false;
	[SerializeField] private bool shouldUseGlobalStepper = false;
	[Space(10)]
	[SerializeField] private PersistentFloat animationSpeed;
	[SerializeField] private List<SpriteForAnimation_Stepper> animatedSprites;

	public bool doneWithLastSprite { get; private set; } = false;

	private int currentSpriteIndex = -1;
	private Vector3 baseScale;
	private Stepper stepper;

	private SpriteRenderer spriteRenderer => _renderer as SpriteRenderer;


	private void OnEnable()
	{
		SetSprite(animatedSprites?[animatedSprites.Count - 1].sprite ?? null);
		baseScale = transform.localScale;
		doneWithLastSprite = false;

		if (shouldUseGlobalStepper)
			stepper = GlobalStepper.GetStepper(animatedSprites.Count);
		else
			stepper = new Stepper(animatedSprites.Count, animationSpeed != null ? animationSpeed.GetValue() : Stepper.DEFAULT_SPEED);
	}

	protected virtual void Reset()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!shouldUseGlobalStepper)
		{
			stepper.Update(Time.deltaTime);
		}

		if (stepper.hasChangedThisFrame || currentSpriteIndex < 0)
		{
			PickNewSprite();
		}
	}


	public List<SpriteForAnimation_Stepper> GetSprites()
	{
		return new List<SpriteForAnimation_Stepper>(animatedSprites);
	}


	private void PickNewSprite()
	{
		Debug.Assert(!animatedSprites.IsNullOrEmpty(), $"There are no sprites to pick from and animate");

		currentSpriteIndex = stepper.currentStep;
		if (currentSpriteIndex >= animatedSprites.Count)
		{
			doneWithLastSprite = true;

			if (shouldRunOnce)
			{
				currentSpriteIndex = animatedSprites.Count;     // To combat the integer increase
				return;
			}

			currentSpriteIndex = -1;

			return;
		}

		doneWithLastSprite = false;

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
[CustomEditor(typeof(SpriteAnimation_Stepper))]
public class SpriteAnimation_StepperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		if (GUILayout.Button($"Reset Animated Sprites To Default"))
		{
			(target as SpriteAnimation_Stepper).SetAnimatedSpritesToDefault();
		}

		if (GUILayout.Button($"Set dirty"))
		{
			UnityEditor.EditorUtility.SetDirty(target);
		}
	}
}
#endif