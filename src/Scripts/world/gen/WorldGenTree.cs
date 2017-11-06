
using System;

public class WorldGenTree : WorldGen
{

    public WorldGenTree()
    {

    }

    public override void generate(Chunk chunk, int x, int y, int z, Random rand)
    {
        base.generate(chunk, x, y, z, rand);
        int randGen = rand.Next(6);
        randGen = (randGen <= 1) ? 2 : randGen;

        Block blockBelow = chunk.getBlockAt(x, (y - 1), z);

        if (blockBelow != Block.green && blockBelow != Block.orange && blockBelow != Block.white && blockBelow != Block.yellow)
        {
            Logger.Log("Could not generate tree.");
            return;
        }

        for (int i = 0; i < randGen; i++)
        {
            chunk.setBlock(x, (y + i), z, Block.brown);
            if (i == randGen)
            {
                chunk.setBlock((x + 1), (y + i), z, Block.brown);
                chunk.setBlock((x - 1), (y + i), z, Block.brown);
                chunk.setBlock(x, (y + i), (z + 1), Block.brown);
                chunk.setBlock(x, (y + i), (z - 1), Block.brown);
            }
        }

        y += randGen;

        chunk.setBlock(x, y, z, Block.lightgreen);
        chunk.setBlock((x + 1), y, z, Block.lightgreen);
        chunk.setBlock(x, y, (z + 1), Block.lightgreen);
        chunk.setBlock((x + 1), y, (z + 1), Block.lightgreen);
        chunk.setBlock((x - 1), y, z, Block.lightgreen);
        chunk.setBlock(x, y, (z - 1), Block.lightgreen);
        chunk.setBlock((x - 1), y, (z - 1), Block.lightgreen);
        chunk.setBlock(x, (y + 1), z, Block.lightgreen);
        chunk.setBlock((x + 1), y, (z - 1), Block.lightgreen);
        chunk.setBlock((x - 1), y, (z + 1), Block.lightgreen);
        chunk.setBlock(x, (y + 1), z, Block.lightgreen);
        chunk.setBlock((x + 1), (y + 1), z, Block.lightgreen);
        chunk.setBlock(x, (y + 1), (z + 1), Block.lightgreen);
        chunk.setBlock((x - 1), (y + 1), z, Block.lightgreen);
        chunk.setBlock(x, (y + 1), (z - 1), Block.lightgreen);
        chunk.setBlock((x - 1), (y + 1), (z - 1), Block.lightgreen);
        chunk.setBlock((x + 1), (y + 1), (z + 1), Block.lightgreen);
        chunk.setBlock((x + 1), (y + 1), (z - 1), Block.lightgreen);
        chunk.setBlock((x - 1), (y + 1), (z + 1), Block.lightgreen);
        chunk.setBlock(x, (y + 2), z, Block.lightgreen);
        chunk.setBlock((x + 1), (y + 2), z, Block.lightgreen);
        chunk.setBlock(x, (y + 2), (z + 1), Block.lightgreen);
        chunk.setBlock((x - 1), (y + 2), z, Block.lightgreen);
        chunk.setBlock(x, (y + 2), (z - 1), Block.lightgreen);
    }
}