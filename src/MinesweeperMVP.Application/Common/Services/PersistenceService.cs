using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinesweeperMVP.Application.Common.Services;

/// <summary>
/// Abstracts a file, which will a persisting object be saved into or loaded from.
/// </summary>
public class PersistenceService<T> where T : class, new()
{
	private readonly string _path;
	private readonly T _persistingObjectDefault;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public PersistenceService(string path, T? persistingObjectDefault = null)
	{
		persistingObjectDefault ??= new T();

		_path = path;
		_persistingObjectDefault = persistingObjectDefault;
		_jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
	}

	public void Save(T persistingObject)
	{
		string jsonString = JsonSerializer.Serialize(persistingObject, _jsonSerializerOptions);
		File.WriteAllText(_path, jsonString);
	}

	public T Load()
	{
		if (!File.Exists(_path)) return _persistingObjectDefault;

		// TODO: Add additional checks
		string loadedJson = File.ReadAllText(_path);
		T? loadedObject = JsonSerializer.Deserialize<T>(loadedJson);

		loadedObject ??= _persistingObjectDefault;

		return loadedObject;
	}
}
