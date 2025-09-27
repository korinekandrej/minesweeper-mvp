using MinesweeperMVP.Application.Common.Interfaces;
using MinesweeperMVP.Application.Main.Menu.MinefieldSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.Menu;

public interface IMenuView : IView
{
	// TODO: generelize this to ButtonClicked with args
	public event EventHandler? StartGameButtonClicked;
	public event EventHandler? HighscoresButtonClicked;

	public IMinefieldSettingsView GetMinefieldSettingsView();
}
