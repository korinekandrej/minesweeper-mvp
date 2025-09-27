using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Main.GameSession.Minefield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.GameSession;

public interface IGameSessionView : IView
{
	public event EventHandler? QuitToMenuButtonClicked;
	public event EventHandler? RestartButtonClicked;

	public IMinefieldView GetMinefieldView();
	public void UpdateMinesLeftInfo(int minesLeft);
	public void UpdateDurationInfo(TimeSpan duration);
	public void NotifyAboutSessionEnd(
		bool result,
		TimeSpan duration,
		bool beatenHighscore = false,
		TimeSpan beatenHighscoreDuration = new()
		);
}
