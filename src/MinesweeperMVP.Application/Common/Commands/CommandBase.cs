using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinesweeperMVP.Application.Common.Commands;

public abstract class CommandBase : ICommand
{
	public event EventHandler? CanExecuteChanged;

	public virtual bool CanExecute(object? parameter)
	{
		// TODO: implement this
		return true;
	}

	public abstract void Execute(object? parameter);

	// Special excecute, which acts as an EventHandler. The source object is discarded
	/// <summary>
	/// The Excecute method in the form of an EventHandler.
	/// </summary>
	/// <param name="_">Event's source, discarded</param>
	/// <param name="eventArgs">Event's arguments</param>
	/// <remarks>
	/// The event's source is discarded, as the Presenter should not know about View elements.
	/// </remarks>
	public void OnEventExecute(object? _, EventArgs eventArgs)
	{
		Execute(eventArgs);
	}

	protected void OnCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, new EventArgs());
	}
}
