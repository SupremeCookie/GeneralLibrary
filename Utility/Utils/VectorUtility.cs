using UnityEngine;

public partial class Utility
{
	/// <summary>
	/// Steps:
	/// 
	/// Turn Vectors into their y=ax+b equivalent
	/// On intersection point our y and x are the same
	/// So (Line1)ax+b = (Line2)ax+b
	/// (intersect)x = (b1 - b2)/(a2 - a1)
	/// (intersect)y = (Line1) a(intersect)x+b
	/// <returns></returns>
	public static Vector2 FindIntersection(Vector2 firstStart, Vector2 firstEnd, Vector2 secondStart, Vector2 secondEnd)
	{
		var result = Vector2.zero;

		double xDelta1 = firstEnd.x - firstStart.x;
		var isVertical1 = xDelta1 == 0;
		xDelta1 = isVertical1 ? 1f : xDelta1;   //XDelta must always be atleast 1, for the direction coeficcient calculations.	In case of vertical lines.

		double a1 = (firstEnd.y - firstStart.y) / xDelta1;
		double b1 = firstStart.y - (firstStart.x * a1);

		double xDelta2 = secondEnd.x - secondStart.x;   //XDelta must always be atleast 1, for the direction coeficcient calculations.	In case of vertical lines.
		var isVertical2 = xDelta2 == 0;
		xDelta2 = isVertical2 ? 1f : xDelta2;

		double a2 = (secondEnd.y - secondStart.y) / xDelta2;
		double b2 = secondStart.y - (secondStart.x * a2);

		if (!isVertical2)
			result.x = (float)((b1 - b2) / (a2 - a1));   //Reason for b1-b2 and a2-a1 is down below.
		else
			result.x = secondStart.x;

		result.y = (float)((a1 * result.x) + b1);

		//Debug.Log((isVertical1) + " :  " + (isVertical2) + " ::::  " + (firstEnd.x - firstStart.x) + " :  " + (secondEnd.x - secondStart.x) + " : " + firstStart + " :  " + secondStart + " ----   "
		//	+ xDelta1 + " : " + xDelta2 + " : " + a1 + " : " + b1 + " : " + a2 + " : " + b2 + "   :   " + result);

		return result;
	}

	public static Vector2 DeltaVector(Vector2 from, Vector2 to)
	{
		return new Vector2(to.x - from.x, to.y - from.y);
	}

	public static bool AreCirclesCloseToEachother(Vector2 first, Vector2 second, float circleRadius)
	{
		//If distance between is smaller than twice the radius.
		float sqrd = circleRadius * circleRadius;
		//TODO: Performance test
		if (FastSqrMagnitude(first, second)/*(first - second).sqrMagnitude */<= ((sqrd + sqrd) * 2f))
		{
			return true;
		}

		return false;
	}

	public static bool AreCloseToEachother(Vector2 first, Vector2 second, float distance)
	{
		//TODO: Performance test.
		float sqrd = distance * distance;
		if (FastSqrMagnitude(first, second)/*(first - second).sqrMagnitude */<= sqrd)
		{
			return true;
		}

		return false;
	}

	public static float SqrMagnitude(Vector2 first, Vector2 second)
	{
		//TODO: Performance test.
		return FastSqrMagnitude(first, second);// (second - first).sqrMagnitude;
	}

	public static float FastSqrMagnitude(Vector2 first, Vector2 second)
	{
		float xDelta = System.Math.Abs(first.x - second.x);
		float yDelta = System.Math.Abs(first.y - second.y);
		return (xDelta * xDelta + yDelta * yDelta);
	}

	public static float FastMagnitude(Vector2 first, Vector2 second)
	{
		float xDelta = System.Math.Abs(second.x - first.x);
		float yDelta = System.Math.Abs(second.y - first.y);
		return Mathf.Sqrt((xDelta * xDelta) + (yDelta * yDelta));
	}

	public static Vector2 AngleToVector(float angle)
	{
		return new Vector2
		(
			Mathf.Cos(angle * Mathf.Deg2Rad).Truncate(),
			Mathf.Sin(angle * Mathf.Deg2Rad).Truncate()
		);
	}
}



/* EXPLANATION (b1-b2)/(a2-a1)
When you put both formulas against eachother you get
ax+b = ax+b
You then get a's and b's to either side.
resulting in 
b - b = ax - ax

b/a is because you want x isolated.


*/
