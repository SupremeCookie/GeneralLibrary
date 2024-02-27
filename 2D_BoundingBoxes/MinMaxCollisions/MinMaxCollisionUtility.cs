using System.Collections.Generic;
using UnityEngine;

public static class MinMaxCollisionUtility
{
	/// <summary>
	/// Checks if a MinMax object is entirely contained by cell points, and has no holes inside.
	/// </summary>
	/// <param name="cellPointSize"> cellPoint size is needed in order to form a volumetric grid, without volume, we can't check for holes </param>
	public static bool IsContainedByPoints(this MinMax boundingBox, ref List<Vector2> cellPoints, float cellPointSize)
	{
		// Note DK: We first convert the cellpoints into a list of greedy meshes.
		// The Greedy Meshes are rectangles where no holes are present in the rectangle. We can easily test for inclusion using existing minmax logic there.
		var greedyMeshes = BoundingBoxConverter.Convert(cellPoints, cellPointSize);

		return IsContainedByBoxes(boundingBox, in greedyMeshes);
	}

	public static bool IsContainedByBoxes(MinMax originalBBox, in List<MinMax> containmentBoxes)
	{
		List<MinMax> overlappingBBoxes = new List<MinMax>();
		for (int i = 0; i < containmentBoxes.Count; ++i)
		{
			var cellBBox = containmentBoxes[i];
			bool hasOverlap = cellBBox.IsOverlapping(originalBBox);

			if (hasOverlap)
			{
				var overlapRegion = cellBBox.GetOverlappingArea(originalBBox);
				overlappingBBoxes.Add(overlapRegion);
			}
		}

		return AreSurfaceAreasTheSame(originalBBox, in overlappingBBoxes);
	}

	public static bool AreSurfaceAreasTheSame(MinMax originalBBox, in List<MinMax> overlaps)
	{
		float mainBoxArea = originalBBox.GetSurfaceArea();

		float testingAreas = 0;
		for (int i = 0; i < overlaps.Count; ++i)
		{
			testingAreas += overlaps[i].GetSurfaceArea();
		}

		if (mainBoxArea.IsCloseTo(testingAreas, 0.001f))
		{
			return true;
		}

		return false;
	}
}