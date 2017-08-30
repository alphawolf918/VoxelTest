using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : VoxObject
{
    private bool isTransparent;

    public static Block air = new Block("air", true);
    public static Block dirt = new Block("dirt");
    public static Block grass = new BlockGrass().setDroppedBlock(dirt);
    public static Block cobblestone = new Block("cobblestone");
    public static Block sand = new BlockFalling("sand");
    public static Block clay = new Block("clay");
    public static Block gravel = new BlockFalling("gravel");
    public static Block stone = new Block("stone").setDroppedBlock(cobblestone);
    public static Block coal_ore = new BlockOre("coal_ore").setOreWeight(25);
    public static Block iron_ore = new BlockOre("iron_ore").setOreWeight(20);
    public static Block gold_ore = new BlockOre("gold_ore").setOreWeight(14);
    public static Block diamond_ore = new BlockOre("diamond_ore").setOreWeight(9);
    public static Block emerald_ore = new BlockOre("emerald_ore").setOreWeight(5);
    public static Block redstone_ore = new BlockOre("redstone_ore").setOreWeight(10);
    public static Block lapis_ore = new BlockOre("lapis_ore").setOreWeight(12);
    public static Block amaranth_ore = new BlockOre("amaranth_ore").setOreWeight(13);
    public static Block obsidian = new Block("obsidian");
    public static Block bedrock = new Block("bedrock").setUnbreakable();
    public static Block log_oak = new Block("log_oak");
    public static Block leaves_oak = new Block("leaves_oak");
    public static Block mushroom_top = new Block("mushroom_top");
    public static Block mushroom_stem = new Block("mushroom_stem");
    public static Block sandstone = new Block("sandstone");
    public static Block savannahGrass = new BlockSavannahGrass().setDroppedBlock(dirt);
    public static Block ice = new Block("ice");
    public static Block snow = new BlockSnow();
    public static Block cloud = new Block("cloud").setDroppedBlock(air);
    public static Block netherrack = new Block("netherrack");
    public static Block soul_sand = new Block("soul_sand");
    public static Block end_stone = new Block("end_stone");

    protected bool isBreakable = true;

    protected bool needsToUpdate = false;

    private Vector2[] UVMap;

    protected int blockID;
    protected string blockName;

    public static Block instance;

    private static int currentID = 0;

    public List<Block> BlockList = new List<Block>();

    public int x, y, z;

    public Chunk chunkInstance;

    public Block droppedBlock;
    public Item droppedItem;

    public Block(string name, bool transparent = false)
    {
        instance = this;
        this.isTransparent = transparent;
        setBlockName(name.Replace("textures/blocks/", ""));
        if (name != "air")
        {
            string blockTextureName = "textures/blocks/" + name + ".png";
            UVMap = UvMap.getUVMap(blockTextureName).UV_MAP;
        }
        this.droppedBlock = this;
        this.Register();
        Logger.Log("Loaded block: " + this.getBlockName());
    }

    public static void init()
    {
        addBlock(air);
        addBlock(grass);
        addBlock(dirt);
        addBlock(cobblestone);
        addBlock(sand);
        addBlock(clay);
        addBlock(gravel);
        addBlock(stone);
        addBlock(coal_ore);
        addBlock(iron_ore);
        addBlock(gold_ore);
        addBlock(diamond_ore);
        addBlock(emerald_ore);
        addBlock(redstone_ore);
        addBlock(lapis_ore);
        addBlock(amaranth_ore);
        addBlock(obsidian);
        addBlock(bedrock);
        addBlock(log_oak);
        addBlock(leaves_oak);
        addBlock(mushroom_top);
        addBlock(mushroom_stem);
        addBlock(sandstone);
        addBlock(savannahGrass);
        addBlock(ice);
        addBlock(snow);
        addBlock(netherrack);
        addBlock(soul_sand);
        addBlock(end_stone);
        Logger.Log("Loaded " + instance.BlockList.Count + " blocks.");
    }

    public Block getDroppedBlock()
    {
        return this.droppedBlock;
    }

    public Block setDroppedBlock(Block block)
    {
        this.droppedBlock = block;
        return this;
    }

    public virtual Vector2[] getUVMap()
    {
        return this.UVMap;
    }

    public void onPlaced(EntityPlayer player)
    {
        Logger.Log("Placed block: " + this.getBlockName());

    }

    public virtual void onUpdate()
    {
        //TODO
    }

    public virtual void onBroken(EntityPlayer player)
    {
        Block db = this.getDroppedBlock();
        if (db == air)
        {
            return;
        }
        player.Inventory.Add(db);
        Logger.Log("Added to your inventory: " + db.getBlockName());
    }

    public bool getNeedsToUpdate()
    {
        return this.needsToUpdate;
    }

    public Block setNeedsToUpdate(bool shouldUpdate = true)
    {
        this.needsToUpdate = shouldUpdate;
        return this;
    }

    static void addBlock(Block block)
    {
        instance.BlockList.Add(block);
    }

    private void Register()
    {
        blockID = currentID;
        currentID++;
        BlockRegistry.registerBlock(this);
    }

    public void setBlockName(string strName)
    {
        this.blockName = strName;
        setName(strName);
    }

    public string getBlockName()
    {
        return this.blockName;
    }

    public void setBlockID(int id)
    {
        blockID = id;
    }

    public int getBlockID()
    {
        return blockID;
    }

    public Block setBreakable(bool canBeBroken)
    {
        this.isBreakable = canBeBroken;
        return this;
    }

    public Block setUnbreakable()
    {
        this.setBreakable(false);
        return this;
    }

    internal bool getIsBreakable()
    {
        return this.isBreakable;
    }

    public bool getIsTransparent()
    {
        return this.isTransparent;
    }

    public override void Start()
    {

    }

    public override void Tick()
    {

    }

    public override void Update()
    {

    }

    public override void OnUnityUpdate()
    {

    }

    public virtual MeshData Draw(Chunk chunk, Block[,,] blocks, int x, int y, int z)
    {
        if (this.Equals(air))
        {
            return new MeshData();
        }

        try
        {
            return MathHelper.drawCube(chunk, blocks, this, x, y, z, this.UVMap);
        }

        catch (Exception ex)
        {
            Logger.Log(ex.StackTrace);
        }

        return new MeshData();
    }

    public override bool isInterfaceObject()
    {
        return true;
    }

    public override int getMaxStackSize()
    {
        return 99;
    }
}