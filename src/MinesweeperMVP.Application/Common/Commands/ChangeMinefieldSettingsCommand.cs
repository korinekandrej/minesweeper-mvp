using MinesweeperMVP.Application.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Commands;

public class ChangeMinefieldSettingsCommand : CommandBase
{
	private readonly MinefieldSettingsModel _minefieldSettingsModel;

	public ChangeMinefieldSettingsCommand(MinefieldSettingsModel minefieldSettingsModel)
	{
		_minefieldSettingsModel = minefieldSettingsModel;
	}

	public override void Execute(object? parameter)
	{
		if (parameter is not MinefieldSettingsChangedEventArgs eventArgs)
		{
			throw new ArgumentException(
				$"Expected argument of type {nameof(MinefieldSettingsChangedEventArgs)}, but got {parameter?.GetType().Name}.",
				nameof(parameter)
			);
		}

		_minefieldSettingsModel.Set(eventArgs.RowCount, eventArgs.ColCount, eventArgs.MineCount);
	}
}
