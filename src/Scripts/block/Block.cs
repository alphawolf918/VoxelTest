using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : VoxObject
{
    private bool isTransparent;

    public static Block air = new Block("air", true);
    public static Block yellow = new BlockSavannahGrass().setDroppedBlock(orange);
    public static Block orange = new Block("orange");
    public static Block green = new BlockGrass().setDroppedBlock(orange);
    public static Block gray = new Block("gray");
    public static Block peach = new BlockFalling("peach");
    public static Block lightgray = new Block("lightgray");
    public static Block darkgray = new BlockFalling("darkgray");
    public static Block gray2 = new Block("gray2").setDroppedBlock(gray);
    public static Block black = new Block("black");
    public static Block brown = new Block("brown");
    public static Block lightgreen = new Block("lightgreen");
    public static Block red = new Block("red");
    public static Block darkwhite = new Block("darkwhite");
    public static Block lightbrown = new Block("lightbrown");
    public static Block ice = new Block("ice").setSlippery();
    public static Block white = new BlockSnow();
    public static Block cloud = new Block("cloud").setDropsNothing();
    public static Block darkred = new Block("darkred");
    public static Block darkbrown = new Block("darkbrown");
    public static Block purple = new Block("purple");
    public static Block candy = new Block("pink");
    public static Block chocolate = new Block("chocolate");
    public static Block cyan = new Block("cyan");
    public static Block darkblue = new Block("darkblue");
    public static Block endrock = new Block("endrock").setUnbreakable();
    public static Block darkcyan = new Block("darkcyan");

    protected bool isBreakable = true;
    protected bool isSlippery = false;
    protected bool isRadioactive = false;
    protected bool isToxic = false;

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
        addBlock(green);
        addBlock(orange);
        addBlock(gray);
        addBlock(peach);
        addBlock(lightgray);
        addBlock(darkgray);
        addBlock(gray2);
        addBlock(black);
        addBlock(brown);
        addBlock(lightgreen);
        addBlock(red);
        addBlock(darkwhite);
        addBlock(lightbrown);
        addBlock(yellow);
        addBlock(ice);
        addBlock(white);
        addBlock(darkred);
        addBlock(darkbrown);
        addBlock(purple);
        addBlock(candy);
        addBlock(chocolate);
        addBlock(cyan);
        addBlock(darkblue);
        addBlock(endrock);
        addBlock(darkcyan);

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

    public virtual void onWalkedOver(EntityPlayer player)
    {
        Block block = this;
        if (block.getIsSlippery())
        {
            GameManager gameMgr = GameManager.instance;
            Vector3 playerPos = gameMgr.playerPos;
            //TODO
        }
    }

    public bool getIsSlippery()
    {
        return this.isSlippery;
    }

    public Block setSlippery(bool slippery = true)
    {
        this.isSlippery = slippery;
        return this;
    }

    public bool getIsRadioactive()
    {
        return this.isRadioactive;
    }

    public Block setRadioactive(bool radioactive = true)
    {
        this.isRadioactive = radioactive;
        return this;
    }

    public bool getIsToxic()
    {
        return this.isToxic;
    }

    public Block setToxic(bool toxic = true)
    {
        this.isToxic = toxic;
        return this;
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

    public Block setDropsNothing()
    {
        this.setDroppedBlock(air);
        return this;
    }

    public bool doesDropBlockNothing()
    {
        return (this.getDroppedBlock() == air);
    }

    public override void Start()
    {

    }

    public override void Tick()
    {

    }

    public override void Update()
    {
        if (this.needsToUpdate)
        {
            this.onUpdate();
        }
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