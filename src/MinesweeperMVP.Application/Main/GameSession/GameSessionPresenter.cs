using MinesweeperMVP.Application.Common.Commands;
using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Services;
using MinesweeperMVP.Application.Main.GameSession.Minefield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace MinesweeperMVP.Application.Main.GameSession;

public class GameSessionPresenter : IPresenterWithModel<IGameSessionView, GameSessionModel>
{
	private readonly NavigateCommand _menuNavigateCommand;
	private readonly NavigateCommand _gameSessionNavigateCommand;
	private readonly HighscoreRegistrationService _highscoreRegistrationService;
	private readonly MinefieldPresenter _minefieldPresenter;
	private readonly Timer _durationUpdateTimer;
	private readonly Dictionary<GameSessionState, Action> _modelEnterStateActionByModelNewState;

	public IGameSessionView View { get; private set; }
	public GameSessionModel Model { get; private set; }

	public GameSessionPresenter(
		IGameSessionView gameSessionView,
		GameSessionModel gameSessionModel,
		MinefieldPresenter minefieldPresenter, // Nested presenter
		NavigationService menuNavigationService, // Services ↓
		NavigationService gameSessionNavigationService,
		HighscoreRegistrationService highscoreRegisterService
	)
	{
		_menuNavigateCommand = new NavigateCommand(menuNavigationService);
		_gameSessionNavigateCommand = new NavigateCommand(gameSessionNavigationService);
		_highscoreRegistrationService = highscoreRegisterService;
		_minefieldPresenter = minefieldPresenter;
		_modelEnterStateActionByModelNewState = new()
		{
			{GameSessionState.Playing, OnModelPlayingStateEntered},
			{GameSessionState.Success, OnModelSuccessStateEntered},
			{GameSessionState.Failure, OnModelFailureStateEntered},
		};


		View = gameSessionView;
		Model = gameSessionModel;

		View.QuitToMenuButtonClicked += _menuNavigateCommand.OnEventExecute;
		View.RestartButtonClicked += _gameSessionNavigateCommand.OnEventExecute;

		// CONSIDER: maybe not grab things from the MinefieldModel? It is not pretty and not encapsulated
		// BUT is there a better solution?
		View.UpdateMinesLeftInfo(Model.MinefieldModel.MinesLeft);
		Model.MinefieldModel.SpecificTilesChanged += OnModelSpecificTilesChanged;

		Model.StateChanged += OnModelStateChanged;

		// Note that the timer is not in the model,
		// as model should not care how often the presenter updates its duration displayed in the view
		// Also note that it has to be update more often than a second (if I want to display seconds)
		// as it is not synchronized with the Model's stopwatch (and would show duplicate values sometimes)
		// CONSIDER: maybe fix that
		_durationUpdateTimer = new Timer(100);
		_durationUpdateTimer.Elapsed += OnDurationUpdateTimerElapsed;
	}

	private void OnModelSpecificTilesChanged(object? o, EventArgs e)
	{
		View.UpdateMinesLeftInfo(Model.MinefieldModel.MinesLeft);
	}

	private void OnModelStateChanged(object? o, GameSessionModelStateChangedEventArgs e)
	{
		_modelEnterStateActionByModelNewState[e.NewState].Invoke();
	}

	private void OnModelPlayingStateEntered()
	{
		_durationUpdateTimer.Start();
	}

	private void OnModelSuccessStateEntered()
	{
		_durationUpdateTimer.Stop();

		_highscoreRegistrationService.TryRegister(out HighscoreModel? beatenHighscore);
		if (beatenHighscore != null)
		{
			View.NotifyAboutSessionEnd(
				true,
				Model.Duration,
				true,
				beatenHighscore.GameSessionDuration
			);
		}
		else View.NotifyAboutSessionEnd(true, Model.Duration);
	}

	private void OnModelFailureStateEntered()
	{
		_durationUpdateTimer.Stop();
		View.NotifyAboutSessionEnd(false, Model.Duration);
	}

	private void OnDurationUpdateTimerElapsed(object? o, EventArgs e)
	{
		View.UpdateDurationInfo(Model.Duration);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		// 1.) DISPOSE self:
		View.QuitToMenuButtonClicked -= _menuNavigateCommand.OnEventExecute;
		View.RestartButtonClicked -= _gameSessionNavigateCommand.OnEventExecute;

		Model.MinefieldModel.SpecificTilesChanged -= OnModelSpecificTilesChanged;
		Model.StateChanged -= OnModelStateChanged;

		_durationUpdateTimer.Dispose();

		// 2.) DISPOSE sideways:
		// This is a specific example of a presenter needing to dispose its model:
		// a) The model, GameSession, has a reference to another model, Minefield, 
		//	   AND it is also subscribed to Minefield's events
		// b) The model, GameSession, exists only when its presenter, this, exists
		Model.Dispose();

		// 3.) DISPOSE down:
		_minefieldPresenter.Dispose();
	}
}
