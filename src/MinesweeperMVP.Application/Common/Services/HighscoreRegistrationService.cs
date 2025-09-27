using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Services;

/// <summary>
/// Encapsulates all domain objects involed in highscore registration.
/// </summary>
public class HighscoreRegistrationService
{
	private HighscoreBoardModel _highscoreBoard;
	private MinefieldSettingsModel _minefieldSettings;
	private GameSessionModel _gameSession;
	private PersistenceService<HighscoreBoardModel> _highscoreBoardPersistenceService;

	public HighscoreRegistrationService(
		HighscoreBoardModel highscoreBoard,
		MinefieldSettingsModel minefieldSettings,
		GameSessionModel gameSession,
		PersistenceService<HighscoreBoardModel> highscoreBoardPersistenceService
		)
	{
		_highscoreBoard = highscoreBoard;
		_minefieldSettings = minefieldSettings;
		_gameSession = gameSession;
		_highscoreBoardPersistenceService = highscoreBoardPersistenceService;
	}

	public bool TryRegister()
	{
		return TryRegister(out _);
	}

	/// <summary>
	/// Creates and adds a new highscore to the highscoreRepository, if the gameSession ended in success
	/// and the (possibly) previously added highscore was worse.
	/// </summary>
	/// <returns>The result of adding/not adding a highscore to the highscoreRepository.</returns>
	public bool TryRegister(out HighscoreModel? beatenHighscore)
	{
		beatenHighscore = null;
		if (_gameSession.State != GameSessionState.Success) return false;

		var result = _highscoreBoard.TryAdd(
			// Note that I am Cloning the settings
			new HighscoreModel(_minefieldSettings.Clone(), _gameSession.Duration),
			out beatenHighscore
		);

		if (result) _highscoreBoardPersistenceService.Save(_highscoreBoard);

		return result;
	}
}
