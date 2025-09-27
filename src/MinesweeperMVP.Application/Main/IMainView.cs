using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Main;

public interface IMainView : IView
{
	public void SwitchSubViewTo(IView view);
}
