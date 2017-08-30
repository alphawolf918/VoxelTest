using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSnow : Block {

    public BlockSnow() : base("snow") {
        this.setNeedsToUpdate();
    }

    public override void onUpdate()
    {
        base.onUpdate();

        Chunk chunkInstance = this.chunkInstance;

        try
        {
            if (chunkInstance.getBlockAt(this.x - 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x - 1, this.y, this.z) == dirt)
            {
                chunkInstance.setBlock(this.x - 1, this.y, this.z, air);
                chunkInstance.setBlock(this.x - 1, this.y, this.z, snow);
            }


            if (chunkInstance.getBlockAt(this.x + 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x + 1, this.y, this.z) == dirt)
            {
                chunkInstance.setBlock(this.x + 1, this.y, this.z, air);
                chunkInstance.setBlock(this.x + 1, this.y, this.z, snow);
            }

            if (chunkInstance.getBlockAt(this.x, this.y, this.z - 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z - 1) == dirt)
            {
                chunkInstance.setBlock(this.x, this.y, this.z - 1, air);
                chunkInstance.setBlock(this.x, this.y, this.z - 1, snow);
            }

            if (chunkInstance.getBlockAt(this.x, this.y, this.z + 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z + 1) == dirt)
            {
                chunkInstance.setBlock(this.x, this.y, this.z + 1, air);
                chunkInstance.setBlock(this.x, this.y, this.z + 1, snow);
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