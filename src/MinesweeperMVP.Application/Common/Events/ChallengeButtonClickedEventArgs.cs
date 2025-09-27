using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Events;

// CONSIDER: this is pretty wierd, figure out something better
public class ChallengeButtonClickedEventArgs(int rowCount, int colCount, int mineCount) :
	MinefieldSettingsChangedEventArgs(rowCount, colCount, mineCount)
{

}
