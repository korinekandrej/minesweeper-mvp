using Godot;
using MinesweeperMVP.Domain.Common; // CONSIDER: I don't understand how can this work, this project does not have Domain as a dependency? Either way, do not use GridPosition in this file
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Main.GameSession.Minefield;

public partial class MinefieldView : Control, IMinefieldView
{
	private static readonly int _tileSetAtlasSourceID = 1;
	// CONSIDER: rename this to tileSet |Atlas| TileCoordinatesByTileContent
	private static readonly Dictionary<int, Vector2I> _tileSetTileCoordinatesByTileContent = new Dictionary<int, Vector2I>()
	{
		// First TileSet Row (tiles without numbers)
		{(int)IMinefieldView.NonNumTileContent.Flag, new(0, 0)},
		{(int)IMinefieldView.NonNumTileContent.Unrevealed, new(1, 0)},
		{(int)IMinefieldView.NonNumTileContent.Mine, new(2, 0)},
		{(int)IMinefieldView.NonNumTileContent.MineRed, new(3, 0)},
		// Second TileSet Row (tiles with numbers)
		{0, new(0, 1)},
		{1, new(1, 1)},
		{2, new(2, 1)},
		{3, new(3, 1)},
		// Third TileSet Row
		{4, new(0, 2)},
		{5, new(1, 2)},
		{6, new(2, 2)},
		{7, new(3, 2)},
		// Fourth TileSet Row
		{8, new(0, 3)},
	};
	

	public event EventHandler<TileClickedEventArgs> TileClicked;

	// TODO: make TileSize work even if separation, margins and textureRegionSize are set in TileSet Editor
	private Vector2I TileSize => _tileMapLayer.TileSet.TileSize;
	private TileMapLayer _tileMapLayer;


	public async void InitializeTiles(int rowCount, int colCount)
	{
		// The presenter should not care if its view is ready (this is an implementation caveat of Godot's nodes)
		// Thus I am awaitng ready here
		if (!IsNodeReady()) await ToSignal(this, "ready");

		ResizeToFitTiles(rowCount, colCount);

		_tileMapLayer.Clear();
		for (int col = 0; col < colCount; col++)
		{
			for (int row = 0; row < rowCount; row++)
			{
				SetTile(row, col, (int)IMinefieldView.NonNumTileContent.Unrevealed);
			}
		}
	}

	public void UpdateSpecificTiles(IEnumerable<(GridPosition Pos, int TileContent)> tileContentInfo)
	{
		foreach (var (Pos, TileContent) in tileContentInfo)
		{
			SetTile(Pos, TileContent);
		}
	}

	private void ResizeToFitTiles(int rowCount, int colCount)
	{
		CustomMinimumSize = new Vector2(colCount * TileSize.X, rowCount * TileSize.Y);
	}

	private void SetTile(GridPosition pos, int tileContent)
	{
		SetTile(pos.Row, pos.Column, tileContent);
	}

	private void SetTile(int row, int column, int tileContent)
	{
		_tileMapLayer.SetCell(
			new Vector2I(column, row),
			_tileSetAtlasSourceID,
			_tileSetTileCoordinatesByTileContent[tileContent]
		);
	}

	public override void _Ready()
	{
		base._Ready();
		_tileMapLayer = GetNode<TileMapLayer>("%TileMapLayer");
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left || mouseEvent.ButtonIndex == MouseButton.Right)
			{
				// By using _tileMapLayer.GlobalPosition instead of GlobalPosition 
				// this will work even if ResizeToFitTiles was not called
				Vector2 clickedTileGridIndices = (mouseEvent.Position - _tileMapLayer.GlobalPosition) / TileSize;

				OnTileClicked(
					mouseEvent.ButtonIndex,
					new Vector2I(
						// Without FloorToInt, the negative position will not be correct
						// For example: if mousePos=(-5,-5) with tileSize=10 gives (0, 0) indices
						Mathf.FloorToInt(clickedTileGridIndices.X),
						Mathf.FloorToInt(clickedTileGridIndices.Y)
					)
				);
			}
		}
	}

	private void OnTileClicked(MouseButton button, Vector2I tilePos)
	{
		// Note that row = X, Column = Y
		TileClicked?.Invoke(this, new TileClickedEventArgs(
			// CONSIDER: SOLVE this ugly thing:
			(MinesweeperMVP.Application.Common.Enums.MouseButton)(int)button,
			new(tilePos.Y, tilePos.X))
		);
	}
}
