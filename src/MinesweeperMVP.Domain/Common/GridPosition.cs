using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Common;

public struct GridPosition
{
	public int Row { get; set; }
	public int Column { get; set; }

	public GridPosition() { }

	public GridPosition(int row, int column)
	{
		Row = row;
		Column = column;
	}
}
