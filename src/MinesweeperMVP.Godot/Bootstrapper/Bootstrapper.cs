using Godot;
using MinesweeperMVP.Application.App;
using MinesweeperMVP.Godot.Main;
using System;

namespace MinesweeperMVP.Godot.Bootstrapper;

/// <summary>
/// A node that initializes and starts the App object from MinesweeperMVP.Application project.
/// </summary>
/// <remarks>
/// It is intended to be an autoload.
/// </remarks>
public partial class Bootstrapper : Node
{
	public override void _Ready()
	{
		base._Ready();

		MainView mainScene = GetTree().CurrentScene as MainView;
		string persistenceFolder = ProjectSettings.GlobalizePath("user://");

		App app = new App(mainScene, new ViewFactory(), persistenceFolder);
		app.Start();

		QueueFree(); // Bootstrapper is no longer needed
	}
}
