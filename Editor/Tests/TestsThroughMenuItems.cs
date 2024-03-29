﻿#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class TestsThroughMenuItems
{
	private const string TestDirName = "Tests/";
	private const string ExtensionsDirName = "ExtensionMethods/";
	private const string UtilityDirName = "UtilityMethods/";
	private const string MultithreadDirName = "MultiThreading/";

	[MenuItem(TestDirName + ExtensionsDirName + "Test Rotation Matrices")]
	private static void TestRotationVector()
	{
		var firstVec = new Vector2(0, 1);
		var secondVec = new Vector2(0, 15);

		var firstOrigin = new Vector2(0, 0);

		var firstAngle = 15f;
		var secondAngle = 90f;

		var resultOne = RotateAround(firstVec, firstOrigin, firstAngle);
		var resultTwo = RotateAround(secondVec, firstOrigin, firstAngle);
		var resultThird = RotateAround(firstVec, firstOrigin, secondAngle);
		var resultFourth = RotateAround(secondVec, firstOrigin, secondAngle);

		Debug.Log("Dot Product {same angle, different vectors}  " + Vector2.Dot(resultOne.normalized, resultTwo.normalized));
		Debug.Log("Dot Product {different angle, same vectors}  " + Vector2.Dot(resultOne.normalized, resultThird.normalized));
		Debug.Log("Dot Product {different angle, different vectors}  " + Vector2.Dot(resultOne.normalized, resultFourth.normalized));
		Debug.Log("Dot Product {different angle set, same angle, different vectors}  " + Vector2.Dot(resultThird.normalized, resultFourth.normalized));

		Debug.Log("Expected Outcome: First evaluation should be the same dotproduct." +
			"Second evaluation should be a differring dotproduct." +
			"Third evaluation should be a differing dotproduct." +
			"Fourth evaluation should be the same.");
	}

	private static Vector2 RotateAround(Vector2 rotation, Vector2 origin, float angle)
	{
		var result = rotation.RotateVectorAroundVector(origin, angle);
		Debug.LogFormat("Rotate {0}, around {1}, with an alpha of: {2}, result: {3}", rotation, origin, angle, result.ToString("N3"));
		return result;
	}

	[MenuItem(TestDirName + ExtensionsDirName + "Test Random Vectors")]
	private static void TestRandomVectors()
	{
		var input = new Vector2(0, 1).normalized;
		var input2 = new Vector2(1, 1).normalized;
		var input3 = new Vector2(1, 0).normalized;
		var input4 = new Vector2(5, 1).normalized;

		var ranges = new Vector2[]
		{
			 new Vector2(0.0f, 0.0f),
			 new Vector2(0.5f, 0.5f),
			 new Vector2(0.0f, 1.0f),
			 new Vector2(0.4f, 0.6f),
		};

		var newRand = Utility.GetRandomVectorInDotProductRange(input, ranges[0].x, ranges[0].y, out float _);
		Debug.Log($"Input ({input})  using dot-ranges  ({ranges[0]})  created random ({newRand})");
		Debug.Log($"Dot product of ({input}) and ({newRand})   is ({DotProduct.CalculateDotProduct(input, newRand)})");

		var newRand2 = Utility.GetRandomVectorInDotProductRange(input2, ranges[1].x, ranges[1].y, out float _);
		Debug.Log($"Input ({input2})  using dot-ranges ({ranges[1]})  created random ({newRand2})");
		Debug.Log($"Dot product of ({input2}) and ({newRand2})   is ({DotProduct.CalculateDotProduct(input2, newRand2)})");

		var newRand3 = Utility.GetRandomVectorInDotProductRange(input3, ranges[2].x, ranges[2].y, out float _);
		Debug.Log($"Input ({input3})  using dot-ranges ({ranges[2]})  created random ({newRand3})");
		Debug.Log($"Dot product of ({input3}) and ({newRand3})   is ({DotProduct.CalculateDotProduct(input3, newRand3)})");

		var newRand4 = Utility.GetRandomVectorInDotProductRange(input4, ranges[3].x, ranges[3].y, out float _);
		Debug.Log($"Input ({input4})  using dot-ranges ({ranges[3]})  created random ({newRand4})");
		Debug.Log($"Dot product of ({input4}) and ({newRand4})   is ({DotProduct.CalculateDotProduct(input4, newRand4)})");
	}


	[MenuItem(TestDirName + ExtensionsDirName + "Test Cross Product")]
	private static void TestCrossProduct()
	{
		var down = Vector2.down;
		var right = Vector2.right;
		var left = Vector2.left;

		var zFirst = Vector3.Cross(down, right).z;
		bool isToUs = zFirst > 0;

		var zSecond = Vector3.Cross(down, left).z;
		bool isFromUs = zFirst < 0;


		Debug.Log($"Result of down and right should be positive and to us, value: ({zFirst}) toUs: ({isToUs})");
		Debug.Log($"Result of down and left should be negative and from us, value: ({zSecond}) toUs: ({isFromUs})");
	}


	[MenuItem(TestDirName + ExtensionsDirName + "Test Angles Between Vectors")]
	private static void TestAngleBetween()
	{
		var vec1 = new Vector2(1, 0);
		var vec2 = new Vector2(0, 1);
		var vec3 = new Vector2(-1, 0);
		var vec4 = new Vector2(0, -1);
		var vec5 = new Vector2(0.9f, -0.1f).normalized;

		var res1 = Utility.RadianBetweenVectors(vec1, vec2);
		var res2 = Utility.RadianBetweenVectors(vec1, vec2);
		var res3 = Utility.RadianBetweenVectors(vec1, vec3);
		var res4 = Utility.RadianBetweenVectors(vec2, vec4);
		var res5 = Utility.RadianBetweenVectors(vec1, vec1);
		var res6 = Utility.RadianBetweenVectors(vec3, vec3);
		var res7 = Vector2.SignedAngle(vec1, vec5);
		var res8 = Vector2.SignedAngle(vec5, vec1);

		Debug.Assert(res1.IsCloseTo(Mathf.PI * 0.5f), $"res1 is not close to half pi: {res1}, halfPi: {Mathf.PI * 0.5f}");  // Half-Pi in radians
		Debug.Assert(res2.IsCloseTo(Mathf.PI * 0.5f), $"res2 is not close to half pi: {res2}, halfPi: {Mathf.PI * 0.5f}");  // Half-Pi in radians
		Debug.Assert(res3.IsCloseTo(Mathf.PI * 1.0f), $"res3 is not close to full pi: {res3}");  // Full-Pi in radians
		Debug.Assert(res4.IsCloseTo(Mathf.PI * 1.0f), $"res4 is not close to full pi: {res4}");  // Full-Pi in radians
		Debug.Assert(res5.IsCloseTo(Mathf.PI * 0.0f), $"res5 is not close to no pi: {res5}");  // No-Pi in radians
		Debug.Assert(res6.IsCloseTo(Mathf.PI * 0.0f), $"res6 is not close to no pi: {res6}");  // No-Pi in radians

		Debug.Log("Res7:  " + res7.ToString("N2"));
		Debug.Log("Res8:  " + res8.ToString("N2"));

		Debug.Log("Finished Test Angles Between Vectors");
	}


	[MenuItem(TestDirName + ExtensionsDirName + "Test Custom Random predictability")]
	private static void TestCustomRandomPredictability()
	{
		Debug.Log("============ Test =============");

		const int runAmount = 30;

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < runAmount; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < runAmount; ++i)
			{
				result += newRand.Next() % 100;
			}

			Debug.Log(result);
		}

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < runAmount; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < runAmount; ++i)
			{
				result += newRand.Next() % 100;
			}

			Debug.Log(result);
		}

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < runAmount; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < runAmount; ++i)
			{
				result += newRand.Next() % 100;
			}

			Debug.Log(result);
		}

		Debug.Log("Normal System Random");

		{
			var seed = SeedGenerator.GetRandomSeed();
			var systemRandom = new System.Random(seed);

			string result = "";
			for (int i = 0; i < runAmount; ++i)
			{
				result += systemRandom.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newSystemRandom = new System.Random(seed);
			for (int i = 0; i < runAmount; ++i)
			{
				result += newSystemRandom.Next() % 100;
			}

			Debug.Log(result);
		}

		// Make a custom random within an editor coroutine
		// Run the code, wait a few seconds, make it anew, run it again.0
		//eDITORCOroutineUtility
		EditorCoroutine coroutine = null;
		void killCoroutine()
		{
			if (coroutine != null)
			{
				EditorCoroutineUtility.StopCoroutine(coroutine);
			}
		}

		coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(DelayedTest(killCoroutine));

		Debug.Log("=========== Test Done ===========");
	}

	private static IEnumerator DelayedTest(System.Action onFinished)
	{
		const int runAmount = 30;
		CustomRandom random = new CustomRandom(SeedGenerator.GetRandomSeed());
		string result = "";
		for (int i = 0; i < runAmount; ++i)
		{
			result += random.Next() % 100;
		}

		Debug.Log(result);

		bool shouldRun = true;
		while (shouldRun)
		{
			Debug.Log($"Waiting a duration");
			yield return new EditorWaitForSeconds(2.5f);

			random = new CustomRandom(random.Seed);
			result = "";
			for (int i = 0; i < runAmount; ++i)
			{
				result += random.Next() % 100;
			}

			Debug.Log(result);
			shouldRun = false;
		}

		onFinished?.Invoke();

		Debug.Log("=========== Delayed Test Done ===========");
	}


	[MenuItem(TestDirName + ExtensionsDirName + "Test Octa Dir Flip")]
	private static void TestOctaDirFlip()
	{
		var first = OctaDirection.Left;
		var second = OctaDirection.Right;
		var third = OctaDirection.Up;
		var fourth = OctaDirection.Down;
		var fifth = OctaDirection.UpLeft;
		var sixth = OctaDirection.UpRight;
		var seventh = OctaDirection.DownLeft;
		var eighth = OctaDirection.DownRight;

		Debug.Log($"From ({first}) to ({first.Flip()})");
		Debug.Log($"From ({second}) to ({second.Flip()})");
		Debug.Log($"From ({third}) to ({third.Flip()})");
		Debug.Log($"From ({fourth}) to ({fourth.Flip()})");
		Debug.Log($"From ({fifth}) to ({fifth.Flip()})");
		Debug.Log($"From ({sixth}) to ({sixth.Flip()})");
		Debug.Log($"From ({seventh}) to ({seventh.Flip()})");
		Debug.Log($"From ({eighth}) to ({eighth.Flip()})");
	}

	[MenuItem(TestDirName + UtilityDirName + "Test Line Intersection")]
	private static void TestLineIntersection()
	{
		RunLineUtilUnitTests();
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
		Test_H();
		Test_I();

		Debug.Log("If no errors showed up, everything went well");
	}

	private static void Test_A()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(1, 0));
		Line second = new Line(new Vector2(1, 0), new Vector2(2, 0));

		// Shouldn't intersect as the second line starts where the first ends
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"A) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (A)");
	}

	private static void Test_B()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(2, 0));
		Line second = new Line(new Vector2(1, -1), new Vector2(1, 1));

		// Should intersect at  1,0
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"B) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (B)");
	}

	private static void Test_C()
	{
		Line first = new Line(new Vector2(0, 1), new Vector2(10, 0));
		Line second = new Line(new Vector2(0, 0), new Vector2(10, 1));

		// Should intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"C) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (C)");
	}

	private static void Test_D()
	{
		Line first = new Line(new Vector2(0, 0), new Vector2(1000, 1));
		Line second = new Line(new Vector2(0, 1), new Vector2(1000, 0));

		// Should intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"D) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (D)");
	}

	private static void Test_E()
	{
		Line first = new Line(new Vector2(0, 0.01f), new Vector2(100, 0.01f));
		Line second = new Line(new Vector2(0, 0), new Vector2(100, 0));

		// Should not intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"E) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (E)");
	}

	private static void Test_F()
	{
		Line first = new Line(new Vector2(0, 1000), new Vector2(1, 0));
		Line second = new Line(new Vector2(0, 0), new Vector2(1, 1000));

		// Should intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"F) The lines {first} and {second} should intersect, yet they do not");
		Debug.Log("Ran test (F)");
	}

	private static void Test_G()
	{
		Line first = new Line(new Vector2(0.01f, 0), new Vector2(0.01f, 10000));
		Line second = new Line(new Vector2(0, 0), new Vector2(0, 10000));

		// Shouldn't intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(!intersect, $"G) The lines {first} and {second} should not intersect, yet they do");
		Debug.Log("Ran test (G)");
	}

	private static void Test_H()
	{
		Line first = new Line(new Vector2(0, -1), new Vector2(0, 1));
		Line second = new Line(new Vector2(4, 0), new Vector2(6, 0));

		// Should intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"H) The lines {first} and {second} shouldn't intersect, and they don't, this proves that Line-Intersections are only on the line segments");
		Debug.Log("Ran test (H)");
	}

	private static void Test_I()
	{
		Line first = new Line(new Vector2(-10, -1), new Vector2(-1, 1));
		Line second = new Line(new Vector2(0, -5), new Vector2(1, -6));

		// Should intersect
		var intersect = Utility.DoLinesIntersect(first, second);

		Debug.Assert(intersect, $"I) The lines {first} and {second} shouldn't intersect, and they don't, this proves that Line-Intersections are only on the line segments");
		Debug.Log("Ran test (I)");
	}


	private static ThreadChecker tc;
	private static bool threadIsMainThread = false;
	private static bool threadIsRunning = false;

	[MenuItem(TestDirName + MultithreadDirName + "Test ThreadChecker")]
	private static void TestThreadChecker()
	{
		if (!Application.isPlaying)
		{
			Debug.LogError($"Can't run this test without running the game");
			return;
		}

		Debug.Log($"Test the Thread Checker");

		threadIsMainThread = false;
		tc = new ThreadChecker();
		tc.SetMainThreadID();

		bool mainThreadIsMainThread = tc.IsCurrentThreadMainThread();

		var newThread = new Thread(ThreadRunner);
		threadIsRunning = true;
		newThread.Start();

		while (threadIsRunning)
		{
			Thread.Sleep(10);
		}

		Debug.Log($"MainThreadIsMainThread: {mainThreadIsMainThread}   (should be true)");
		Debug.Log($"Worked thread is main thread: {threadIsMainThread}   (should be false)");
	}

	private static void ThreadRunner()
	{
		threadIsMainThread = tc.IsCurrentThreadMainThread();
		Thread.Sleep(10);

		threadIsRunning = false;
	}
}
#endif