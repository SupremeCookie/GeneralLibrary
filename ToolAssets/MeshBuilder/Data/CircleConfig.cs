using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace MeshBuilder.Data
{
	[Serializable]
	public class CircleConfig
	{
		public float radius = 5f;

		[Range(6, 64)]
		public int resolution = 12;

		public string GetID()
		{
			return $"meshID_{resolution}";
		}
	}

	public static class CircleMeshBuilder
	{
		public static Mesh Create(CircleConfig config)
		{
			Mesh result = new Mesh();

			Vector3[] vertices = new Vector3[config.resolution + 1];
			vertices[0] = Vector2.zero;

			float anglePerVert = Mathf.PI * 2f / config.resolution;
			for (int i = 0; i < config.resolution; ++i)
			{
				int index = i + 1;
				vertices[index] = new Vector3(Mathf.Cos(anglePerVert * i), 0, Mathf.Sin(anglePerVert * i)) * config.radius;
			}

			result.vertices = vertices;

			int[] triangles = new int[config.resolution * 3];
			int triangleIndex = 0;

			for (int i = 1; i < config.resolution; ++i)
			{
				triangles[triangleIndex] = 0;
				triangles[triangleIndex + 2] = i;
				triangles[triangleIndex + 1] = i + 1;

				triangleIndex += 3;
			}

			// Final triangle.
			triangles[triangleIndex] = 0;
			triangles[triangleIndex + 2] = config.resolution;
			triangles[triangleIndex + 1] = 1;

			result.triangles = triangles;

			result.RecalculateNormals();

			return result;
		}
	}
}
