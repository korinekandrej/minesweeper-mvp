using MinesweeperMVP.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Highscores;

// make a generic Repository base class
public class HighscoreBoardModel : IModel
{
	public List<HighscoreModel> Highscores { get; set; }

	public HighscoreBoardModel()
	{
		Highscores = [];
	}

	// Mainly for testing, does not check if a highscore is actually the highest score
	// Remove later
	public HighscoreBoardModel ForceAdd(HighscoreModel highscore)
	{
		Highscores.Add(highscore);
		return this;
	}

	public bool TryAdd(HighscoreModel potentialHighscore)
	{
		return TryAdd(potentialHighscore, out _);
	}

	/// <summary>
	/// Potentially adds a new highscore to this repository, if it has unique MinefieldSettings, or the one highscore
	/// with matching MinefieldSettings is worse (has longer GameSessionDuration).
	/// </summary>
	/// <param name="potentialHighscore"></param>
	/// <param name="replacedHighscore"></param>
	/// <returns>The result of this operation.</returns>
	public bool TryAdd(HighscoreModel potentialHighscore, out HighscoreModel? replacedHighscore)
	{
		replacedHighscore = null;
		foreach (HighscoreModel item in Highscores)
		{
			if (item.MinefieldSettings.Equals(potentialHighscore.MinefieldSettings))
			{
				if (item.GameSessionDuration > potentialHighscore.GameSessionDuration)
				{
					// New highscore is better (shorter)
					Remove(item);
					Add(potentialHighscore);
					replacedHighscore = item;
					return true;
				}
				return false;
			}
		}
		// potentialHighscore has unique MinefieldSettings
		Highscores.Add(potentialHighscore);
		return true;
	}

	private void Add(HighscoreModel highscore)
	{
		Highscores.Add(highscore);
	}

	private void Remove(HighscoreModel highscore)
	{
		Highscores.Remove(highscore);
	}
}
