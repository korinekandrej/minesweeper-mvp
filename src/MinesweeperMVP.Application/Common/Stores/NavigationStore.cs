using MinesweeperMVP.Application.Common.Events;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Stores;

public class NavigationStore
{
	public event EventHandler<CurrentPresenterChangedEventArgs>? CurrentPresenterChanged;

	// CONSIDER: remember recent presenters, to implement a "go back" command
	//private Stack<Action<IPresenter<IView>>> _recentPresenterFactoryMethods;

	private IPresenter<IView> _currentPresenter;

	public IPresenter<IView> CurrentPresenter
	{
		get => _currentPresenter;
		set
		{
			if (_currentPresenter != value)
			{
				// Will dispose all possible subpresenters of this presenter 
				// (via a chain reaction, that should be implemented in their Dispose methods)
				// This is similar to Godot's QueueFree
				_currentPresenter?.Dispose();

				_currentPresenter = value;
				OnCurrentPresenterChanged();
			}
		}
	}

	private void OnCurrentPresenterChanged()
	{
		CurrentPresenterChanged?.Invoke(this, new CurrentPresenterChangedEventArgs(_currentPresenter));
	}
}
