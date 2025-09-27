using MinesweeperMVP.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Core;

// Does not need to be a model
public class Grid<T> : IEnumerable<T>
{
	public T[] Items;
	public int RowCount { get; }
	public int ColumnCount { get; }
	public int ItemCount { get => Items.Length; }

	public Grid(int rowCount, int columnCount)
	{
		RowCount = rowCount;
		ColumnCount = columnCount;

		Items = new T[RowCount * ColumnCount];
	}

	public IEnumerator<T> GetEnumerator()
	{
		foreach (var item in Items) yield return item;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	// This is how 3 rows x 4 columns Grid is indexed:
	//   0123
	//   ||||
	// 0-0369
	// 1-147A
	// 2-258B  (where A=10, B=11)
	// Index: 2 => GridPosition: Row 2, Column 0
	public T this[int row, int column]
	{
		get => Items[GridPositionToIndex(new(row, column))];
		set => Items[GridPositionToIndex(new(row, column))] = value;
	}

	public T this[GridPosition pos]
	{
		get => Items[GridPositionToIndex(pos)];
		set => Items[GridPositionToIndex(pos)] = value;
	}

	public T this[int index]
	{
		get => Items[index];
		set => Items[index] = value;
	}

	protected int GridPositionToIndex(GridPosition pos)
	{
		return pos.Row * ColumnCount + pos.Column;
	}

	protected GridPosition IndexToGridPosition(int index)
	{
		return new GridPosition(index / ColumnCount, index % ColumnCount);
	}

	protected bool IsRowValid(int row)
	{
		return 0 <= row && row <= RowCount - 1;
	}

	protected bool IsColumnValid(int column)
	{
		return 0 <= column && column <= ColumnCount - 1;
	}

	protected bool IsGridPositionValid(GridPosition pos)
	{
		return IsRowValid(pos.Row) && IsColumnValid(pos.Column);
	}

	protected IEnumerable<T> AdjacentItems(int index)
	{
		return AdjacentItems(IndexToGridPosition(index));
	}

	protected IEnumerable<T> AdjacentItems(GridPosition pos)
	{
		foreach (GridPosition adjItemPos in AdjacentItemPositions(pos))
		{
			yield return this[adjItemPos];
		}
	}

	protected IEnumerable<GridPosition> AdjacentItemPositions(GridPosition pos)
	{
		GridPosition adjItemPos = new();

		foreach (int[] vector in Utils.PermutationsRep([-1, 0, 1], 2))
		{
			if (vector[0] == 0 && vector[1] == 0) continue; // vector (0,0) points to the inputted position

			adjItemPos.Row = pos.Row + vector[0];
			adjItemPos.Column = pos.Column + vector[1];

			if (!IsGridPositionValid(adjItemPos)) continue;

			yield return adjItemPos;
		}
	}
}
