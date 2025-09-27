using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Events;

public class CurrentPresenterChangedEventArgs(IPresenter<IView> newPresenter) : EventArgs
{
	public IPresenter<IView> NewPresenter { get; } = newPresenter;
}
