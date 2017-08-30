using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BlockRegistry
{
	private static readonly bool DebugMode = GameSettings.enableDebugMode;

	private static List<Block> registeredBlocks = new List<Block>();

	public static void registerBlock(Block b)
	{
		registeredBlocks.Add(b);
	}

	public static void registerBlocks()
	{
		if (DebugMode)
		{
			int i = 0;
			List<string> _names = new List<string>();
			foreach (Block b in registeredBlocks)
			{
				_names.Add(string.Format("CurrentID: {0}, BlockName: {1}, BlockID: {2}", i, b.getBlockName(), b.getBlockID()));
				i++;
			}

			File.WriteAllLines("BlockRegistry.txt", _names.ToArray());
		}
	}

	internal static Block getBlockFromID(int ID)
	{
		try
		{
			return registeredBlocks[ID];
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}

		return null;
	}

}