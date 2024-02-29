using UnityEngine;

public class TestUIResolutionInfluence : MonoBehaviour
{
	private RectTransform rect;
	private Canvas canvas;

	[SerializeField] [Readonly] private Vector2 sizeDelta;
	[SerializeField] [Readonly] private Vector2 sizeDeltaScaled;
	[SerializeField] [Readonly] private Vector2 position;
	[SerializeField] [Readonly] private Vector2 positionScaled;

	[SerializeField] [Readonly] private MinMaxRectangle bbox;
	[SerializeField] [Readonly] private MinMaxRectangle bboxScaled;


	private void Awake()
	{
		rect = this.GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
	}

	private void Update()
	{
		var canvasScalar = canvas.scaleFactor;

		sizeDelta = rect.sizeDelta;
		sizeDeltaScaled = rect.sizeDelta * canvasScalar;
		position = rect.position;
		positionScaled = rect.position * canvasScalar;
		bbox = new MinMaxRectangle(position + (sizeDelta * -0.5f), position + (sizeDelta * 0.5f));
		bboxScaled = new MinMaxRectangle(position + (sizeDelta * -0.5f) * canvasScalar, position + (sizeDelta * 0.5f) * canvasScalar);
	}
}
