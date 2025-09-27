using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Main.Menu;

public partial class MenuView : Node, IMenuView
{
	public event EventHandler StartGameButtonClicked;
	public event EventHandler HighscoresButtonClicked;

	public IMinefieldSettingsView GetMinefieldSettingsView()
	{
		// Cannot wait until "ready", as making this method asynchronous will change its signature
		//if (!IsNodeReady()) await ToSignal(this, "ready");

		return GetNode<IMinefieldSettingsView>("%MinefieldSettingsView");
	}

	public override void _Ready()
	{
		base._Ready();

		// Note that when a node subscribes to its child's event,
		// it does not need to manually unsubscribe to be freed from memory
		// because QueueFree takes care of it
		GetNode<Button>("%StartGameButton").Pressed += OnStartGameButtonClicked;
		GetNode<Button>("%HighscoresButton").Pressed += OnHighscoresButtonClicked;
	}

	public void OnStartGameButtonClicked()
	{
		StartGameButtonClicked?.Invoke(this, EventArgs.Empty);
	}

	public void OnHighscoresButtonClicked()
	{
		HighscoresButtonClicked?.Invoke(this, EventArgs.Empty);
	}
}
