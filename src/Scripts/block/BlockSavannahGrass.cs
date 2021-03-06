﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSavannahGrass : Block
{

    public BlockSavannahGrass() : base("yellow")
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
                if (chunkInstance.getBlockAt(this.x - 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x - 1, this.y, this.z) == orange)
                {
                    chunkInstance.setBlock(this.x - 1, this.y, this.z, air);
                    chunkInstance.setBlock(this.x - 1, this.y, this.z, yellow);
                }


                if (chunkInstance.getBlockAt(this.x + 1, this.y, this.z) != null && chunkInstance.getBlockAt(this.x + 1, this.y, this.z) == orange)
                {
                    chunkInstance.setBlock(this.x + 1, this.y, this.z, air);
                    chunkInstance.setBlock(this.x + 1, this.y, this.z, yellow);
                }

                if (chunkInstance.getBlockAt(this.x, this.y, this.z - 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z - 1) == orange)
                {
                    chunkInstance.setBlock(this.x, this.y, this.z - 1, air);
                    chunkInstance.setBlock(this.x, this.y, this.z - 1, yellow);
                }

                if (chunkInstance.getBlockAt(this.x, this.y, this.z + 1) != null && chunkInstance.getBlockAt(this.x, this.y, this.z + 1) == orange)
                {
                    chunkInstance.setBlock(this.x, this.y, this.z + 1, air);
                    chunkInstance.setBlock(this.x, this.y, this.z + 1, yellow);
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