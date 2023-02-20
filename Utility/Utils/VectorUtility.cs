//#define LOG_WARNINGS

using UnityEngine;

public partial class Utility
{
	private const float PiRadToDegrees = 180f / Mathf.PI;

	private static CustomRandom _random;
	private static CustomRandom random
	{
		get { if (_random == null) { _random = new CustomRandom(); } return _random; }
	}

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

	public static Vector2 RandomVector2()
	{
		Vector2 vec = random.NextVector();
		return vec;
	}

	public static Vector2 AngleToVector(float angle)
	{
		return new Vector2
		(
			Mathf.Cos(angle * Mathf.Deg2Rad).Truncate(),
			Mathf.Sin(angle * Mathf.Deg2Rad).Truncate()
		);
	}

	/// <summary>
	/// Please specify in -Pi to +Pi
	/// </summary>
	public static Vector2 RadialToVector(float radials)
	{
		return new Vector2
		(
			Mathf.Cos(radials),
			Mathf.Sin(radials)
		);
	}

	public enum DotProductDirection
	{
		Clockwise,
		Counter_Clockwise,
		Both,
	};

	public static Vector2 GetVector2ForDotProduct(Vector2 input, float dotProduct, DotProductDirection dir)
	{
		Debug.Assert(input != default, $"Given Input is default Vector2, can't use this: {input}");

		bool takePositiveDegrees = dir == DotProductDirection.Counter_Clockwise;

		bool isNormalised = input.sqrMagnitude.IsCloseTo(1.0f);
		if (!isNormalised)
		{
			input.Normalize();
		}

		float radTheta = Mathf.Acos(dotProduct);        //from 0 to pi.		So 0-180 degrees.	or 0 to 1 pi-rad
		float degrees = RadianToAngle(radTheta);        //this turns the 0-3.14 space into 0-180 degrees

		var rotatedVec = Matrices.ApplyRotationMatrix(input, takePositiveDegrees ? degrees : -degrees);

		return rotatedVec;
	}

	public static Vector2 GetRandomVectorInDotProductRange(Vector2 input, float minDot, float maxDot, out float chosenDot, DotProductDirection dir = DotProductDirection.Both)
	{
		#region Explanation
		// so full rotation is 2 pi rad.
		// the input vector is at 1 dot product result
		// the minDot is from -1 to 1, max same.
		// if mindot is -1, we got an angle of 1 pi rad.
		// if mindot is 0, we got an angle of 0.5 pi rad or -0.5 pi rad

		// so dot product formula is    (result  a . b) =   |a||b| * cos(theta)
		// to get the pi rad angle theta based off of the crossproduct we can do this
		// cos theta = a . b   /   |a||b|
		// since we use normalized vectors, that becomes
		// cos theta = a . b
		// theta = arccos(a . b)

		// but we don't want to go from theta back to a new vector, we want a directly new vector  from the dot product


		// so there's an easy way to determine it for 2d normalised vectors.
		// you multiply the dotproduct against the x component of your input, then abs it, and take the leftovers of 1 - that absed value
		// and turn it into both signs, and you got your vectors.

		// ~ ~ Explanation ~ ~
		// whilst hopeful, the above mathematics doesn't work, so we gotta calculate theta, then turn that into a vector
		// next to that, we effectively rotate the vector using the dot product. 
		// so we need to calculate the theta in degrees, then use that to either go left or right, and rotate the vector.
		#endregion

		Debug.Assert(input != default, $"Given Input is default Vector2, can't use this: {input}");

		float randomDot = _rand.Range(minDot, maxDot);
		chosenDot = randomDot;

		bool takePositiveDegrees = _rand.RandBool();
		if (dir == DotProductDirection.Clockwise)
		{
			takePositiveDegrees = false;
		}
		else if (dir == DotProductDirection.Counter_Clockwise)
		{
			takePositiveDegrees = true;
		}

		//Debug.Log($"RandomDot ({randomDot})   take counter-clock turn ({takePositiveDegrees})");


		bool isNormalised = input.sqrMagnitude.IsCloseTo(1.0f);
		if (!isNormalised)
		{
			input.Normalize();
		}

		float radTheta = Mathf.Acos(randomDot);         //from 0 to pi.		So 0-180 degrees.	or 0 to 1 pi-rad
		float degrees = RadianToAngle(radTheta);        //this turns the 0-3.14 rad space into 0-180 degrees

		var rotatedVec = Matrices.ApplyRotationMatrix(input, takePositiveDegrees ? degrees : -degrees);

		return rotatedVec;
	}

