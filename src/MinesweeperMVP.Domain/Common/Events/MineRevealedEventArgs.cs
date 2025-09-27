using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Common.Events;

public class MineRevealedEventArgs(GridPosition pos)
{
	public GridPosition Pos { get; set; } = pos;
}
