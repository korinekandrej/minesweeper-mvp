using Godot;
using MinesweeperMVP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Bootstrapper;

public class ViewFactory : IViewFactory
{
	// The commented lines are scenes already contained in other scenes, thus they don't need to be created
	private readonly Dictionary<Type, string> _godotSceneUIDByViewType = new Dictionary<Type, string>()
	{
		//{typeof(IMainView), "uid://ck5yoqvewnrqh"},
		{typeof(IMenuView), "uid://dors5u3fdsgj3"},
		{typeof(IGameSessionView), "uid://fq61vt6ek03q"},
		{typeof(IHighscoreBoardView), "uid://r8gjwlpgsbhc"},
		//{typeof(IMinefieldSettingsView), "uid://d4lcrae4r6oaw"},
		//{typeof(IMinefieldView), "uid://bhgwi633hr6sj"},
	};

	public T Create<T>() where T : class, IView
	{
		PackedScene packedScene = (PackedScene)ResourceLoader.Load(_godotSceneUIDByViewType[typeof(T)]);
		return packedScene.Instantiate<T>();
	}
}
