using Godot;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Main;

public partial class MainView : Node, IMainView
{
	private Node _currentView;
	private ColorRect _dissolveRect;
	private CanvasLayer _screenViewLayer;
	private CanvasLayer _viewTransition;

	private float transitionDuration;

	public MainView()
	{
		transitionDuration = 0.3f;
	}

	public override void _Ready()
	{
		base._Ready();
		_screenViewLayer = GetNode<CanvasLayer>("%ScreenViewLayer");
		_viewTransition = GetNode<CanvasLayer>("%ViewTransition");
		_dissolveRect = GetNode<ColorRect>("%DissolveRect");

		// To always be in the front
		// Note that tha max value as seen in the editor seem to be 128
		_viewTransition.Layer = int.MaxValue;

		/*
		_dissolveRect.Modulate = new Color(
			_dissolveRect.Modulate.R,
			_dissolveRect.Modulate.G,
			_dissolveRect.Modulate.B,
			1);*/
	}

	// CONSIDER: rename to SwitchScreenViewTo
	public async void SwitchSubViewTo(IView targetView /*, <type> transitionType*/)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		if (targetView is not Node)
		{
			throw new ArgumentException("The argument 'view' is an IView, but not a Node.", nameof(targetView));
		}

		// Simple fade out/in transition
		// TODO: allow for different transitions
		// BUG: during transition, if you spam a button, errors pop up (CONSIDER: disable UI during transition. But how?)
		void SwitchSubViewToAux(IView targetView)
		{
			_currentView?.QueueFree();
			_currentView = targetView as Node;
			_screenViewLayer.AddChild(targetView as Node);
		}

		void FadeInNewView()
		{
			SwitchSubViewToAux(targetView);
			AlphaTweener(_dissolveRect, 0f, transitionDuration / 2).Play();
		}

		if (_currentView != null)
		{
			Tween tween = AlphaTweener(_dissolveRect, 1f, transitionDuration / 2); // Fades in a rectangle which blocks
			tween.Finished += FadeInNewView;
			tween.Play();
		}
		// BUG: At the start, there is a small duration, when the godot logo is shown, but the tweener was already run,
		// so the player does not see the transition
		else FadeInNewView();
	}

	private static Tween AlphaTweener(CanvasItem canvasItem, float finalAlpha, float duration)
	{
		Tween tween = canvasItem.GetTree().CreateTween();
		tween.TweenProperty(canvasItem, "modulate:a", finalAlpha, duration);
		return tween;
	}
}
