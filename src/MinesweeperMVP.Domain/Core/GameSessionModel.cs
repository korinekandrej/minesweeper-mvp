using MinesweeperMVP.Domain.Common;
using MinesweeperMVP.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Core;

public enum GameSessionState
{
	InitPaused,
	Playing,
	Success,
	Failure
}

public class GameSessionModel : IModel, IDisposable
{
	public event EventHandler<GameSessionModelStateChangedEventArgs>? StateChanged;

	public GameSessionState _state;
	public Stopwatch _durationStopwatch;

	public MinefieldModel MinefieldModel { get; private set; }
	public TimeSpan Duration => _durationStopwatch.Elapsed;

	public GameSessionState State
	{
		get => _state;
		set
		{
			// A very simple enum state machine
			if (_state != value)
			{
				if (_state == GameSessionState.Playing) OnPlayingStateExited();
				_state = value;
				if (_state == GameSessionState.Playing) OnPlayingStateEntered();
				OnStateChanged(_state);
			}
		}
	}

	public GameSessionModel(MinefieldModel minefieldModel)
	{
		MinefieldModel = minefieldModel;
		_durationStopwatch = new Stopwatch();

		State = GameSessionState.InitPaused;

		MinefieldModel.SpecificTilesChanged += EnterPlaying;
		MinefieldModel.MineRevealed += EnterFailure;
		MinefieldModel.SuccessfullySweeped += EnterSuccess;
	}

	private void EnterPlaying(object? o, EventArgs e) // First tile revealed
	{
		State = GameSessionState.Playing;
		MinefieldModel.SpecificTilesChanged -= EnterPlaying;
	}

	private void EnterFailure(object? o, MineRevealedEventArgs e)
	{
		State = GameSessionState.Failure;
		MinefieldModel.MineRevealed -= EnterFailure;
		MinefieldModel.SuccessfullySweeped -= EnterSuccess;
	}

	private void EnterSuccess(object? o, EventArgs e)
	{
		State = GameSessionState.Success;
		MinefieldModel.MineRevealed -= EnterFailure;
		MinefieldModel.SuccessfullySweeped -= EnterSuccess;
	}

	private void OnPlayingStateEntered() => _durationStopwatch.Start();

	private void OnPlayingStateExited() => _durationStopwatch.Stop();

	protected virtual void OnStateChanged(GameSessionState newState)
	{
		StateChanged?.Invoke(this, new(newState));
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		MinefieldModel.SpecificTilesChanged -= EnterPlaying;
		MinefieldModel.MineRevealed -= EnterFailure;
		MinefieldModel.SuccessfullySweeped -= EnterSuccess;

		_durationStopwatch.Stop();
	}
}
