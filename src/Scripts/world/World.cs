using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class World : ILoopable
{
    public static World worldInstance
    {
        get; private set;
    }

    public static string worldName = "world";

    private Thread worldThread;

    private bool isRunning = false;
    private bool ranOnce = false;

    private List<Chunk> LoadedChunks = new List<Chunk>();

    private static readonly int renderDistance = 3;

    public static Int3 startVector;

    private BiomeBase mainBiome = null;

    private static System.Random rand = new System.Random();

    public static void Instantiate()
    {
        worldInstance = new World();
        MainLoopable.instance().registerLoopable(worldInstance);

        System.Random rand = new System.Random();

        startVector = new Int3(new Vector3(rand.Next(-1000, 1000), 150, rand.Next(-1000, 1000)));
    }

    public void OnApplicationQuit()
    {
        foreach (Chunk c in new List<Chunk>(LoadedChunks))
        {
            try
            {
                Serializer.Serialize_ToFile_FullPath<int[,,]>(FileManager.getChunkString(c.posX, c.posZ), c.getChunkSaveData());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
        }

        isRunning = false;
        Logger.Log("Stopping world thread..");
    }

    public void Start()
    {
        isRunning = true;
        worldThread = new Thread(() =>
        {
            Logger.Log("Initializing world thread..");
            while (isRunning)
            {
                try
                {
                    if (!ranOnce)
                    {
                        ranOnce = true;

                        BiomeBase currentBiome = (mainBiome == null) ? BiomeBase.instance.getWeightedRandomBiome() : mainBiome;
                        BiomeBase nextBiome = null;

                        for (int x = -renderDistance; x < renderDistance; x++)
                        {
                            for (int z = -renderDistance; z < renderDistance; z++)
                            {
                                Int3 chunkPosNew = new Int3(startVector.x, startVector.y, startVector.z);
                                chunkPosNew.addPos(new Int3((x * Chunk.chunkWidth), startVector.y, (z * Chunk.chunkWidth)));
                                chunkPosNew.ToChunkCoordinates();

                                string path = FileManager.getChunkString(chunkPosNew.x, chunkPosNew.z);
                                if (File.Exists(path))
                                {
                                    try
                                    {
                                        Chunk c = new Chunk(chunkPosNew.x, chunkPosNew.z, Serializer.Deserialize_FromFile<int[,,]>(path), this);
                                        c.world = this;
                                        LoadedChunks.Add(c);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Log("Error: " + ex.StackTrace);
                                    }
                                }
                                else
                                {
                                    Chunk c = new Chunk(chunkPosNew.x, chunkPosNew.z, this);
                                    c.world = this;
                                    c.setBiome(currentBiome);
                                    if (nextBiome == null)
                                    {
                                        int randGen = rand.Next(200);

                                        if (randGen <= 5)
                                        {
                                            nextBiome = BiomeBase.instance.getWeightedRandomBiome();
                                        }
                                        else
                                        {
                                            nextBiome = currentBiome;
                                        }
                                    }
                                    mainBiome = nextBiome;
                                    LoadedChunks.Add(c);
                                }
                            }
                        }

                        foreach (Chunk c in LoadedChunks)
                        {
                            c.Start();
                        }

                    }

                    if (GameManager.playerLoaded())
                    {
                        startVector = new Int3(GameManager.instance.playerPos);
                    }

                    foreach (Chunk c in new List<Chunk>(LoadedChunks))
                    {
                        Vector2 chunkPosition = new Vector2(c.posX * Chunk.chunkWidth, c.posZ * Chunk.chunkWidth);
                        Vector2 v2PlayerPosition = new Vector2(startVector.x, startVector.z);
                        int renderChunkDistance = renderDistance + 2;
                        int chunkWidth = Chunk.chunkWidth;

                        if (Vector2.Distance(chunkPosition, v2PlayerPosition) > (renderChunkDistance * chunkWidth))
                        {
                            c.Degenerate();
                        }
                    }

                    for (int x = -renderDistance; x < renderDistance; x++)
                    {
                        for (int z = -renderDistance; z < renderDistance; z++)
                        {
                            Int3 chunkPosNew = new Int3(startVector.x, startVector.y, startVector.z);
                            chunkPosNew.addPos(new Int3((x * Chunk.chunkWidth), 0, (z * Chunk.chunkWidth)));
                            chunkPosNew.ToChunkCoordinates();

                            if (!chunkExists(chunkPosNew.x, chunkPosNew.z))
                            {
                                string path = FileManager.getChunkString(chunkPosNew.x, chunkPosNew.z);

                                BiomeBase currentBiome = (mainBiome == null) ? BiomeBase.instance.getWeightedRandomBiome() : mainBiome;
                                BiomeBase nextBiome = null;

                                if (File.Exists(path))
                                {
                                    try
                                    {
                                        Chunk c = new Chunk(chunkPosNew.x, chunkPosNew.z, Serializer.Deserialize_FromFile<int[,,]>(path), this);
                                        c.world = this;
                                        c.Start();
                                        LoadedChunks.Add(c);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Log("Error: " + ex.StackTrace);
                                    }
                                }
                                else
                                {
                                    Chunk c = new Chunk(chunkPosNew.x, chunkPosNew.z, this);
                                    c.world = this;
                                    c.setBiome(currentBiome);
                                    if (nextBiome == null)
                                    {
                                        int randGen = rand.Next(200);

                                        if (randGen <= 5)
                                        {
                                            nextBiome = BiomeBase.instance.getWeightedRandomBiome();
                                        }
                                        else
                                        {
                                            nextBiome = currentBiome;
                                        }
                                    }
                                    mainBiome = nextBiome;
                                    c.Start();
                                    LoadedChunks.Add(c);
                                }
                            }
                        }
                    }

                    foreach (Chunk c in new List<Chunk>(LoadedChunks))
                    {
                        c.Update();
                    }

                }
                catch (Exception ex)
                {
                    Logger.Log("An exception has been caught and logged: " + ex.Message + " " + ex.StackTrace);
                }
            }
            Logger.Log("World thread successfully stopped.");
            Logger.mainLog.Update();
        });
        worldThread.Start();
    }

    internal void removeChunk(Chunk chunk)
    {
        LoadedChunks.Remove(chunk);
    }

    public bool chunkExists(int px, int pz)
    {
        foreach (Chunk c in new List<Chunk>(LoadedChunks))
        {
            if (c.posX.Equals(px) && c.posZ.Equals(pz))
            {
                return true;
            }
        }
        return false;
    }

    public Chunk GetChunk(int px, int pz)
    {
        foreach (Chunk c in new List<Chunk>(LoadedChunks))
        {
            if (c.posX.Equals(px) && c.posZ.Equals(pz))
            {
                return c;
            }
        }
        return new ErroredChunk(0, 0, this);
    }

    public void Update()
    {
        foreach (Chunk c in new List<Chunk>(LoadedChunks))
        {
            if (c != null)
            {
                c.OnUnityUpdate();
            }
        }
    }

    public static void setBlock(int x, int y, int z, Block block)
    {
        Chunk.instance.setBlock(x, y, z, block);
    }

    public static void setBlock(Vector3 blockPos, Block block)
    {
        Chunk.instance.setBlock(blockPos, block);
    }

    public World getWorld()
    {
        return this;
    }
}