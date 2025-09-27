using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Interfaces;

// Note that the presenters (and triads respectively) can be divided into 3 categories:
// "main" presenter (MainPresenter) - could be seen as the "window"
// "screen" presenters (Menu, GameSession) - they encompass the whole window (one at a time)
// "nested" presenters (MinefieldSettings, Minefield) - they are part of the screen presenters

// Implements IDisposable, as presenters always have events which need to be unsubscribed
public interface IPresenter<out TView> : IDisposable where TView : IView
{
	public TView View { get; }
}
