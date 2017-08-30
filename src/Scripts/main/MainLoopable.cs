using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoopable : ILoopable
{

	private static MainLoopable INSTANCE;
	private List<ILoopable> registeredLoops = new List<ILoopable>();

	public static void Instantiate()
	{
		INSTANCE = new MainLoopable();
		Logger.Instantiate();
		World.Instantiate();
		Block.air.getBlockName();
		BlockRegistry.registerBlocks();
	}

	public void registerLoopable(ILoopable loop)
	{
		this.registeredLoops.Add(loop);
	}

	public void unregisterLoop(ILoopable loop)
	{
		this.registeredLoops.Remove(loop);
	}

	public static MainLoopable instance()
	{
		return INSTANCE;
	}

	public void Start()
	{
		foreach (ILoopable loop in this.registeredLoops)
		{
			loop.Start();
		}
	}

	public void Update()
	{
		foreach (ILoopable loop in this.registeredLoops)
		{
			loop.Update();
		}
	}

	public void OnApplicationQuit()
	{
		foreach (ILoopable loop in this.registeredLoops)
		{
			loop.OnApplicationQuit();
		}
	}
}