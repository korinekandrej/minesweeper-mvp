using MinesweeperMVP.Application.Common.Events;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.HighscoreBoard;

public interface IHighscoreBoardView : IView
{
	public event EventHandler? BackToMenuButtonClicked;
	public event EventHandler<ChallengeButtonClickedEventArgs>? ChallengeButtonClicked;

	public void AddHighscore(int rowCount, int colCount, int mineCount, TimeSpan duration);
}
