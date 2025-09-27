using MinesweeperMVP.Application.Common.Enums;
using MinesweeperMVP.Application.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Commands;

public class AffectTileCommand : CommandBase
{
	private readonly MinefieldModel _minefieldModel;

	public AffectTileCommand(MinefieldModel minefieldModel)
	{
		_minefieldModel = minefieldModel;
	}

	public override void Execute(object? parameter)
	{
		if (parameter is not TileClickedEventArgs eventArgs)
		{
			throw new ArgumentException(
				$"Expected argument of type {nameof(TileClickedEventArgs)}, but got {parameter?.GetType().Name}.",
				nameof(parameter)
			);
		}

		switch (eventArgs.Button)
		{
			case MouseButton.Left:
				_minefieldModel.RevealTile(eventArgs.TilePos);
				break;
			case MouseButton.Right:
				_minefieldModel.FlagTile(eventArgs.TilePos);
				break;
			default:
				throw new NotImplementedException($"MouseButton.{eventArgs.Button} case not implemented.");
		}
	}
}
