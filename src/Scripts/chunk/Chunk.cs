using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chunk : ITickable
{

    private BiomeBase chunkBiome = BiomeBase.biomePlains;

    public bool hasDecorated = false;

    public static readonly int chunkWidth = 16;
    public static int chunkHeight = 64;

    public static Chunk instance;

    public static int landscapeSize = getChunkWidth();
    public static int width = landscapeSize;
    public static int depth = landscapeSize;
    public static int height = getChunkHeight();

    public int heightOffset = 100;

    public int heightScale = 20;
    public float detailScale = 25.0f;

    public int amountOfClouds = 5;

    public int amountOfCaves = 5;

    private World worldObj;

    public Block[,,] blocks;

    public int posX
    {
        private set; get;
    }

    public int posY
    {
        private set; get;
    }

    public int posZ
    {
        private set; get;
    }

    protected bool hasGenerated = false;
    protected bool hasDrawn = false;
    protected bool hasRendered = false;
    private bool drawnLock = false;

    private GameObject go;

    private MeshData data;

    private bool needsToUpdate = false;
    private bool renderLock = false;

    private static bool firstChunkLoaded = false;
    private bool isFirstChunk = false;
    private bool chunksHaveGenerated = false;

    private System.Random rand = new System.Random();

    public List<Block> chunkBlocks = new List<Block>();

    int minCaveY = 6;

    public Chunk(int px, int pz, World world)
    {
        posX = px;
        posZ = pz;

        this.worldObj = world;
    }

    public Chunk(int px, int pz, int[,,] _data, World world)
    {
        hasGenerated = true;

        posX = px;
        posZ = pz;

        loadChunkFromData(_data);

        this.worldObj = world;
    }

    public BiomeBase getBiome()
    {
        return this.chunkBiome;
    }

    public void setBiome(BiomeBase biome)
    {
        this.chunkBiome = biome;
    }

    public void Degenerate()
    {
        try
        {
            Serializer.Serialize_ToFile_FullPath<int[,,]>(FileManager.getChunkString(posX, posZ), getChunkSaveData());
        }
        catch (Exception ex)
        {
            Logger.Log(ex.ToString());
        }


        GameManager.instance.registerDelegate(new Action(() =>
        {
            GameObject.Destroy(go);
        }));

        worldObj.removeChunk(this);
    }


    public int[,,] getChunkSaveData()
    {
        return blocks.ToIntArray();
    }

    public void loadChunkFromData(int[,,] _data)
    {
        blocks = _data.ToBlockArray();
    }

    public float GetHeight(float px, float py, float pz)
    {
        px += (posX * chunkWidth);
        pz += (posZ * chunkWidth);

        float p1 = Mathf.PerlinNoise(px / GameManager.Sdx, pz / GameManager.Sdz) * GameManager.Smul;
        p1 *= (GameManager.Smy * py);

        return p1;
    }

    public virtual void Start()
    {
        if (!firstChunkLoaded)
        {
            firstChunkLoaded = true;
            isFirstChunk = true;
        }

        if (hasGenerated)
        {
            return;
        }

        blocks = new Block[chunkWidth, getChunkHeight(), chunkWidth];

        for (int x = 0; x < getChunkWidth(); x++)
        {
            for (int y = 0; y < getChunkHeight(); y++)
            {
                for (int z = 0; z < getChunkWidth(); z++)
                {
                    float perlin = GetHeight(x, y, z);

                    perlin -= (chunkBiome.getChunkHeightDifference() / 2);

                    if (perlin > GameManager.Scutoff)
                    {
                        setBlock(x, y, z, Block.air);
                    }
                    else
                    {
                        if (perlin > (GameManager.Scutoff / 2))
                        {
                            for (int i = 1; i < 3; i++)
                            {
                                setBlock(x, (y - i), z, chunkBiome.getFillerBlock());
                            }

                            setBlock(x, y, z, chunkBiome.topBlock);
                        }
                        else
                        {
                            int cutOffPoint = (getChunkHeight() / 2);

                            if (y < cutOffPoint)
                            {
                                setBlock(x, y, z, chunkBiome.getStoneBlock());
                            }
                            else if (y < cutOffPoint - 2)
                            {
                                setBlock(x, y, z, chunkBiome.getFillerBlock());
                            }
                            else
                            {
                                setBlock(x, y, z, chunkBiome.getTopBlock());

                                if (!this.hasDecorated)
                                {
                                    chunkBiome.decorate(this, x, y, z);
                                    hasDecorated = true;
                                }
                            }
                        }
                    }

                    if (y <= 2)
                    {
                        setBlock(x, y, z, Block.bedrock);
                    }

                    if (y == 0)
                    {
                        setBlock(x, y, z, Block.air);
                    }

                    else if (y <= 28 && y > 3 && getBlockAt(x, y, z) == chunkBiome.getStoneBlock())
                    {
                        Block randBlock = this.getRandomOre();
                        setBlock(x, y, z, randBlock);

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock((x - i), y, z, randBlock);
                            }

                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock(x, y, (z - i), randBlock);
                            }
                        }

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock((x + i), y, z, randBlock);
                            }
                        }

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock(x, y, (z - i), randBlock);
                            }
                        }

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock(x, y, (z + i), randBlock);
                            }
                        }

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock(x, (y - i), z, randBlock);
                            }
                        }

                        if (Utils.getRNG())
                        {
                            for (int i = 0; i < rand.Next(0, 3); i++)
                            {
                                setBlock(x, (y + i), z, randBlock);
                            }
                        }

                        if (Utils.getRNG(875) && y > minCaveY)
                        {
                            this.generateCaveSystem(x, y, z);
                        }
                    }
                }
            }
        }
        hasGenerated = true;
        chunksHaveGenerated = true;
    }

    public void generateClouds(Vector3 cloudPos, int numClouds = 5)
    {
        this.generateClouds((int) cloudPos.x, (int) cloudPos.y, (int) cloudPos.z, numClouds);
    }

    public void generateClouds(int x, int y, int z, int numClouds = 5)
    {
        for (int c = 0; c < numClouds; c++)
        {

        }
    }

    public void buildRoom(int x, int y, int z)
    {
        this.setBlock(x, y, z, Block.air);
        this.setBlock(x, y + 1, z, Block.air);
        this.setBlock(x + 1, y, z + 1, Block.air);
        this.setBlock(x + 1, y + 1, z + 1, Block.air);
        this.setBlock(x - 1, y, z - 1, Block.air);
        this.setBlock(x - 1, y + 1, z - 1, Block.air);
        this.setBlock(x + 1, y, z - 1, Block.air);
        this.setBlock(x + 1, y + 1, z - 1, Block.air);
        this.setBlock(x - 1, y, z + 1, Block.air);
        this.setBlock(x - 1, y + 1, z + 1, Block.air);
        this.setBlock(x - 1, y, z, Block.air);
        this.setBlock(x - 1, y + 1, z, Block.air);
        this.setBlock(x, y, z - 1, Block.air);
        this.setBlock(x, y + 1, z - 1, Block.air);
        this.setBlock(x + 1, y, z, Block.air);
        this.setBlock(x + 1, y + 1, z, Block.air);
        this.setBlock(x, y, z + 1, Block.air);
        this.setBlock(x, y + 1, z + 1, Block.air);
    }

    public void makeCaves(int x, int y, int z)
    {
        this.buildRoom(x, y, z);
        this.buildRoom(x + 3, y, z);
        this.buildRoom(x - 3, y, z);
        this.buildRoom(x, y, z + 3);
        this.buildRoom(x, y, z - 3);
        this.buildRoom(x + 3, y, z + 3);
        this.buildRoom(x - 3, y, z - 3);
        this.buildRoom(x + 3, y, z - 3);
        this.buildRoom(x - 3, y, z + 3);
        this.buildRoom(x, y + 3, z);

        if (y > minCaveY)
        {
            this.buildRoom(x, y - rand.Next(1, 3), z);
        }
    }

    public void generateCaveSystem(int x, int y, int z)
    {
        if (Utils.getRNG(40))
        {
            this.makeCaves(x, y, z);
        }

        if (Utils.getRNG(50))
        {
            this.makeCaves(x - 3, y, z);
        }

        if (Utils.getRNG(50))
        {
            this.makeCaves(x + 3, y, z);
        }

        if (Utils.getRNG(50))
        {
            this.makeCaves(x, y, z + 3);
        }

        if (Utils.getRNG(50))
        {
            this.makeCaves(x, y, z - 3);
        }

        if (Utils.getRNG(75))
        {
            this.makeCaves(x + 3, y, z + 3);
        }

        if (Utils.getRNG(75))
        {
            this.makeCaves(x + 3, y, z - 3);
        }

        if (Utils.getRNG(75))
        {
            this.makeCaves(x - 3, y, z + 3);
        }

        if (Utils.getRNG(75))
        {
            this.makeCaves(x - 3, y, z - 3);
        }
    }

    public static int getChunkHeight()
    {
        return chunkHeight;
    }

    public static int getChunkWidth()
    {
        return chunkWidth;
    }

    public void setChunkHeight(int ch)
    {
        chunkHeight = ch;
    }

    internal void setBlock(int x, int y, int z, Block block)
    {
        try
        {
            block.x = x;
            block.y = y;
            block.z = z;

            block.chunkInstance = this;

            chunkBlocks.Add(block);

            blocks[x, y, z] = block;

            this.needsToUpdate = true;

            if (block.getNeedsToUpdate())
            {
                block.onUpdate();

                if (getBlockAt(x - 1, y, z) != null && getBlockAt(x - 1, y, z).getNeedsToUpdate())
                {
                    getBlockAt(x - 1, y, z).onUpdate();
                }

                if (getBlockAt(x + 1, y, z) != null && getBlockAt(x + 1, y, z).getNeedsToUpdate())
                {
                    getBlockAt(x + 1, y, z).onUpdate();
                }

                if (getBlockAt(x, y, z - 1) != null && getBlockAt(x, y, z - 1).getNeedsToUpdate())
                {
                    getBlockAt(x, y, z - 1).onUpdate();
                }

                if (getBlockAt(x, y, z + 1) != null && getBlockAt(x, y, z + 1).getNeedsToUpdate())
                {
                    getBlockAt(x, y, z + 1).onUpdate();
                }

                if (getBlockAt(x, y + 1, z) != null && getBlockAt(x, y + 1, z).getNeedsToUpdate())
                {
                    getBlockAt(x, y + 1, z).onUpdate();
                }

                if (getBlockAt(x, y - 1, z) != null && getBlockAt(x, y - 1, z).getNeedsToUpdate())
                {
                    getBlockAt(x, y - 1, z).onUpdate();
                }
            }
        }
        catch (Exception ex)
        {
            if (GameSettings.enableDebugMode)
            {
                Logger.Log(ex.Message + " \r\n " + ex.StackTrace);
            }
        }
    }

    internal void setBlock(Vector3 blockPos, Block block)
    {
        int x = (int) blockPos.x;
        int y = (int) blockPos.y;
        int z = (int) blockPos.z;

        setBlock(x, y, z, block);
    }

    internal void removeBlock(int x, int y, int z)
    {
        if (getBlockAt(x, y, z).getIsBreakable())
        {
            setBlock(x, y, z, Block.air);
        }
    }

    internal void removeBlock(Vector3 blockPos)
    {
        int x = (int) blockPos.x;
        int y = (int) blockPos.y;
        int z = (int) blockPos.z;

        removeBlock(x, y, z);
    }


    public Block getBlockAt(int posX, int posY, int posZ)
    {
        Block block = blocks[posX, posY, posZ];
        return block;
    }

    public Block getBlockAt(Vector3 roundedPosition)
    {
        Block block = getBlockAt((int) roundedPosition.x, (int) roundedPosition.y, (int) roundedPosition.z);
        return block;
    }

    public Block getRandomOre(int max = 500)
    {
        foreach (Block b in Block.instance.BlockList)
        {
            if (b is BlockOre)
            {
                BlockOre bo = (BlockOre) b;
                int randGen = rand.Next(max);
                if (randGen <= bo.getOreWeight())
                {
                    return bo;
                }
            }
        }
        return Block.stone;
    }

    public void Tick()
    {

    }

    public virtual void Update()
    {
        if (needsToUpdate)
        {
            if (!drawnLock && !renderLock)
            {
                hasDrawn = false;
                hasRendered = false;
                needsToUpdate = false;
            }
        }

        if (!hasDrawn && hasGenerated && !drawnLock)
        {
            drawnLock = true;
            data = new MeshData();

            for (int x = 0; x < getChunkWidth(); x++)
            {
                for (int y = 0; y < getChunkHeight(); y++)
                {
                    for (int z = 0; z < getChunkWidth(); z++)
                    {
                        data.Merge(blocks[x, y, z].Draw(this, blocks, x, y, z));
                    }
                }
            }
            drawnLock = false;
            hasDrawn = true;
        }

        foreach (Block b in new List<Block>(chunkBlocks))
        {
            if (b.getNeedsToUpdate())
            {
                int randGen = rand.Next(1600);
                if (randGen == 5)
                {
                    b.onUpdate();
                }
            }
        }
    }

    public virtual void OnUnityUpdate()
    {
        if (hasGenerated && !hasRendered && hasDrawn && !renderLock)
        {
            hasRendered = true;
            renderLock = true;

            Mesh mesh = data.ToMesh();

            if (go == null)
            {
                go = new GameObject();
                go.name = "Chunk";

            }

            Transform t = go.transform;

            if (t.gameObject.GetComponent<MeshFilter>() == null)
            {
                t.gameObject.AddComponent<MeshFilter>();
                t.gameObject.AddComponent<MeshRenderer>();
                t.gameObject.AddComponent<MeshCollider>();

                t.gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("ChunkMat");
                t.transform.position = new Vector3(posX * chunkWidth, 0, posZ * chunkWidth);

                Texture2D tmp = new Texture2D(0, 0);
                tmp.LoadImage(File.ReadAllBytes("atlas.png"));
                tmp.filterMode = FilterMode.Point;

                GameObject to = t.gameObject;

                MeshRenderer meshRenderer = to.GetComponent<MeshRenderer>();
                Material meshMaterial = meshRenderer.material;
                meshMaterial.mainTexture = tmp;
            }

            t.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            t.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

            renderLock = false;

            if (isFirstChunk)
            {
                string path = FileManager.getPlayerPosString();
                Vector3 defaultStartPos = new Vector3(posX * chunkWidth, 100, posZ * chunkWidth);

                Int3 defaultPos;

                if (File.Exists(path))
                {
                    try
                    {
                        Logger.Log("Attempting to load Player...");
                        defaultPos = Serializer.Deserialize_FromFile<Int3>(path);
                        defaultStartPos = new Vector3(defaultPos.x, defaultPos.y, defaultPos.z);
                        Logger.Log("Player loaded at: " + defaultStartPos.x + " " + defaultStartPos.y + " " + defaultStartPos.z + ".");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error when loading Player: " + ex.Message + " \r\n " + ex.StackTrace);
                    }
                }

                if (chunksHaveGenerated)
                {
                    GameManager.instance.StartPlayer(defaultStartPos);
                }
            }
        }
    }
}