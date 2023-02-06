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



	public static void RunLineUtilUnitTests()
	{
		Test_A();
		Test_B();
		Test_C();
		Test_D();
		Test_E();
		Test_F();
		Test_G();

		Debug.Log("If no errors showed up, everything went well");
	}

	private static void Test_A()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(1, 0));
		Line second = new Line(new Vector2(1, 0), new Vector2(2, 0));

		// Shouldn't intersect as the second line starts where the first ends
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"A) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (A)");
	}

	private static void Test_B()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(2, 0));
		Line second = new Line(new Vector2(1, -1), new Vector2(1, 1));

		// Should intersect at  1,0
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"B) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (B)");
	}

	private static void Test_C()
	{
		Line first = new Line(new Vector2(0, 1), new Vector2(10, 0));
		Line second = new Line(new Vector2(0, 0), new Vector2(10, 1));

		// Should intersect
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"C) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (C)");
	}

	private static void Test_D()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(1000, 1));
		Line second = new Line(new Vector2(0, 1), new Vector2(1000, 0));

		// Should intersect
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"D) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (D)");
	}

	private static void Test_E()
	{
		Line first = new Line(new Vector2(0, 0.01f), new Vector2(100, 0.01f));
		Line second = new Line(new Vector2(0, 0), new Vector2(100, 0));

		// Should not intersect
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"E) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (E)");
	}

	private static void Test_F()
	{
		Line first = new Line(new Vector2(0, 1000), new Vector2(1, 0));
		Line second = new Line(new Vector2(0, 0), new Vector2(1, 1000));

		// Should intersect
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"F) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (F)");
	}

	private static void Test_G()
	{
		Line first = new Line(new Vector2(0.01f, 0), new Vector2(0.01f, 10000));
		Line second = new Line(new Vector2(0, 0), new Vector2(0, 10000));

		// Shouldn't intersect
		var intersect = DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"G) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (G)");
	}
}
