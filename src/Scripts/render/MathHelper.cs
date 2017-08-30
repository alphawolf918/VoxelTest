using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Int3
{

    public int x, y, z;

    public Int3(int argX, int argY, int argZ)
    {
        this.x = argX;
        this.y = argY;
        this.z = argZ;
    }

    public Int3(Vector3 blockPos)
    {
        this.x = (int) blockPos.x;
        this.y = (int) blockPos.y;
        this.z = (int) blockPos.z;
    }

    public override string ToString()
    {
        return string.Format("x: {0}, y: {1}, z: {2}", x, y, z);
    }

    internal void addPos(Int3 int3)
    {
        this.x += int3.x;
        this.y += int3.y;
        this.z += int3.z;
    }

    internal void ToChunkCoordinates()
    {
        this.x = Mathf.FloorToInt(x / Chunk.chunkWidth);
        this.z = Mathf.FloorToInt(z / Chunk.chunkWidth);
    }
}

[Serializable]
public class Int4
{
    public int x, y, z, w;

    public Int4(int argX, int argY, int argZ, int argW)
    {
        this.x = argX;
        this.y = argY;
        this.z = argZ;
        this.w = argW;
    }

    public Int4(Quaternion blockPos)
    {
        this.x = (int) blockPos.x;
        this.y = (int) blockPos.y;
        this.z = (int) blockPos.z;
        this.w = (int) blockPos.w;
    }
}

[Serializable]
public class WorldTime
{
    public int x, y, z;
    public float timeFloat;

    public WorldTime(Int3 worldVector, float deltaTime)
    {
        this.x = worldVector.x;
        this.y = worldVector.y;
        this.z = worldVector.z;
        this.timeFloat = deltaTime;
    }
}

public class MathHelper
{
    public static MeshData drawBlock(Chunk chunk, Block block, int x, int y, int z)
    {
        MeshData d = new MeshData();

        return d;
    }


    public static MeshData drawCube(Chunk chunk, Block[,,] blocks, Block block, int x, int y, int z, Vector2[] uvMap)
    {
        MeshData d = new MeshData();
        if (block.Equals(Block.air))
        {
            return new MeshData();

        }

        if (y - 1 <= 0 || blocks[x, y - 1, z].getIsTransparent())
        {
            d.Merge(new MeshData( // Bottom Face
        new List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        },
        new List<int>() {
                 0,2,1   ,3,1,2
        },
        uvMap));
        }

        if (y + 1 >= Chunk.getChunkHeight() || blocks[x, y + 1, z].getIsTransparent())
        {
            d.Merge(new MeshData( // Top Face
          new List<Vector3>() {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
           },
           new List<int>() {
                 0,1,2,3,2,1
           },
            uvMap));
        }



        if (x + 1 >= Chunk.chunkWidth || blocks[x + 1, y, z].getIsTransparent())
        {
            d.Merge(new MeshData( // Back Face
          new List<Vector3>() {
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
           },
           new List<int>() {
                 0,2,1,3,1,2
           },
            uvMap));

        }

        if (x - 1 <= 0 || blocks[x - 1, y, z].getIsTransparent())
        {
            d.Merge(new MeshData( // Front Face
         new List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,0),
            new Vector3(0,1,1)
          },
          new List<int>() {
                 0,1,2,3,2,1
          },
           uvMap));
        }
        if (z + 1 >= Chunk.chunkWidth || blocks[x, y, z + 1].getIsTransparent())
        {
            d.Merge(new MeshData( // Right Face
          new List<Vector3>() {
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1)
           },
           new List<int>() {
                 0,1,2,3,2,1
           },
            uvMap));
        }
        if (z - 1 <= 0 || blocks[x, y, z - 1].getIsTransparent())
        {
            d.Merge(new MeshData( // Left Face
         new List<Vector3>() {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
         },
         new List<int>() {
                 0,2,1    ,3,1,2
         },
        uvMap));

        }

        d.AddPos(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        return d;
    }

    internal static void AddBlock(Vector3 roundedPosition, Block block, EntityPlayer player)
    {
        if (roundedPosition.y >= Chunk.getChunkHeight())
        {
            Logger.Log("Block placement exceeded maximum height. Aborting.");
            return;
        }

        int chunkPosX = Mathf.FloorToInt(roundedPosition.x / Chunk.chunkWidth);
        int chunkPosZ = Mathf.FloorToInt(roundedPosition.z / Chunk.chunkWidth);

        Chunk currentChunk;

        try
        {
            currentChunk = World.worldInstance.GetChunk(chunkPosX, chunkPosZ);
            if (currentChunk.GetType().Equals(typeof(ErroredChunk)))
            {
                Logger.Log("Current chunk is errored: " + roundedPosition.ToString());
                return;
            }

            int x = (int) (roundedPosition.x - (chunkPosX * Chunk.chunkWidth));
            int y = (int) roundedPosition.y;
            int z = (int) (roundedPosition.z - (chunkPosZ * Chunk.chunkWidth));

            if (currentChunk.getBlockAt(x, y, z) == Block.air || block == Block.air && currentChunk.getBlockAt(x, y, z) != null)
            {
                if (block == Block.air)
                {
                    if (currentChunk.getBlockAt(x, y, z).getIsBreakable())
                    {
                        Block targetBlock = currentChunk.getBlockAt(x, y, z);
                        player.setActiveBlock(targetBlock);
                        if (player.Inventory.Contains(targetBlock))
                        {
                            int blockIndex = player.Inventory.BinarySearch(targetBlock);
                            Block invBlock = (Block) player.Inventory[blockIndex];
                            invBlock.increaseCurrentStackSize(1);
                        }
                        currentChunk.getBlockAt(x, y, z).onBroken(player);
                        currentChunk.setBlock(x, y, z, Block.air);
                    }
                    else
                    {
                        Logger.Log("That block is unbreakable!");
                    }
                }
                else
                {
                    currentChunk.setBlock(x, y, z, block);
                    Block b = currentChunk.getBlockAt(x, y, z);
                    b.onPlaced(player);
                }
            }
            else
            {
                Logger.Log("Cannot place block there; a block already exists in that location.");
            }

        }
        catch (Exception ex)
        {
            Logger.Log("Exception when adding block: " + ex.Message + " \r\n Stack Trace: \r\n " + ex.StackTrace);
        }
    }

    internal static void RemoveBlock(Vector3 roundedPosition, EntityPlayer player)
    {
        AddBlock(roundedPosition, Block.air, player);
    }

    public static float GenRandFloat(float one, float two)
    {
        System.Random rand = new System.Random();
        return (float) (one + rand.NextDouble() * (two - one));
    }

    public static Guid GenerateSeededGuid(int seed)
    {
        System.Random r = new System.Random(seed);
        byte[] guid = new byte[16];
        r.NextBytes(guid);

        return new Guid(guid);
    }
}