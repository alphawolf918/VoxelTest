using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFalling : Block
{

    public static BlockFalling fallingInstance;

    public BlockFalling(string blockName) : base(blockName)
    {
        fallingInstance = this;
        this.setNeedsToUpdate();
    }

    public override void onUpdate()
    {
        base.onUpdate();

        Chunk chunkInstance = this.chunkInstance;

        try
        {
            if (chunkInstance.getBlockAt(this.x, this.y - 1, this.z) == air)
            {
                chunkInstance.setBlock(this.x, this.y, this.z, air);
                chunkInstance.setBlock(this.x, this.y - 1, this.z, this);
            }
        }
        catch (Exception ex)
        {
            if (GameSettings.enableDebugMode)
            {
                Logger.Log("Error: " + ex.Message + " \r\n " + ex.StackTrace);
            }
        }
    }

}