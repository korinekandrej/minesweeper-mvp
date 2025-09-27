using MinesweeperMVP.Domain.Common;
using MinesweeperMVP.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Core;

public class MinefieldModel : Grid<TileModel>, IModel
{
	public event EventHandler<SpecificTilesChangedEventArgs>? SpecificTilesChanged;
	public event EventHandler<MineRevealedEventArgs>? MineRevealed;
	public event EventHandler? SuccessfullySweeped;

	private readonly int _mineCount;
	private int _flagCount;
	private int _revealedMinelessTileCount; // CONSIDER: rename this to be less confusing
	private bool _isEndConditionReached; // CONSIDER: this feels like it should belong to the GameSession

	/// <summary>
	/// The amount of mines left to be flagged.
	/// </summary>
	/// <remarks>
	/// The value represents the amount flags that the player can still place.
	/// This is because from the player's POV, they are placing flags correctly.
	/// </remarks>
	public int MinesLeft { get => _mineCount - _flagCount; }

	// CONSIDER: instead use MinefieldSettings as input
	public MinefieldModel(int rowCount, int columnCount, int mineCount) : base(rowCount, columnCount)
	{
		_mineCount = mineCount;
		_flagCount = 0;
		_revealedMinelessTileCount = 0;
		_isEndConditionReached = false;

		if (_mineCount > ItemCount) throw new ArgumentException("Minefield cannot have more mines than tiles.");

		PopulateGrid();
	}

	/// <summary>
	/// Toggle a tile's flag.
	/// </summary>
	/// <param name="pos">Position of the tile.</param>
	public void FlagTile(GridPosition pos)
	{
		if (_isEndConditionReached) return;
		if (!IsGridPositionValid(pos)) return;
		if (this[pos].IsRevealed) return;

		if (this[pos].IsFlagged)
		{
			this[pos].IsFlagged = false;
			_flagCount--;
		}
		else
		{
			if (MinesLeft == 0) return; // You can't place more flags than there is mines

			this[pos].IsFlagged = true;
			_flagCount++;
		}

		SuccessfullSweepCheck();
		OnSpecificTileChanged((pos, this[pos]));
	}

	/// <summary>
	/// Reveal a tile.
	/// </summary>
	/// <param name="pos">Position of the tile.</param>
	public void RevealTile(GridPosition pos)
	{
		if (_isEndConditionReached) return;
		if (!IsGridPositionValid(pos)) return;
		if (this[pos].IsRevealed || this[pos].IsFlagged) return;

		if (_revealedMinelessTileCount == 0) AddMines(pos); // Delayed mine initialization

		if (this[pos].HasMine) // Lose condition
		{
			_isEndConditionReached = true;
			// WARNING: the order of these would matter, if the first revealed tile could be a mine,
			// because GameSessionModel uses SpecificTilesChanged and OnMineRevealed events
			// as transition conditions in its state machine
			OnSpecificTilesChanged(RevealAllTilesWithMines());
			OnMineRevealed(pos);
			return;
		}

		List<(GridPosition, TileModel)> revealedTilePositions = [];
		TileChainReveal(pos, revealedTilePositions);
		OnSpecificTilesChanged(revealedTilePositions.ToArray());

		_revealedMinelessTileCount += revealedTilePositions.Count;
		SuccessfullSweepCheck(); // Win condition
	}

	private void SuccessfullSweepCheck()
	{
		if (_revealedMinelessTileCount + _flagCount == ItemCount) // Don't need to check if MinesLeft is 0
		{
			_isEndConditionReached = true;
			OnSuccessfullySweeped();
		}
	}

	private (GridPosition, TileModel)[] RevealAllTilesWithMines()
	{
		TileModel tile;
		List<(GridPosition, TileModel)> tilesWithMines = [];

		for (int row = 0; row < RowCount; row++)
		{
			for (int column = 0; column < ColumnCount; column++)
			{
				tile = this[row, column];
				if (tile.HasMine)
				{
					tile.IsRevealed = true;
					tilesWithMines.Add((new(row, column), tile));
				}
			}
		}
		return tilesWithMines.ToArray();
	}

	//TODO: maybe make this iterative with a queue and a hashset(for visited tiles)
	private void TileChainReveal(GridPosition pos, in List<(GridPosition, TileModel)> revealedTilePositions)
	{
		this[pos].IsRevealed = true;
		revealedTilePositions.Add((pos, this[pos]));

		if (this[pos].AdjacentMineCount != 0) return;

		foreach (GridPosition adjTilePos in AdjacentItemPositions(pos))
		{
			if (this[adjTilePos].IsRevealed) continue; // Duplicate check, maybe unavoidable

			// When such tile was incorrectly flagged by the user beforehand, unflag it
			if (this[adjTilePos].IsFlagged) FlagTile(adjTilePos);

			TileChainReveal(adjTilePos, revealedTilePositions);
		}
	}

	private void PopulateGrid()
	{
		for (int i = 0; i < ItemCount; i++)
		{
			this[i] = new TileModel();
		}
	}

	private void AddMines(GridPosition initRevealedPos)
	{
		// Special case, unable to make the first revealed tile to not have a mine
		if (RowCount * ColumnCount == _mineCount)
		{
			for (int i = 0; i < _mineCount; i++) AddMine(i);
			return;
		}

		// Exclude the first revealed tile from the possibility of having a mine
		int[] exclusions = [GridPositionToIndex(initRevealedPos)];
		IEnumerable<int> randUniqNums = Utils.RandomUniqueNumbers(0, ItemCount - 1, _mineCount, exclusions);

		foreach (int randUniqNum in randUniqNums)
		{
			AddMine(randUniqNum);
		}
	}

	private void AddMine(int index)
	{
		if (this[index].HasMine) throw new InvalidOperationException("Cannot add a mine to a Tile which already has one.");

		this[index].HasMine = true;
		foreach (TileModel adjacentTile in AdjacentItems(index))
		{
			adjacentTile.AdjacentMineCount++;
		}
	}

	protected virtual void OnSpecificTileChanged((GridPosition Pos, TileModel Tile) changedTile)
	{
		OnSpecificTilesChanged([changedTile]);
	}

	protected virtual void OnSpecificTilesChanged((GridPosition Pos, TileModel Tile)[] changedTiles)
	{
		SpecificTilesChanged?.Invoke(this, new(changedTiles));
	}

	protected virtual void OnMineRevealed(GridPosition pos)
	{
		MineRevealed?.Invoke(this, new(pos));
	}

	protected virtual void OnSuccessfullySweeped()
	{
		SuccessfullySweeped?.Invoke(this, EventArgs.Empty);
	}

	public override string ToString()
	{
		GridPosition pos = new();
		StringBuilder result = new();
		for (int row = 0; row < RowCount; row++)
		{
			for (int column = 0; column < ColumnCount; column++)
			{
				result.Append(this[row, column].ToString());
			}
			result.Append('\n');
		}
		return result.ToString();
	}
}
