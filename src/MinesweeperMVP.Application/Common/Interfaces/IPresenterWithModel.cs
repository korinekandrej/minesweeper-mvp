using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Interfaces;

// This exists because all presenters have a view, but not all have a model
public interface IPresenterWithModel<TView, TModel> : IPresenter<TView> where TView : IView where TModel : IModel
{
	public TModel Model { get; }
}
