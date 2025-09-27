using MinesweeperMVP.Application.Common.Commands;
using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Services;
using MinesweeperMVP.Application.Main.Menu.MinefieldSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinesweeperMVP.Application.Main.Menu;

public class MenuPresenter : IPresenter<IMenuView>
{
	private readonly MinefieldSettingsPresenter _minefieldSettingsPresenter;
	private readonly NavigateCommand _gameSessionNavigateCommand;
	private readonly NavigateCommand _highscoreListingNavigateCommand;

	public IMenuView View { get; private set; }

	public MenuPresenter(
		IMenuView menuView,
		MinefieldSettingsPresenter minefieldSettingsPresenter,
		NavigationService gameSessionNavigationService,
		NavigationService highscoreListingNavigationStore
	)
	{
		_minefieldSettingsPresenter = minefieldSettingsPresenter;
		_gameSessionNavigateCommand = new NavigateCommand(gameSessionNavigationService);
		_highscoreListingNavigateCommand = new NavigateCommand(highscoreListingNavigationStore);

		View = menuView;
		// Note that starting a session can be a simple NavigateCommand,
		// as the actual GameSessionModel creation is tied to the creation of its Presenter (see App class).
		// The GameSessionModel cannot exists without its Presenter.
		View.StartGameButtonClicked += _gameSessionNavigateCommand.OnEventExecute;
		View.HighscoresButtonClicked += _highscoreListingNavigateCommand.OnEventExecute;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		View.StartGameButtonClicked -= _gameSessionNavigateCommand.OnEventExecute;
		View.HighscoresButtonClicked -= _highscoreListingNavigateCommand.OnEventExecute;

		_minefieldSettingsPresenter.Dispose();
	}
}
