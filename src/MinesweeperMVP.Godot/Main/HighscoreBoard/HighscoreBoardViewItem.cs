using Godot;
using MinesweeperMVP.Godot.Common;
using System;

namespace MinesweeperMVP.Presentation.HighscoreBoard;

public partial class HighscoreBoardViewItem : PanelContainer
{
	public event EventHandler ChallengeButtonClicked;

	private Label _sizeLabel;
	private Label _mineCountLabel;
	private Label _durationLabel;
	private Button _challengeButton;

	public override void _Ready()
	{
		base._Ready();
		_sizeLabel = GetNode<Label>("%SizeLabel");
		_mineCountLabel = GetNode<Label>("%MineCountLabel");
		_durationLabel = GetNode<Label>("%DurationLabel");
		GetNode<Button>("%ChallengeButton").Pressed += OnChallengeButtonClicked;
	}

	public void SetValues(
		int rowCount,
		int colCount,
		int mineCount,
		TimeSpan duration
		)
	{
		_sizeLabel.Text = $"{rowCount} x {colCount}";
		_mineCountLabel.Text = $"{mineCount} mine{(mineCount>1 ? "s" : "")}";
		_durationLabel.Text = Utils.BuildTimeSpanString(duration);
	}

	protected virtual void OnChallengeButtonClicked()
	{
		ChallengeButtonClicked?.Invoke(this, EventArgs.Empty);
	}
}
