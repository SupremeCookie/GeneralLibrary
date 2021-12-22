using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BoundingBoxConverter
{
	// Note DK: We use an algorithm called Greedy-Meshing.
	public static List<MinMax> Convert(List<Vector2> cells, float cellSize)
	{
		var copyOfCells = new List<Vector2>(cells);
		copyOfCells.Sort(CompareVector2OnXThenY());  // Note DK: This sorts the array on the x axis, then the y axis. So we start at bottom left most item.

		var result = new List<MinMax>();

		bool isChecking = false;
		bool isFillingRow = false;
		List<Vector2> currentRow = null;
		List<Vector2> minMaxCells = null;

		int internalIterations = 0;
		while (copyOfCells.Count > 0)
		{
			if (!isChecking)
			{
				isChecking = true;
				currentRow = new List<Vector2>();
				isFillingRow = true;

				currentRow.Add(copyOfCells[0]);
				copyOfCells.RemoveAt(0);
			}

			bool stoppedFindingColumns = false;

			while (isFillingRow)
			{
				var currentRowEndCell = currentRow[currentRow.Count - 1];
				var targetPos = currentRowEndCell + new Vector2(cellSize, 0);   // Note DK: Filling rows, means we're moving right on the x axis

				int indexOfCell = FindCell(targetPos, copyOfCells);
				if (indexOfCell >= 0)
				{
					currentRow.Add(copyOfCells[indexOfCell]);
					copyOfCells.RemoveAt(indexOfCell);
				}
				else
				{
					isFillingRow = false;

					if (minMaxCells == null) { minMaxCells = new List<Vector2>(); }

					minMaxCells.AddRange(currentRow);
				}
			}

			if (!isFillingRow)
			{
				// If we aren't filling a row, we are checking if the last made row, can move upwards one in the y.
				var lastRow = new List<Vector2>(currentRow);
				currentRow.Clear();

				List<int> newRowIndexes = new List<int>(lastRow.Count);

				for (int i = 0; i < lastRow.Count; ++i)
				{
					var currentPos = lastRow[i];
					var targetPos = currentPos + new Vector2(0, cellSize);  // Note DK: Filling collums, means we're moving upward on the y axis

					int indexOfCell = FindCell(targetPos, copyOfCells);
					if (indexOfCell >= 0)
					{
						newRowIndexes.Add(indexOfCell);
					}
				}

				// Note DK: First we get the data for the new cells. Then we check if its valid, if so, we add the cells to the currentRow.
				bool countsAreTheSame = newRowIndexes.Count == lastRow.Count;
				bool noDuplicateIndexes = HasAnyDuplicates(newRowIndexes);

				if (countsAreTheSame && !noDuplicateIndexes)
				{
					for (int i = 0; i < newRowIndexes.Count; ++i)
					{
						currentRow.Add(copyOfCells[newRowIndexes[i]]);
					}

					newRowIndexes.Sort();

					for (int i = newRowIndexes.Count - 1; i >= 0; --i)
					{
						copyOfCells.RemoveAt(newRowIndexes[i]);
					}


					minMaxCells.AddRange(currentRow);
				}
				else
				{
					stoppedFindingColumns = true;
				}
			}


			// Note DK: We store the current minMaxCells if there are no more cells, or we stopped making a shape, because our current column can't fit another row in.
			bool shouldStoreCells = stoppedFindingColumns || copyOfCells.Count == 0;
			if (shouldStoreCells)
			{
				var resultingMinMax = ConvertToMinMax(minMaxCells, cellSize);
				result.Add(resultingMinMax);

				isChecking = false;
				minMaxCells = null;
				currentRow = null;
			}


			internalIterations++;
			if (internalIterations > cells.Count)
			{
				Debug.Assert(false, "Went over internalIterations limit");
				break;
			}
		}


		return result;
	}

	public static List<MinMax> Convert(Vector2[] cells, float cellSize)
	{
		return Convert(cells.ToList(), cellSize);
	}



	private static int FindCell(Vector2 targetPos, List<Vector2> cells)
	{
		if (cells.IsNullOrEmpty())
		{
			return -1;
		}

		for (int i = 0; i < cells.Count; ++i)
		{
			if (cells[i].IsCloseTo(targetPos, 0.001f))
			{
				return i;
			}
		}

		return -1;
	}

	private static MinMax ConvertToMinMax(List<Vector2> cellPoints, float cellSize)
	{
		// Note DK: The cell points are only concerned about the central points.
		// So for the min and max values, we need to take into account the cell sizes.

		MinMax result = new MinMax();
		if (cellPoints.Count == 0)
		{
			return result;
		}

		for (int i = 0; i < cellPoints.Count; ++i)
		{
			result.TryStoreNewMinOrMax(cellPoints[i]);
		}

		result.Min -= Vector2.one * cellSize * 0.5f;    // Note DK: Half a cell to the bottom left.
		result.Max += Vector2.one * cellSize * 0.5f;    // Note DK: Half a cell to the top right.

		return result;
	}

	private static bool HasAnyDuplicates(List<int> indexes)
	{
		if (indexes.IsNullOrEmpty())
		{
			return false;
		}

		List<int> existingIntegers = new List<int>(indexes.Count);
		for (int i = 0; i < indexes.Count; ++i)
		{
			if (!existingIntegers.Contains(indexes[i]))
			{
				existingIntegers.Add(indexes[i]);
			}
			else
			{
				return true;
			}
		}

		return false;
	}





	private static IComparer<Vector2> CompareVector2OnXThenY()
	{
		return new CompareVector2OnXThenYComparer();
	}

	private class CompareVector2OnXThenYComparer : IComparer<Vector2>
	{
		public int Compare(Vector2 first, Vector2 second)
		{
			if (first.x < second.x)
			{
				return -1;
			}
			else if (first.x > second.x)
			{
				return 1;
			}

			if (first.x == second.x)
			{
				if (first.y < second.y)
				{
					return -1;
				}
				else if (first.y > second.y)
				{
					return 1;
				}
			}

			return 0;
		}
	}
}