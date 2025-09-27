using MinesweeperMVP.Application.Common.Commands;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main.GameSession.Minefield;

public class MinefieldPresenter : IPresenterWithModel<IMinefieldView, MinefieldModel>
{
	private readonly AffectTileCommand _affectTileCommand;

	public IMinefieldView View { get; private set; }
	public MinefieldModel Model { get; private set; }

	public MinefieldPresenter(IMinefieldView view, MinefieldModel model)
	{
		_affectTileCommand = new AffectTileCommand(model);

		View = view;
		Model = model;

		View.InitializeTiles(Model.RowCount, Model.ColumnCount);
		// CONSIDER: this gets called every time you click in this scene, no mather if its outside of minefield
		// In the view, add CollisionShape to register clicks
		View.TileClicked += _affectTileCommand.OnEventExecute;

		Model.SpecificTilesChanged += OnModelSpecificTilesChanged;
		Model.MineRevealed += OnModelMineRevealed;
	}

	private void OnModelSpecificTilesChanged(object o, SpecificTilesChangedEventArgs e)
	{
		View.UpdateSpecificTiles(e.TileModelInfo.Select(
			item => (item.Pos, TileModelToContent(item.TileModel)))
		);
	}

	private void OnModelMineRevealed(object o, MineRevealedEventArgs e)
	{
		// Mark the directly revealed mine with red color
		View.UpdateSpecificTiles([(e.Pos, (int)IMinefieldView.NonNumTileContent.MineRed)]);
	}

	private static int TileModelToContent(TileModel tileModel)
	{
		if (tileModel.IsRevealed)
		{
			if (tileModel.HasMine) return (int)IMinefieldView.NonNumTileContent.Mine;
			return tileModel.AdjacentMineCount; // Numeric tile content
		}
		if (tileModel.IsFlagged) return (int)IMinefieldView.NonNumTileContent.Flag;
		return (int)IMinefieldView.NonNumTileContent.Unrevealed;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		View.TileClicked -= _affectTileCommand.OnEventExecute;

		Model.SpecificTilesChanged -= OnModelSpecificTilesChanged;
		Model.MineRevealed -= OnModelMineRevealed;
	}
}