	public static Vector2 GetRandomVector()
	{
		return _rand.NextVector();
	}

	public static Vector2 Abs(Vector2 input)
	{
		return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
	}



	// Angle in Radians
	// Unfortunately this takes the shortest angle, always.
	public static float RadianBetweenVectors(in Vector2 inputFirst, in Vector2 inputSecond)
	{
		// We use the dot product function here
		// Dot product dictates,   dot prod between a and b =  mag(a) * mag(b) * cos(theta)    where theta is the angle between the 2 vectors
		// Now we need the theta, and we luckily can calculate the result of the dot product differently. 

		float resultingDotProd = inputFirst.x * inputSecond.x + inputFirst.y * inputSecond.y;
		float sqrMag_1 = (inputFirst.x * inputFirst.x + inputFirst.y * inputFirst.y);
		float sqrMag_2 = (inputSecond.x * inputSecond.x + inputSecond.y * inputSecond.y);
		float sqrMagnitudes = sqrMag_1 * sqrMag_2;  // We can do this multiplication because the initial formula dictates this relation
		float magnitude = Mathf.Sqrt(sqrMagnitudes);

		Debug.Assert(magnitude != 0, $"magnitude is 0, ({magnitude}), inputFirst: {inputFirst}, inputSecond: {inputSecond}, dotProd: {resultingDotProd}, sqrMag1: {sqrMag_1}, sqrMag2: {sqrMag_2}, sqrMagnitudes: {sqrMagnitudes}");
		float nonArcCossed = resultingDotProd / magnitude;
		float arcCossed = Mathf.Acos(nonArcCossed);

		if (float.IsNaN(arcCossed) && nonArcCossed.IsCloseTo(1.0f))
		{
#if LOG_WARNINGS
			Debug.LogWarning($"arcCossed is considered NaN, nonArcCossed is close to 1: {nonArcCossed.ToString("N2")}, hard setting arcCossed to 0");
#endif
			arcCossed = 0;
		}

		Debug.Assert(!float.IsNaN(resultingDotProd), "-resultingDotProd- is NaN");
		Debug.Assert(!float.IsNaN(sqrMag_1), "-sqrMag_1- is NaN");
		Debug.Assert(!float.IsNaN(sqrMag_2), "-sqrMag_2- is NaN");
		Debug.Assert(!float.IsNaN(sqrMagnitudes), "-sqrMagnitudes- is NaN");
		Debug.Assert(!float.IsNaN(magnitude), "-magnitude- is NaN");
		Debug.Assert(!float.IsNaN(nonArcCossed), "-nonArcCossed- is NaN");
		Debug.Assert(!float.IsNaN(arcCossed), $"-arcCossed- is NaN, arcCossed:{arcCossed}, nonArcCossed: {nonArcCossed.ToString("N2")},   arcCossed = mathf.Acos(nonArcCossed)");
		return arcCossed;
	}

	public static float SignedThetaBetweenVectors(in Vector2 inputFirst, in Vector2 inputSecond)
	{
		return SignedRadianBetweenVectors(in inputFirst, in inputSecond);
	}

	public static float SignedRadianBetweenVectors(in Vector2 inputFirst, in Vector2 inputSecond)
	{
		float nonSignedAngle = RadianBetweenVectors(inputFirst, inputSecond);
		Vector3 crossBetween = Cross(in inputFirst, in inputSecond);

		if (crossBetween.z > 0)
		{
			return nonSignedAngle;
		}
		else
		{
			return -nonSignedAngle;
		}
	}

