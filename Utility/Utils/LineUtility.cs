using UnityEngine;

public partial class Utility
{
	// https://stackoverflow.com/questions/9043805/test-if-two-lines-intersect-javascript-function
	public static bool DoLinesIntersect(Line first, Line second)
	{
		float determinant, gamma, lambda;

		var fStart = first.start;
		var fEnd = first.end;
		var sStart = second.start;
		var sEnd = second.end;

		determinant = (fEnd.x - fStart.x) * (sEnd.y - sStart.y) - (sEnd.x - sStart.x) * (fEnd.y - fStart.y);
		if (determinant.IsCloseTo(0, 0.001f))
		{
			return false;
		}

		lambda = ((sEnd.y - sStart.y) * (sEnd.x - fStart.x) + (sStart.x - sEnd.x) * (sEnd.y - fStart.y)) / determinant;
		gamma = ((fStart.y - fEnd.y) * (sEnd.x - fStart.x) + (fEnd.x - fStart.x) * (sEnd.y - fStart.y)) / determinant;
		return (0 < lambda && lambda < 1) && (0 < gamma && gamma < 1);
	}

	public static Vector2 GetLinesIntersectionPoint(Line first, Line second)
	{
		float determinant, lambda;

		var fStart = first.start;
		var fEnd = first.end;
		var sStart = second.start;
		var sEnd = second.end;

		determinant = (fEnd.x - fStart.x) * (sEnd.y - sStart.y) - (sEnd.x - sStart.x) * (fEnd.y - fStart.y);
		if (determinant.IsCloseTo(0, 0.001f))
		{
			Debug.Log($"No intersection found between lines: {first}, and {second}");
			return Vector2.negativeInfinity;
		}

		lambda = ((sEnd.y - sStart.y) * (sEnd.x - fStart.x) + (sStart.x - sEnd.x) * (sEnd.y - fStart.y)) / determinant;
		return Vector2.Lerp(first.start, first.end, lambda);
	}
}
