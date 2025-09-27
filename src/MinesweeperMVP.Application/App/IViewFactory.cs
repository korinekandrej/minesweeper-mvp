using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.App;

public interface IViewFactory
{
	public T Create<T>() where T : class, IView;
}
