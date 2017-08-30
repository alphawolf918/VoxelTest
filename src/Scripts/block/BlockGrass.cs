using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlockGrass : Block
{

    public BlockGrass() : base("grass")
    {
        this.setNeedsToUpdate();
    }

    public BlockGrass(string strTexture) : base(strTexture)
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
                if (chunkInstance.getBlockAt(x - 1, y, z) != null && chunkInstance.getBlockAt(x - 1, y, z) == dirt)
                {
                    chunkInstance.setBlock(x - 1, y, z, air);
                    chunkInstance.setBlock(x - 1, y, z, grass);
                }


                if (chunkInstance.getBlockAt(x + 1, y, z) != null && chunkInstance.getBlockAt(x + 1, y, z) == dirt)
                {
                    chunkInstance.setBlock(x + 1, y, z, air);
                    chunkInstance.setBlock(x + 1, y, z, grass);
                }

                if (chunkInstance.getBlockAt(x, y, z - 1) != null && chunkInstance.getBlockAt(x, y, z - 1) == dirt)
                {
                    chunkInstance.setBlock(x, y, z - 1, air);
                    chunkInstance.setBlock(x, y, z - 1, grass);
                }

                if (chunkInstance.getBlockAt(x, y, z + 1) != null && chunkInstance.getBlockAt(x, y, z + 1) == dirt)
                {
                    chunkInstance.setBlock(x, y, z + 1, air);
                    chunkInstance.setBlock(x, y, z + 1, grass);
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