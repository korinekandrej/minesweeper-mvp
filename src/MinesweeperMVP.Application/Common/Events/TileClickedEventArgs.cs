using MinesweeperMVP.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Events;

public class TileClickedEventArgs(MouseButton button, GridPosition tilePos) : EventArgs
{
	public MouseButton Button { get; } = button; // Note that this is not Godot's MouseButton
	public GridPosition TilePos { get; } = tilePos;
}
