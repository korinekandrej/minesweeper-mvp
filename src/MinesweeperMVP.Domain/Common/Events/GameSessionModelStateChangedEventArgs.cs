using MinesweeperMVP.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Common.Events;

public class GameSessionModelStateChangedEventArgs(GameSessionState newState) : EventArgs
{
	public GameSessionState NewState { get; } = newState;
}
