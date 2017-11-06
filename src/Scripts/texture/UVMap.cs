using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UvMap
{

	private static List<UvMap> _Maps = new List<UvMap>();

	public string name;
	public Vector2[] UV_MAP;

	public UvMap(string uvName, Vector2[] uvMap)
	{
		this.name = uvName;
		this.UV_MAP = uvMap;
	}

	public void Register()
	{
		_Maps.Add(this);
	}

	public static UvMap getUVMap(string name)
	{
		if (name.Equals("air"))
		{
			return _Maps[0];
		}

		foreach (UvMap m in _Maps)
		{
            if (m.name.Equals(name))
            {
                return m;
            }
            else
            {
                Logger.Log("Error");
            }
		}

		Logger.Log("Can't find associated image called: " + name);

		List<string> _names = new List<string>();

		foreach (UvMap m in _Maps)
		{
			_names.Add(m.name + " NOT " + name);
		}

		File.WriteAllLines("names.txt", _names.ToArray());

		GameManager.exitGame();

		return _Maps[0];
	}

}