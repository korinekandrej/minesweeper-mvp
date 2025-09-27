using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Events;

public class MinefieldSettingsChangedEventArgs(int rowCount, int colCount, int mineCount) : EventArgs
{
	public int RowCount { get; } = rowCount;
	public int ColCount { get; } = colCount;
	public int MineCount { get; } = mineCount;
}
