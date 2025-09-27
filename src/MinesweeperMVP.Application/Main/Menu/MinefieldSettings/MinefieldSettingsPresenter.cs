using MinesweeperMVP.Application.Common.Commands;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.Menu.MinefieldSettings;

public class MinefieldSettingsPresenter : IPresenterWithModel<IMinefieldSettingsView, MinefieldSettingsModel>
{
	private readonly ChangeMinefieldSettingsCommand _changeMinefieldSettingsCommand;

	public IMinefieldSettingsView View { get; set; }
	public MinefieldSettingsModel Model { get; set; }

	public MinefieldSettingsPresenter(IMinefieldSettingsView view, MinefieldSettingsModel model)
	{
		_changeMinefieldSettingsCommand = new ChangeMinefieldSettingsCommand(model);

		View = view;
		Model = model;

		View.SetValues(Model.RowCount, Model.ColCount, Model.MineCount);
		View.ValuesChanged += _changeMinefieldSettingsCommand.OnEventExecute;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		View.ValuesChanged -= _changeMinefieldSettingsCommand.OnEventExecute;
	}
}
