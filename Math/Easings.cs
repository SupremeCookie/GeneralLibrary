using UnityEngine;

// https://easings.net/
public static class Easings
{
	// https://easings.net/#easeInSine
	public static float In_Sine(float input)
	{
		return 1f - Mathf.Cos((input * Mathf.PI) / 2f);
	}
	// https://easings.net/#easeInCubic
	public static float In_Cubic(float input)
	{
		return input * input * input;
	}
	// https://easings.net/#easeInQuint
	public static float In_Quint(float input)
	{
		return input * input * input * input * input;
	}
	// https://easings.net/#easeInCirc
	public static float In_Circ(float input)
	{
		return 1f - Mathf.Sqrt(1f - Mathf.Pow(input, 2));
	}
	// https://easings.net/#easeInElastic
	/// <summary>
	/// [Note]: Rather expensive, so use sparingly.
	/// </summary>
	public static float In_Elastic(float input)
	{
		const float c4 = (2 * Mathf.PI) / 3;

		return input == 0
		  ? 0
		  : input == 1
			? 1f
			: -Mathf.Pow(2, 10f * input - 10f) * Mathf.Sin((input * 10f - 10.75f) * c4);
	}
	// https://easings.net/#easeInQuad
	public static float In_Quad(float input)
	{
		return input * input;
	}
	// https://easings.net/#easeInQuart
	public static float In_Quart(float input)
	{
		return input * input * input * input;
	}
	// https://easings.net/#easeInExpo
	public static float In_Expo(float input)
	{
		return input == 0
			? 0
			: Mathf.Pow(2, 10f * input - 10f);
	}
	// https://easings.net/#easeInBack
	public static float In_Back(float input)
	{
		const float c1 = 1.70158f;
		const float c3 = c1 + 1f;

		return c3 * input * input * input - c1 * input * input;
	}
	// https://easings.net/#easeInBounce
	public static float In_Bounce(float input)
	{
		return 1f - Out_Bounce(1f - input);
	}


	// https://easings.net/#easeOutSine
	public static float Out_Sine(float input)
	{
		return Mathf.Sin((input * Mathf.PI) / 2f);
	}
	// https://easings.net/#easeOutCubic
	public static float Out_Cubic(float input)
	{
		return 1f - Mathf.Pow(1f - input, 3);
	}
	// https://easings.net/#easeOutQuint
	public static float Out_Quint(float input)
	{
		return 1f - Mathf.Pow(1f - input, 5);
	}
	// https://easings.net/#easeOutCirc
	public static float Out_Circ(float input)
	{
		return Mathf.Sqrt(1f - Mathf.Pow(input - 1f, 2));
	}
	// https://easings.net/#easeOutElastic
	/// <summary>
	/// [Note]: Rather expensive, so use sparingly.
	/// </summary>
	public static float Out_Elastic(float input)
	{
		const float c4 = (2f * Mathf.PI) / 3f;

		return input == 0
		  ? 0
		  : input == 1
			? 1
			: Mathf.Pow(2, -10f * input) * Mathf.Sin((input * 10f - 0.75f) * c4) + 1f;
	}
	// https://easings.net/#easeOutQuad
	public static float Out_Quad(float input)
	{
		return 1f - (1f - input) * (1f - input);
	}
	// https://easings.net/#easeOutQuart
	public static float Out_Quart(float input)
	{
		return 1f - Mathf.Pow(1f - input, 4);
	}
	// https://easings.net/#easeOutExpo
	public static float Out_Expo(float input)
	{
		return input == 1f
			? 1f
			: 1f - Mathf.Pow(2, -10f * input);
	}
	// https://easings.net/#easeOutBack
	public static float Out_Back(float input)
	{
		const float c1 = 1.70158f;
		const float c3 = c1 + 1f;

		return 1 + c3 * Mathf.Pow(input - 1f, 3) + c1 * Mathf.Pow(input - 1f, 2);
	}
	// https://easings.net/#easeOutBounce
	public static float Out_Bounce(float input)
	{
		const float n1 = 7.5625f;
		const float d1 = 2.75f;

		if (input < 1f / d1)
		{
			return n1 * input * input;
		}
		else if (input < 2f / d1)
		{
			return n1 * (input -= 1.5f / d1) * input + 0.75f;
		}
		else if (input < 2.5f / d1)
		{
			return n1 * (input -= 2.25f / d1) * input + 0.9375f;
		}
		else
		{
			return n1 * (input -= 2.625f / d1) * input + 0.984375f;
		}
	}


