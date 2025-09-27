using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperMVP.Application.Common.Services;

namespace MinesweeperMVP.Application.Common.Commands;

public class ChallengeHighscoreCommand : CommandBase
{
	// CONSIDER: should this command implement IDisposable?
	private readonly NavigateCommand _gameSessionNavigateCommand;
	private readonly ChangeMinefieldSettingsCommand _changeMinefieldSettingsCommand;

	public ChallengeHighscoreCommand(
		MinefieldSettingsModel minefieldSettings,
		NavigationService gameSessionNavigationService
	)
	{
		_gameSessionNavigateCommand = new NavigateCommand(gameSessionNavigationService);
		_changeMinefieldSettingsCommand = new ChangeMinefieldSettingsCommand(minefieldSettings);
	}

	public override void Execute(object parameter)
	{
		_changeMinefieldSettingsCommand.Execute(parameter);
		_gameSessionNavigateCommand.Execute(parameter);
	}
}
