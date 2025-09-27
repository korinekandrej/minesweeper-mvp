using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Main.Menu.MinefieldSettings;

public partial class MinefieldSettingsView : Control, IMinefieldSettingsView
{
	public event EventHandler<MinefieldSettingsChangedEventArgs> ValuesChanged;

	private SpinBox _rowsSpinBox;
	private SpinBox _columnsSpinBox;
	private SpinBox _minesSpinBox;

	public override void _Ready()
	{
		base._Ready();

		// TODO: remove magic numbers
		GetNode<Button>("%EasyPresetButton").Pressed += () => SetValues(9, 9, 10);
		GetNode<Button>("%MediumPresetButton").Pressed += () => SetValues(16, 16, 40);
		GetNode<Button>("%HardPresetButton").Pressed += () => SetValues(16, 30, 99);

		_rowsSpinBox = GetNode<SpinBox>("%RowsSpinBox");
		_columnsSpinBox = GetNode<SpinBox>("%ColumnsSpinBox");
		_minesSpinBox = GetNode<SpinBox>("%MinesSpinBox");

		_rowsSpinBox.ValueChanged += val => OnValuesChanged();
		_columnsSpinBox.ValueChanged += val => OnValuesChanged();
		_minesSpinBox.ValueChanged += val => OnValuesChanged();
	}

	public async void SetValues(int rowCount, int colCount, int mineCount)
	{
		if (!IsNodeReady()) await ToSignal(this, "ready");

		_rowsSpinBox.Value = rowCount;
		_columnsSpinBox.Value = colCount;
		_minesSpinBox.Value = mineCount;
	}

	private void OnValuesChanged()
	{
		_minesSpinBox.MaxValue = _rowsSpinBox.Value * _columnsSpinBox.Value;

		ValuesChanged?.Invoke(this, new(
			(int)_rowsSpinBox.Value,
			(int)_columnsSpinBox.Value,
			(int)_minesSpinBox.Value
		));
	}
}
