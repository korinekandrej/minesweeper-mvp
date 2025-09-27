using MinesweeperMVP.Application.Common.Events;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.GameSession.Minefield;

public interface IMinefieldView : IView
{
	// CONSIDER: Move this somewhere else?
	public enum NonNumTileContent
	{
		// Numbers 0 - 8 represent revealed tiles with a number ("NumericTileContent")
		Flag = 9,
		Unrevealed = 10,
		Mine = 11,
		MineRed = 12,
	}

	public event EventHandler<TileClickedEventArgs>? TileClicked;

	public void InitializeTiles(int rowCount, int colCount);
	public void UpdateSpecificTiles(IEnumerable<(GridPosition Pos, int TileContent)> tileContentInfo);
}
