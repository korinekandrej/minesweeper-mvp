using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Presentation.HighscoreBoard;

public partial class HighscoreBoardView : Node, IHighscoreBoardView
{
	public event EventHandler BackToMenuButtonClicked;
	public event EventHandler<ChallengeButtonClickedEventArgs> ChallengeButtonClicked;

	private VBoxContainer _highscoreList;
	private PackedScene _highscoreListingItemPackedScene;

	public override void _Ready()
	{
		base._Ready();

		_highscoreList = GetNode<VBoxContainer>("%HighscoreList");

		GetNode<Button>("%BackToMenuButton").Pressed += OnBackToMenuButtonClicked;

		// CONSIDER: beware or magic uid
		_highscoreListingItemPackedScene = (PackedScene)ResourceLoader.Load("uid://bw5foifcm1e07");
	}

	public async void AddHighscore(int rowCount, int colCount, int mineCount, TimeSpan duration)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		HighscoreBoardViewItem highscoreListingItem = _highscoreListingItemPackedScene.Instantiate<HighscoreBoardViewItem>();
		_highscoreList.AddChild(highscoreListingItem);
		highscoreListingItem.SetValues(rowCount, colCount, mineCount, duration);
		highscoreListingItem.ChallengeButtonClicked += (o, e) => OnChallengeButtonClicked(rowCount, colCount, mineCount);
	}

	private void OnBackToMenuButtonClicked()
	{
		BackToMenuButtonClicked?.Invoke(this, EventArgs.Empty);
	}

	private void OnChallengeButtonClicked(int rowCount, int colCount, int mineCount)
	{
		ChallengeButtonClicked?.Invoke(this, new(rowCount, colCount, mineCount));
	}
}
