using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Services;
using MinesweeperMVP.Application.Common.Stores;
using MinesweeperMVP.Application.Main;
using MinesweeperMVP.Application.Main.GameSession;
using MinesweeperMVP.Application.Main.GameSession.Minefield;
using MinesweeperMVP.Application.Main.HighscoreBoard;
using MinesweeperMVP.Application.Main.Menu;
using MinesweeperMVP.Application.Main.Menu.MinefieldSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.App;

public class App
{
	private readonly IMainView _mainView;
	private readonly IViewFactory _viewFactory;
	private readonly NavigationStore _navigationStore;
	private readonly MinefieldSettingsModel _minefieldSettingsModel;
	private readonly HighscoreBoardModel _highscoreBoard;
	private readonly PersistenceService<HighscoreBoardModel> _highscoreBoardPersistenceService;

	public App(IMainView mainView, IViewFactory viewFactory, string persistenceFolder)
	{
		_mainView = mainView;
		_viewFactory = viewFactory;

		_navigationStore = new NavigationStore();
		_minefieldSettingsModel = new MinefieldSettingsModel(9, 9, 10);

		string highscoresSaveFile = Path.Combine(persistenceFolder, "highscores.save");
		_highscoreBoardPersistenceService = new PersistenceService<HighscoreBoardModel>(highscoresSaveFile, new());
		_highscoreBoard = _highscoreBoardPersistenceService.Load();
	}

	public void Start()
	{
		// Can be discarded, as it will not be freed because of circular dependencies
		_ = new MainPresenter(_mainView, _navigationStore);

		// Set main scene after the MainPresenter i created, so it could update correctly
		_navigationStore.CurrentPresenter = CreateInitialPresenter();
	}

	// CONSIDER: I am calling it initialPRESENTER, as if the MainPresenter did not exist. Maybe rename something?
	// CONSIDER: create a PresenterFactory
	private IPresenter<IView> CreateInitialPresenter()
	{
		// By changing this factory method, you can change the "Main Scene" behaviour
		// Note that the presenters have to be robust enough to be created "out of order"
		return CreateMenuPresenter();
		//return CreateGameSessionPresenter();
		//return CreateHighscoreBoardPresenter();
	}

	private MenuPresenter CreateMenuPresenter()
	{
		IMenuView menuView = _viewFactory.Create<IMenuView>();

		// Note the unified order of parameters in the presenter constructors:
		// My View, My Model, Other Model/s, NestedPresenter/s, Service/s
		return new MenuPresenter(
			menuView,
			// Nested presenter
			new MinefieldSettingsPresenter(
				menuView.GetMinefieldSettingsView(), // WARNING: GetMinefieldSettingsView needs view to be ready
				_minefieldSettingsModel
			),
			new NavigationService(_navigationStore, CreateGameSessionPresenter),
			new NavigationService(_navigationStore, CreateHighscoreBoardPresenter)
		);
	}

	// CONSIDER: make these constructors use object initializers
	private GameSessionPresenter CreateGameSessionPresenter()
	{
		MinefieldModel minefieldModel = new(
			_minefieldSettingsModel.RowCount,
			_minefieldSettingsModel.ColCount,
			_minefieldSettingsModel.MineCount
		);
		IGameSessionView gameSessionView = _viewFactory.Create<IGameSessionView>();
		GameSessionModel gameSessionModel = new(minefieldModel);

		return new GameSessionPresenter(
			gameSessionView,
			gameSessionModel,
			new MinefieldPresenter(gameSessionView.GetMinefieldView(), minefieldModel), // Nested presenter
			new NavigationService(_navigationStore, CreateMenuPresenter),
			new NavigationService(_navigationStore, CreateGameSessionPresenter),
			new HighscoreRegistrationService(
				_highscoreBoard,
				_minefieldSettingsModel,
				gameSessionModel,
				_highscoreBoardPersistenceService
			)
		);
	}

	private HighscoreBoardPresenter CreateHighscoreBoardPresenter()
	{
		return new HighscoreBoardPresenter(
			_viewFactory.Create<IHighscoreBoardView>(),
			_highscoreBoard,
			_minefieldSettingsModel,
			new NavigationService(_navigationStore, CreateMenuPresenter),
			new NavigationService(_navigationStore, CreateGameSessionPresenter)
		);
	}
}
