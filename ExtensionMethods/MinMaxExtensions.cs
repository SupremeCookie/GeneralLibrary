using UnityEngine;

public static class MinMaxExtensions
{
	public static float GetSurfaceArea(this MinMaxRectangle current)
	{
		Debug.Assert(current != null, "Can't grab teh surface area of a MinMax that's null");
		return current.width * current.height;
	}

	public static bool IsOverlapping(this MinMaxRectangle current, MinMaxRectangle other)
	{
		if (current == null || other == null)
		{
			return false;
		}

		float totalWidth = (current.width + other.width) * 0.5f;            // Note DK: We only take half, because we test between centers, so we only test halfwidth/height
		float totalHeight = (current.height + other.height) * 0.5f;         // Note DK: We only take half, because we test between centers, so we only test halfwidth/height

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

	public static MinMaxRectangle GetOverlappingArea(this MinMaxRectangle current, MinMaxRectangle other)
	{
		// Note DK: We assume that IsOverlapping has been checked, but for debug reasons we will be asserting anyway.
		Debug.Assert(current.IsOverlapping(other), "We are trying to get the overlapping area between two minMaxes, but there is no overlap");

		var result = new MinMaxRectangle();

		// The idea is that you find the most relatable value on the min and max variables
		// This means the biggest of the 2 mins, and the smallest of the 2 maxes.
		// Then you have the inner most area of the 2 overlapping squares.
		// Doing it the other way around gives you the total bounding box.
		result.min = new Vector2(Mathf.Max(current.min.x, other.min.x), Mathf.Max(current.min.y, other.min.y));
		result.max = new Vector2(Mathf.Min(current.max.x, other.max.x), Mathf.Min(current.max.y, other.max.y));

		return result;
	}

	public static bool ContainsPoint(this MinMaxRectangle minMax, Vector2 point)
	{
		return
			minMax.min.x < point.x &&
			minMax.min.y < point.y &&
			minMax.max.x > point.x &&
			minMax.max.y > point.y;
	}

	public static void AddOffset(this MinMaxRectangle minMax, Vector2 offset)
	{
		minMax.min += offset;
		minMax.max += offset;
	}

	public static void Multiply(this MinMaxRectangle minMax, float multiplier)
	{
		minMax.min *= multiplier;
		minMax.max *= multiplier;
	}

	public static void MultiplyAroundCenter(this MinMaxRectangle minMax, float multiplier)
	{
		// Note DK: we only multiply the size, we don't relocate the object.
		// That means we don't simply multiply the min and maxes, we grab the center
		// then we multiply the delta to min and max, and move the min and max accordingly.
		// We multiply around the center.

		var center = minMax.GetCenter();
		var deltaToMax = minMax.max - center;

		var newDeltaToMax = deltaToMax * multiplier;
		var deltaToMin = -newDeltaToMax;

		minMax.min = center + deltaToMin;
		minMax.max = center + newDeltaToMax;
	}

	public static MinMaxRectangle Copy(this MinMaxRectangle original)
	{
		return new MinMaxRectangle
		{
			min = new Vector2(original.min.x, original.min.y),
			max = new Vector2(original.max.x, original.max.y),
		};
	}


	private static readonly Color whiteColor = new Color(1, 1, 1, 1);
	public static void DrawGizmos(this MinMaxRectangle target, Vector3 positionOffset = default(Vector3), bool drawCross = false, bool drawFill = false, Color? color = null)
	{
		if (color == null)
		{
			color = whiteColor;
		}

		var bboxPositions = new Vector3[]
		{
			new Vector3(target.min.x, target.min.y) + positionOffset,	// Bottom left
			new Vector3(target.min.x, target.max.y) + positionOffset,
			new Vector3(target.max.x, target.max.y) + positionOffset,	// Bottom right
			new Vector3(target.max.x, target.min.y) + positionOffset,
		};

		if (drawFill)
		{
			var colorCopy = color.Value;
			colorCopy.a *= 0.2f;
			Gizmos.color = colorCopy;

			var centerPos = Vector3.Lerp(bboxPositions[0], bboxPositions[2], 0.5f); // Note DK: Super cheap check, the middle point between bottom left and top right is center
			Gizmos.DrawCube(centerPos, new Vector3(target.width, target.height));
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
