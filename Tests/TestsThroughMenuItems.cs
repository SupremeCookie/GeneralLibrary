﻿
#if UNITY_EDITOR
using RogueLike;
using UnityEditor;
using UnityEngine;

public class TestsThroughMenuItems
{
	private const string TestDirName = "Tests/";
	private const string ExtensionsDirName = "ExtensionMethods/";

	[MenuItem(TestDirName + ExtensionsDirName + "Test Rotation Matrices")]
	private static void TestRotationVector()
	{
		var firstVec = new Vector2(0, 1);
		var secondVec = new Vector2(0, 15);
		//var thirdVec = new Vector2(0, -1);
		//var fourthVec = new Vector2(-3, 0);
		//var fifthVec = new Vector2(5, 15);
		//var sixthVec = new Vector2(-5, 7);

		var firstOrigin = new Vector2(0, 0);
		//var secondOrigin = new Vector2(1, 3);
		//var thirdOrigin = new Vector2(-4, 6);

		var firstAngle = 15f;
		var secondAngle = 90f;
		//var thirdAngle = 150f;
		//var fourthAngle = -40f;

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

		//Debug.LogFormat("Rotate {0}, around {1}, result: {2}", firstVec, firstOrigin, firstVec.RotateVectorAroundVector(firstOrigin, firstAngle).ToString("N3"));
		//Debug.LogFormat("Rotate {0}, around {1}, result: {2}", secondVec, firstOrigin, secondVec.RotateVectorAroundVector(firstOrigin, firstAngle).ToString("N3"));
		//Debug.LogFormat("Rotate {0}, around {1}, result: {2}", firstVec, firstOrigin, firstVec.RotateVectorAroundVector(firstOrigin, firstAngle).ToString("N3"));
		//Debug.LogFormat("Rotate {0}, around {1}, result: {2}", secondVec, firstOrigin, secondVec.RotateVectorAroundVector(firstOrigin, firstAngle).ToString("N3"));
		//Debug.Log(firstVec.RotateVectorAroundVector(secondOrigin, secondAngle).ToString("N3"));

		//Debug.Log(secondVec.RotateVectorAroundVector(thirdOrigin, thirdAngle).ToString("N3"));
		//Debug.Log(secondVec.RotateVectorAroundVector(firstOrigin, fourthAngle).ToString("N3"));

		//Debug.Log(thirdVec.RotateVectorAroundVector(secondOrigin, firstAngle).ToString("N3"));
		//Debug.Log(thirdVec.RotateVectorAroundVector(thirdOrigin, secondAngle).ToString("N3"));

		//Debug.Log(fourthVec.RotateVectorAroundVector(firstOrigin, thirdAngle).ToString("N3"));
		//Debug.Log(fourthVec.RotateVectorAroundVector(secondOrigin, fourthAngle).ToString("N3"));

		//Debug.Log(fifthVec.RotateVectorAroundVector(thirdOrigin, firstAngle).ToString("N3"));
		//Debug.Log(fifthVec.RotateVectorAroundVector(firstOrigin, secondAngle).ToString("N3"));

		//Debug.Log(sixthVec.RotateVectorAroundVector(secondOrigin, thirdAngle).ToString("N3"));
		//Debug.Log(sixthVec.RotateVectorAroundVector(thirdOrigin, fourthAngle).ToString("N3"));
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

		var newRand = Utility.GetRandomVectorInDotProductRange(input, ranges[0].x, ranges[0].y);
		Debug.Log($"Input ({input})  using dot-ranges  ({ranges[0]})  created random ({newRand})");
		Debug.Log($"Dot product of ({input}) and ({newRand})   is ({DotProduct.CalculateDotProduct(input, newRand)})");

		var newRand2 = Utility.GetRandomVectorInDotProductRange(input2, ranges[1].x, ranges[1].y);
		Debug.Log($"Input ({input2})  using dot-ranges ({ranges[1]})  created random ({newRand2})");
		Debug.Log($"Dot product of ({input2}) and ({newRand2})   is ({DotProduct.CalculateDotProduct(input2, newRand2)})");

		var newRand3 = Utility.GetRandomVectorInDotProductRange(input3, ranges[2].x, ranges[2].y);
		Debug.Log($"Input ({input3})  using dot-ranges ({ranges[2]})  created random ({newRand3})");
		Debug.Log($"Dot product of ({input3}) and ({newRand3})   is ({DotProduct.CalculateDotProduct(input3, newRand3)})");

		var newRand4 = Utility.GetRandomVectorInDotProductRange(input4, ranges[3].x, ranges[3].y);
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

		var res1 = Utility.AngleBetweenVectors(vec1, vec2);
		var res2 = Utility.AngleBetweenVectors(vec1, vec2);
		var res3 = Utility.AngleBetweenVectors(vec1, vec3);
		var res4 = Utility.AngleBetweenVectors(vec2, vec4);
		var res5 = Utility.AngleBetweenVectors(vec1, vec1);
		var res6 = Utility.AngleBetweenVectors(vec3, vec3);
		var res7 = Vector2.SignedAngle(vec1, vec5);
		var res8 = Vector2.SignedAngle(vec5, vec1);

		Debug.Assert(res1.IsCloseTo(Mathf.PI * 0.5f));  // Half-Pi in radians
		Debug.Assert(res2.IsCloseTo(Mathf.PI * 0.5f));  // Half-Pi in radians
		Debug.Assert(res3.IsCloseTo(Mathf.PI * 1.0f));  // Full-Pi in radians
		Debug.Assert(res4.IsCloseTo(Mathf.PI * 1.0f));  // Full-Pi in radians
		Debug.Assert(res5.IsCloseTo(Mathf.PI * 0.0f));  // No-Pi in radians
		Debug.Assert(res6.IsCloseTo(Mathf.PI * 0.0f));  // No-Pi in radians
		Debug.Log("Res7:  " + res7.ToString("N2"));
		Debug.Log("Res8:  " + res8.ToString("N2"));

		Debug.Log("Finished Test Angles Between Vectors");
	}


	[MenuItem(TestDirName + ExtensionsDirName + "Test Custom Random predictability")]
	private static void TestCustomRandomPredictability()
	{
		Debug.Log("============ Test =============");

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < 20; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < 20; ++i)
			{
				result += newRand.Next() % 100;
			}

			Debug.Log(result);
		}

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < 20; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < 20; ++i)
			{
				result += newRand.Next() % 100;
			}

			Debug.Log(result);
		}

		{
			var rand = new CustomRandom(SeedGenerator.GetRandomSeed());

			string result = "";
			for (int i = 0; i < 20; ++i)
			{
				result += rand.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newRand = new CustomRandom(rand.Seed);
			for (int i = 0; i < 20; ++i)
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
			for (int i = 0; i < 20; ++i)
			{
				result += systemRandom.Next() % 100;
			}

			Debug.Log(result);

			result = "";
			var newSystemRandom = new System.Random(seed);
			for (int i = 0; i < 20; ++i)
			{
				result += newSystemRandom.Next() % 100;
			}

			Debug.Log(result);
		}



		Debug.Log("=========== Test Done ===========");
	}
}
#endif