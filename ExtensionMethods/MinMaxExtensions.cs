using UnityEngine;

public static class MinMaxExtensions
{
	public static float GetSurfaceArea(this MinMax current)
	{
		Debug.Assert(current != null, "Can't grab teh surface area of a MinMax that's null");
		return current.Width * current.Height;
	}

	public static bool IsOverlapping(this MinMax current, MinMax other)
	{
		if (current == null || other == null)
		{
			return false;
		}

		float totalWidth = (current.Width + other.Width) * 0.5f;            // Note DK: We only take half, because we test between centers, so we only test halfwidth/height
		float totalHeight = (current.Height + other.Height) * 0.5f;         // Note DK: We only take half, because we test between centers, so we only test halfwidth/height

		bool totalWidthBiggerThanZero = totalWidth > 0;
		bool totalHeightBiggerThanZero = totalHeight > 0;
		if (!totalWidthBiggerThanZero || !totalHeightBiggerThanZero)
		{
			return false;
		}

		Vector2 delta = other.GetCenter() - current.GetCenter();

		bool widthCloseEnough = Mathf.Abs(delta.x) < totalWidth;
		bool heightCloseEnough = Mathf.Abs(delta.y) < totalHeight;

		bool widthOnEdge = Mathf.Abs(delta.x).IsCloseTo(totalWidth, 0.0001f);
		bool heightOnEdge = Mathf.Abs(delta.y).IsCloseTo(totalHeight, 0.0001f);

		return (widthCloseEnough && heightCloseEnough) && (!widthOnEdge && !heightOnEdge);
	}

	public static MinMax GetOverlappingArea(this MinMax current, MinMax other)
	{
		// Note DK: We assume that IsOverlapping has been checked, but for debug reasons we will be asserting anyway.
		Debug.Assert(current.IsOverlapping(other), "We are trying to get the overlapping area between two minMaxes, but there is no overlap");

		var result = new MinMax();

		// The idea is that you find the most relatable value on the min and max variables
		// This means the biggest of the 2 mins, and the smallest of the 2 maxes.
		// Then you have the inner most area of the 2 overlapping squares.
		// Doing it the other way around gives you the total bounding box.
		result.Min = new Vector2(Mathf.Max(current.Min.x, other.Min.x), Mathf.Max(current.Min.y, other.Min.y));
		result.Max = new Vector2(Mathf.Min(current.Max.x, other.Max.x), Mathf.Min(current.Max.y, other.Max.y));

		return result;
	}

	public static bool ContainsPoint(this MinMax minMax, Vector2 point)
	{
		return
			minMax.Min.x < point.x &&
			minMax.Min.y < point.y &&
			minMax.Max.x > point.x &&
			minMax.Max.y > point.y;
	}

	public static void AddOffset(this MinMax minMax, Vector2 offset)
	{
		minMax.Min += offset;
		minMax.Max += offset;
	}

	public static void Multiply(this MinMax minMax, float multiplier)
	{
		minMax.Min *= multiplier;
		minMax.Max *= multiplier;
	}

	public static void MultiplyAroundCenter(this MinMax minMax, float multiplier)
	{
		// Note DK: we only multiply the size, we don't relocate the object.
		// That means we don't simply multiply the min and maxes, we grab the center
		// then we multiply the delta to min and max, and move the min and max accordingly.
		// We multiply around the center.

		var center = minMax.GetCenter();
		var deltaToMax = minMax.Max - center;

		var newDeltaToMax = deltaToMax * multiplier;
		var deltaToMin = -newDeltaToMax;

		minMax.Min = center + deltaToMin;
		minMax.Max = center + newDeltaToMax;
	}

	public static MinMax Copy(this MinMax original)
	{
		return new MinMax
		{
			Min = new Vector2(original.Min.x, original.Min.y),
			Max = new Vector2(original.Max.x, original.Max.y),
		};
	}


	private static readonly Color whiteColor = new Color(1, 1, 1, 1);
	public static void DrawGizmos(this MinMax target, Vector3 positionOffset = default(Vector3), bool drawCross = false, bool drawFill = false, Color? color = null)
	{
		if (color == null)
		{
			color = whiteColor;
		}

		var bboxPositions = new Vector3[]
		{
			new Vector3(target.Min.x, target.Min.y) + positionOffset,	// Bottom left
			new Vector3(target.Min.x, target.Max.y) + positionOffset,
			new Vector3(target.Max.x, target.Max.y) + positionOffset,	// Bottom right
			new Vector3(target.Max.x, target.Min.y) + positionOffset,
		};

		if (drawFill)
		{
			var colorCopy = color.Value;
			colorCopy.a *= 0.2f;
			Gizmos.color = colorCopy;

			var centerPos = Vector3.Lerp(bboxPositions[0], bboxPositions[2], 0.5f); // Note DK: Super cheap check, the middle point between bottom left and top right is center
			Gizmos.DrawCube(centerPos, new Vector3(target.Width, target.Height));
		}

		Gizmos.color = color.Value;

		for (int i = 0; i < bboxPositions.Length; ++i)
		{
			Gizmos.DrawSphere(bboxPositions[i], 0.01f);
			Gizmos.DrawLine(bboxPositions[i], bboxPositions[(i + 1) % 4]);
		}

		if (drawCross)
		{
			Gizmos.DrawLine(bboxPositions[0], bboxPositions[2]);
			Gizmos.DrawLine(bboxPositions[1], bboxPositions[3]);
		}
	}
}
