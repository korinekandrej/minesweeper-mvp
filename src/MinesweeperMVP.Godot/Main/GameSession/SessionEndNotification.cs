using Godot;
using System;

namespace MinesweeperMVP.Godot.Main.GameSession;

public partial class SessionEndNotification : MarginContainer
{
	private Label _label;

	public SessionEndNotification()
	{
		// Allows player to optionally see the Minefield fully
		MouseEntered += () => Modulate = new Color(1, 1, 1, 0.2f);
		MouseExited += () => Modulate = new Color(1, 1, 1, 1f);
	}

	public override void _Ready()
	{
		base._Ready();
		_label = GetNode<Label>("%Label");
	}

	public async void PopUp(string text)
	{
		if (!IsNodeReady()) await ToSignal(this, nameof(Ready));

		Show();
		_label.Text = text;
	}
}
