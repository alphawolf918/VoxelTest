using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOre : Block
{

    private int oreWeight = 25;

    public static BlockOre oreInstance;

    public BlockOre(string textureName) : base(textureName)
    {
        oreInstance = this;
    }

    public int getOreWeight()
    {
        return this.oreWeight;
    }

    public Block setOreWeight(int weight)
    {
        this.oreWeight = weight;
        return this;
    }

    public override void onBroken(EntityPlayer player)
    {
        base.onBroken(player);
        Logger.Log("You found an ore!");
    }

}