using System.Collections.Generic;
using UnityEngine;

namespace Triangulator
{
	// Refactor through to some kind of TriangleUtility class, this class should be ear specific logic, or be a passthrough/converter from Ear to Triangle
	public static class EarUtility
	{

		private static Vertex[] pointsCopy;
		private static VertexTriangle[] pointTriangles;

		static EarUtility()
		{
			pointTriangles = new VertexTriangle[3];
			pointTriangles[0] = new VertexTriangle();
			pointTriangles[1] = new VertexTriangle();
			pointTriangles[2] = new VertexTriangle();
		}

		public static bool AnyPointLiesWithinEar(in Ear ear, in List<Vertex> points)
		{
			bool defaultReturn = false;
			if (points.Count == 0)
			{
				return defaultReturn;
			}


			Debug.Assert(ear != null, $"Given ear is null, gotta fix this");
			Debug.Assert(!ear.vertices.IsNullOrEmpty(), $"Given ear's vertices are null or empty, gotta fix this");

			pointsCopy = ScrubEarVertices(in ear, in points);

			//Caching the pointsCopy is going to save soooo much garbage
			for (int i = 0; i < pointsCopy.Length; ++i)
			{
				var point = pointsCopy[i];
				if (DoesPointLieWithinEar(point, in ear))
				{
					return true;
				}
			}

			return defaultReturn;
		}

		private static Vertex[] ScrubEarVertices(in Ear ear, in List<Vertex> vertices)
		{
			var result = new Vertex[vertices.Count];
			var earVerts = ear.vertices;
			for (int i = 0; i < vertices.Count; ++i)
			{
				Vertex vert = vertices[i];
				bool canAddVert = !earVerts.ContainsCloseEnough(in vert);
				if (canAddVert)
				{
					result[i] = vert;
				}
			}

			return result;
		}

		private static bool DoesPointLieWithinEar(Vertex point, in Ear targetEar)
		{
			// Gotta make triangle data out of these, then pass to TriangleUtility
			Debug.Assert(targetEar != null, $"Given Ear is null, please fix");
			Debug.Assert(!targetEar.vertices.IsNullOrEmpty(), $"Given Ear's vertices are null or empty, please fix");
			var targetVertices = targetEar.vertices;

			pointTriangles[0].SetVertices(point, targetVertices[0], targetVertices[1]);
			pointTriangles[1].SetVertices(point, targetVertices[1], targetVertices[2]);
			pointTriangles[2].SetVertices(point, targetVertices[2], targetVertices[0]);

			float earArea = targetEar.GetArea();
			float pointArea = 0;

			for (int i = 0; i < pointTriangles.Length; ++i)
			{
				pointArea += CalculateArea(pointTriangles[i]);
			}

			bool areasMatch = earArea.IsCloseTo(pointArea, 0.001f);
			return areasMatch;
		}


		public static float CalculateArea(in Vertex[] vertices)
		{
			return CalculateArea(in vertices[0].position, in vertices[1].position, in vertices[2].position);
		}

		private static float CalculateArea(in VertexTriangle vTriangle)
		{
			return CalculateArea(vTriangle[0].position, vTriangle[1].position, vTriangle[2].position);
		}


		// This is the SAS method of calculating a triangle's area
		// It uses 2 square roots, to get the lengths of the triangle sides. 
		private static float CalculateArea(in Vector2 first, in Vector2 second, in Vector2 third)
		{
			var edge1 = third - first;
			var edge2 = second - first;

			const float margin = 0.01f;
			bool firstHasNoMagnitude = edge1.sqrMagnitude.IsCloseTo(0, margin);
			bool secondHasNoMagnitude = edge2.sqrMagnitude.IsCloseTo(0, margin);
			Debug.Assert(!firstHasNoMagnitude && !secondHasNoMagnitude, $"Calculating area vertices are wrongly constructed, 2 of the vertices are the same, vert0.position: {first}, vert1.position: {second}, vert2.position: {third}");

			var angleBetween = Utility.ThetaBetweenVectors(in edge1, in edge2);
			var sinBetween = Mathf.Sin(angleBetween);

			var result = 0.5f * edge1.magnitude * edge2.magnitude * sinBetween;
			return result;
		}
	}
}
