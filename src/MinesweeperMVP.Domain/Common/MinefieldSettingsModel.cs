using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Common;

public class MinefieldSettingsModel : IModel
{
	public int RowCount { get; private set; }
	public int ColCount { get; private set; }
	public int MineCount { get; private set; }

	public MinefieldSettingsModel(int rowCount, int colCount, int mineCount)
	{
		Set(rowCount, colCount, mineCount);
	}

	public void Set(int rowCount, int colCount, int mineCount)
	{
		RowCount = rowCount;
		ColCount = colCount;
		MineCount = mineCount;
	}

	public MinefieldSettingsModel Clone()
	{
		return new MinefieldSettingsModel(RowCount, ColCount, MineCount);
	}

	public override bool Equals(object? obj)
	{
		if (obj == null) return false;
		if (obj is MinefieldSettingsModel msm)
		{
			return msm.RowCount == RowCount && msm.ColCount == ColCount && msm.MineCount == MineCount;
		}
		else return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(RowCount, ColCount, MineCount);
	}

	public static bool operator ==(MinefieldSettingsModel left, MinefieldSettingsModel right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(MinefieldSettingsModel left, MinefieldSettingsModel right)
	{
		return !(left == right);
	}
}
