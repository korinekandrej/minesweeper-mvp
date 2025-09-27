using MinesweeperMVP.Application.Common.Events;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.Menu.MinefieldSettings;

public interface IMinefieldSettingsView : IView
{
	// TODO: rename args to ValuesChangedEventArgs
	public event EventHandler<MinefieldSettingsChangedEventArgs> ValuesChanged;

	public void SetValues(int rowCount, int colCount, int mineCount);
}
