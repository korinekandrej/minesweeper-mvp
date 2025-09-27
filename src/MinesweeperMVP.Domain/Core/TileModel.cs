using MinesweeperMVP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Core;

public class TileModel : IModel
{
	public bool IsFlagged { get; set; }
	public bool IsRevealed { get; set; }
	public bool HasMine { get; set; }
	public byte AdjacentMineCount { get; set; }

	public TileModel()
	{
		IsFlagged = false;
		IsRevealed = false;
		HasMine = false;
		AdjacentMineCount = 0;
	}

	// For testing
	public override string ToString()
	{
		if (IsRevealed)
		{
			if (HasMine) return "M";
			return AdjacentMineCount == 0 ? "." : AdjacentMineCount.ToString();
		}
		if (IsFlagged) return "F";
		return "#";
	}
}
