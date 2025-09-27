using Godot;
using MinesweeperMVP.Godot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Main.GameSession;

internal partial class GameSessionView : Node, IGameSessionView
{
	public event EventHandler QuitToMenuButtonClicked;
	public event EventHandler RestartButtonClicked;

	private Label _minesLeftLabel;
	private Label _timerLabel;
	// TODO: encapsulate this into a View
	private SessionEndNotification _sessionEndNotification;

	public override void _Ready()
	{
		base._Ready();

		_minesLeftLabel = GetNode<Label>("%MinesLeftLabel");
		_timerLabel = GetNode<Label>("%TimerLabel");

		_sessionEndNotification = GetNode<SessionEndNotification>("%SessionEndNotification");

		GetNode<Button>("%QuitToMenuButton").Pressed += OnQuitToMenuButtonClicked;
		GetNode<Button>("%RestartButton").Pressed += OnRestartButtonClicked;
	}

	public IMinefieldView GetMinefieldView()
	{
		// TODO: Wait until this view is ready

		return GetNode<IMinefieldView>("%MinefieldView");
	}

	public async void UpdateMinesLeftInfo(int minesLeft)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		_minesLeftLabel.Text = $"Mines left: {minesLeft}";
	}

	public async void UpdateDurationInfo(TimeSpan duration)
	{
		// This should not be needed, but awaiting just in case
		if (!IsNodeReady()) await ToSignal(this, "ready");

		CallDeferred(nameof(UpdateDurationInfoAux), Utils.BuildTimeSpanString(duration, 4));
	}
	private void UpdateDurationInfoAux(string duration) => _timerLabel.Text = $"Timer: {duration}";


	public async void NotifyAboutSessionEnd(
		bool result,
		TimeSpan duration,
		bool beatenHighscore = false,
		TimeSpan beatenHighscoreDuration = new()
	)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		string popupText;
		if (beatenHighscore)
		{
			popupText = $"""
				New best time
				{Utils.BuildTimeSpanString(duration)}
				[{Utils.BuildTimeSpanString(beatenHighscoreDuration)}]
				""";
		}
		else popupText = $"{(result ? "You won" : "You lost")}\n{Utils.BuildTimeSpanString(duration)}";

		_sessionEndNotification.PopUp(popupText);
	}

	protected virtual void OnQuitToMenuButtonClicked()
	{
		QuitToMenuButtonClicked?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnRestartButtonClicked()
	{
		RestartButtonClicked?.Invoke(this, EventArgs.Empty);
	}
}