	// https://easings.net/#easeInOutSine
	public static float In_Out_Sine(float input)
	{
		return -(Mathf.Cos(Mathf.PI * input) - 1f) / 2f;
	}
	// https://easings.net/#easeInOutCubic
	public static float In_Out_Cubic(float input)
	{
		return input < 0.5f
			? 4f * input * input * input
			: 1f - Mathf.Pow(-2 * input + 2, 3) / 2;
	}
	// https://easings.net/#easeInOutQuint
	public static float In_Out_Quint(float input)
	{
		return input < 0.5f
			? 16f * input * input * input * input * input
			: 1f - Mathf.Pow(-2f * input + 2f, 5) / 2f;
	}
	// https://easings.net/#easeInOutCirc
	public static float In_Out_Circ(float input)
	{
		return input < 0.5
			? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * input, 2f))) / 2f
			: (Mathf.Sqrt(1f - Mathf.Pow(-2f * input + 2f, 2f)) + 1f) / 2f;
	}
	// https://easings.net/#easeInOutElastic
	/// <summary>
	/// [Note]: Rather expensive, so use sparingly.
	/// </summary>
	public static float In_Out_Elastic(float input)
	{
		const float c5 = (2f * Mathf.PI) / 4.5f;

		return input == 0
		  ? 0f
		  : input == 1
			? 1f
			: input < 0.5f
				? -(Mathf.Pow(2f, 20f * input - 10f) * Mathf.Sin((20f * input - 11.125f) * c5)) / 2f
				: (Mathf.Pow(2f, -20f * input + 10f) * Mathf.Sin((20f * input - 11.125f) * c5)) / 2f + 1f;
	}
	// https://easings.net/#easeInOutQuad
	public static float In_Out_Quad(float input)
	{
		return input < 0.5f
			? 2f * input * input
			: 1f - Mathf.Pow(-2f * input + 2f, 2f) / 2f;
	}
	// https://easings.net/#easeInOutQuart
	public static float In_Out_Quart(float input)
	{
		return input < 0.5f
			? 8f * input * input * input * input
			: 1f - Mathf.Pow(-2f * input + 2f, 4f) / 2f;
	}
	// https://easings.net/#easeInOutEinputpo
	public static float In_Out_Einputpo(float input)
	{
		return input == 0
		  ? 0f
		  : input == 1
			? 1f
			: input < 0.5f
				? Mathf.Pow(2f, 20f * input - 10f) / 2f
				: (2f - Mathf.Pow(2f, -20f * input + 10f)) / 2f;
	}
	// https://easings.net/#easeInOutBack
	public static float In_Out_Back(float input)
	{
		const float c1 = 1.70158f;
		const float c2 = c1 * 1.525f;

		return input < 0.5f
		  ? (Mathf.Pow(2f * input, 2f) * ((c2 + 1f) * 2f * input - c2)) / 2f
		  : (Mathf.Pow(2f * input - 2f, 2f) * ((c2 + 1f) * (input * 2f - 2f) + c2) + 2f) / 2f;
	}
	// https://easings.net/#easeInOutBounce
	public static float In_Out_Bounce(float input)
	{
		return input < 0.5f
			? (1f - Out_Bounce(1f - 2f * input)) / 2f
			: (1f + Out_Bounce(2f * input - 1f)) / 2f;
	}
}