using MinesweeperMVP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Highscores;

public class HighscoreModel : IModel
{
	public MinefieldSettingsModel MinefieldSettings { get; set; }
	public TimeSpan GameSessionDuration { get; set; }

	public HighscoreModel(MinefieldSettingsModel minefieldSettings, TimeSpan gameSessionDuration)
	{
		MinefieldSettings = minefieldSettings;
		GameSessionDuration = gameSessionDuration;
	}

	public override bool Equals(object? obj)
	{
		if (obj == null) return false;
		if (obj is HighscoreModel hm)
		{
			return hm.MinefieldSettings.Equals(MinefieldSettings) && hm.GameSessionDuration == GameSessionDuration;
		}
		else return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(MinefieldSettings, GameSessionDuration);
	}
}
