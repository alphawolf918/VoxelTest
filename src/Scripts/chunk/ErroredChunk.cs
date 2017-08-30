using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErroredChunk : Chunk
{

	public ErroredChunk(int px, int pz, World world) : base(px, pz, world)
	{

	}

	public override void OnUnityUpdate()
	{
		throw new Exception("Tried to use OnUnityUpdate in ErroredChunk class.");
	}

	public override void Start()
	{
		throw new Exception("Tried to use Start in ErroredChunk class.");
	}

	public override void Update()
	{
		throw new Exception("Tried to use Update in ErroredChunk class.");
	}

}