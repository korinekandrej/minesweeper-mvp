using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Services;

// CONSIDER: make an INavigationService interface
public class NavigationService
{
	private readonly NavigationStore _navigationStore;
	private readonly Func<IPresenter<IView>> _createPresenter;

	public NavigationService(NavigationStore navigationStore, Func<IPresenter<IView>> createPresenter)
	{
		_navigationStore = navigationStore;
		_createPresenter = createPresenter;
	}

	public void Navigate()
	{
		_navigationStore.CurrentPresenter = _createPresenter();
	}
}