	public static float ThetaBetweenVectors(in Vector2 inputFirst, in Vector2 inputSecond)
	{
		return RadianBetweenVectors(inputFirst, inputSecond);
	}


	public static float RadianToAngle(in float radians)
	{
		return radians * PiRadToDegrees;
	}



	public static Vector3 Cross(in Vector2 inputFirst, in Vector2 inputSecond)
	{
		// We always work in the domain of vector3 but the z component is 0 for our input.
		Vector3 f = inputFirst;
		Vector3 s = inputSecond;

		Vector3 result = Vector3.zero;

		result.x = (f.y * s.z) - (f.z * s.y);
		result.y = (f.z * s.x) - (f.x * s.z);
		result.z = (f.x * s.y) - (f.y * s.x);

		return result;
	}



	/// <summary> Only works for eclipses alligned on the x/y axis; rotated eclipses aren't supported </summary>
	public static bool LiesWithinEclipse(Vector2 point, Vector2 eclipseExtents)
	{
		return LiesWithinEclipse(point.x, point.y, eclipseExtents.x, eclipseExtents.y);
	}

	/// <summary> Only works for eclipses alligned on the x/y axis; rotated eclipses aren't supported </summary>
	public static bool LiesWithinEclipse(Vector2 point, float eclipseExtentsX, float eclipseExtentsY)
	{
		return LiesWithinEclipse(point.x, point.y, eclipseExtentsX, eclipseExtentsY);
	}

	/// <summary> Only works for eclipses alligned on the x/y axis; rotated eclipses aren't supported </summary>
	public static bool LiesWithinEclipse(float pointX, float pointY, Vector2 eclipseExtents)
	{
		return LiesWithinEclipse(pointX, pointY, eclipseExtents.x, eclipseExtents.y);
	}

	/// <summary> Only works for eclipses alligned on the x/y axis; rotated eclipses aren't supported </summary>
	public static bool LiesWithinEclipse(float pointX, float pointY, float eclipseExtentsX, float eclipseExtentsY)
	{
		Debug.Assert(!float.IsNaN(pointX), "pointX is NaN");
		Debug.Assert(!float.IsNaN(pointY), "pointY is NaN");
		Debug.Assert(!float.IsNaN(eclipseExtentsX), "eclipseExtentsX is NaN");
		Debug.Assert(!float.IsNaN(eclipseExtentsY), "eclipseExtentsY is NaN");


		float sqrPointX = pointX * pointX;
		float sqrPointY = pointY * pointY;
		float sqrEclipseX = eclipseExtentsX * eclipseExtentsX;
		float sqrEclipseY = eclipseExtentsY * eclipseExtentsY;

		float xPortion = sqrPointX / sqrEclipseX;
		float yPortion = sqrPointY / sqrEclipseY;

		bool liesWithinEclipse = xPortion + yPortion <= 1;
		return liesWithinEclipse;

		#region verbose
		/*
			Why does this work?
			Well a circle and eclipse share a lot of similarities.
			Circles are just really simple eclipses.
			How do I know if a point lies within a circle? I check to see if the magnitude of the vector between it and the circle center is less than the radius of the circle.
			So
			sqrPointX + sqrPointY <= sqrRadiusCircle

			The thing is, an eclipse doesn't work entirely in that way, what needs to happen is that you need to factor in the differences between the radius-major and radius-minor.
			If the eclipse is alligned on the x/y axis that means either x axis is major or minor and the y axis is minor or major.
			Now how does this factor in on the earlier equation? 
			Quite simply, we can rewrite the earlier equation to
			sqrPointX / sqrRadiusCircle + sqrPointY / sqrRadiusCircle <= 1

			Why do we do this? Well we can't just go radius.minor * radius.major as meaning sqrRadiusCircle. Because then we'd effectively create a circle with radius in the midpoint between radius.minor and radius.major

			So what we do then is replace the sqrRadiusCircle's for their subsequent proper meanings in an eclipse which is
			sqrPointX / sqrRadius_xAxis + sqrPointY / sqrRadius_yAxis <= 1
		*/
		#endregion
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
