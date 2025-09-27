using MinesweeperMVP.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Common.Events;

public class SpecificTilesChangedEventArgs(IEnumerable<(GridPosition Pos, TileModel TileModel)> tileModelInfo) : EventArgs
{
	public IEnumerable<(GridPosition Pos, TileModel TileModel)> TileModelInfo { get; } = tileModelInfo;
}
