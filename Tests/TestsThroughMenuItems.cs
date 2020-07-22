
#if UNITY_EDITOR
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
}
#endif