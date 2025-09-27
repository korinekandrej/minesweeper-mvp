using MinesweeperMVP.Application.Common.Commands;
using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.HighscoreBoard;

// TODO: rename to HighscoreBoardPresenter
public class HighscoreBoardPresenter : IPresenterWithModel<IHighscoreBoardView, HighscoreBoardModel>
{
	private readonly NavigateCommand _menuNavigateCommand;
	private readonly ChallengeHighscoreCommand _challengeHighscoreCommand;

	public IHighscoreBoardView View { get; set; }
	public HighscoreBoardModel Model { get; }

	public HighscoreBoardPresenter(
		IHighscoreBoardView highscoreListingView,
		HighscoreBoardModel highscoreBoardModel,
		MinefieldSettingsModel minefieldSettings,
		NavigationService menuNavigationService,
		NavigationService gameSessionNavigationService
	)
	{
		_menuNavigateCommand = new NavigateCommand(menuNavigationService);
		_challengeHighscoreCommand = new ChallengeHighscoreCommand(minefieldSettings, gameSessionNavigationService);

		View = highscoreListingView;
		Model = highscoreBoardModel;

		View.BackToMenuButtonClicked += _menuNavigateCommand.OnEventExecute;
		View.ChallengeButtonClicked += _challengeHighscoreCommand.OnEventExecute;

		foreach (var item in Model.Highscores)
		{
			// TODO: rename this more appropriately
			View.AddHighscore(
				item.MinefieldSettings.RowCount,
				item.MinefieldSettings.ColCount,
				item.MinefieldSettings.MineCount,
				item.GameSessionDuration
			);
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		View.BackToMenuButtonClicked -= _menuNavigateCommand.OnEventExecute;
		View.ChallengeButtonClicked -= _challengeHighscoreCommand.OnEventExecute;
	}
}
