using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSavannahGrass : Block
{

    public BlockSavannahGrass() : base("savannah_grass")
    {
        this.setNeedsToUpdate();
    }

    public override void onUpdate()
    {
        base.onUpdate();

        Chunk chunkInstance = this.chunkInstance;

        try
        {
            if (chunkInstance.getBlockAt(x, y + 1, z) == air)
            {
                if (chunkInstance.getBlockAt(this.x - 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x - 1, this.y, this.z) == dirt)
                {
                    chunkInstance.setBlock(this.x - 1, this.y, this.z, air);
                    chunkInstance.setBlock(this.x - 1, this.y, this.z, savannahGrass);
                }


                if (chunkInstance.getBlockAt(this.x + 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x + 1, this.y, this.z) == dirt)
                {
                    chunkInstance.setBlock(this.x + 1, this.y, this.z, air);
                    chunkInstance.setBlock(this.x + 1, this.y, this.z, savannahGrass);
                }

                if (chunkInstance.getBlockAt(this.x, this.y, this.z - 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z - 1) == dirt)
                {
                    chunkInstance.setBlock(this.x, this.y, this.z - 1, air);
                    chunkInstance.setBlock(this.x, this.y, this.z - 1, savannahGrass);
                }

                if (chunkInstance.getBlockAt(this.x, this.y, this.z + 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z + 1) == dirt)
                {
                    chunkInstance.setBlock(this.x, this.y, this.z + 1, air);
                    chunkInstance.setBlock(this.x, this.y, this.z + 1, savannahGrass);
                }
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