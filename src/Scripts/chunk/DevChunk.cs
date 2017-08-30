using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevChunk : Chunk
{

	public DevChunk(int px, int pz, World world) : base(px, pz, world)
	{

	}

	public override void OnUnityUpdate()
	{

		if (hasGenerated && !hasRendered && hasDrawn)
		{
			base.OnUnityUpdate();

			hasGenerated = false;
			hasDrawn = false;
			hasRendered = false;

			Start();
		}
	}

}