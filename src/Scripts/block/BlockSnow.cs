using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSnow : Block {

    public BlockSnow() : base("white") {
        this.setNeedsToUpdate();
    }

    public override void onUpdate()
    {
        base.onUpdate();

        Chunk chunkInstance = this.chunkInstance;

        try
        {
            if (chunkInstance.getBlockAt(this.x - 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x - 1, this.y, this.z) == orange)
            {
                chunkInstance.setBlock(this.x - 1, this.y, this.z, air);
                chunkInstance.setBlock(this.x - 1, this.y, this.z, white);
            }


            if (chunkInstance.getBlockAt(this.x + 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x + 1, this.y, this.z) == orange)
            {
                chunkInstance.setBlock(this.x + 1, this.y, this.z, air);
                chunkInstance.setBlock(this.x + 1, this.y, this.z, white);
            }

            if (chunkInstance.getBlockAt(this.x, this.y, this.z - 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z - 1) == orange)
            {
                chunkInstance.setBlock(this.x, this.y, this.z - 1, air);
                chunkInstance.setBlock(this.x, this.y, this.z - 1, white);
            }

            if (chunkInstance.getBlockAt(this.x, this.y, this.z + 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z + 1) == orange)
            {
                chunkInstance.setBlock(this.x, this.y, this.z + 1, air);
                chunkInstance.setBlock(this.x, this.y, this.z + 1, white);
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