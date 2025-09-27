using MinesweeperMVP.Application.Common.Events;
using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Common.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main;

public class MainPresenter : IPresenter<IMainView>
{
	private readonly NavigationStore _navigationStore;

	public IMainView View { get; }

	public MainPresenter(IMainView mainView, NavigationStore navigationStore)
	{
		_navigationStore = navigationStore;
		_navigationStore.CurrentPresenterChanged += OnCurrentPresenterChanged;

		View = mainView;
	}

	private void OnCurrentPresenterChanged(object? sender, CurrentPresenterChangedEventArgs e)
	{
		View.SwitchSubViewTo(e.NewPresenter.View);
	}

	// Note that this might not even be used, as the MainPresenter is persistent
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_navigationStore.CurrentPresenterChanged -= OnCurrentPresenterChanged;
	}
}
