using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBase
{

    public Block topBlock = Block.green;
    public Block fillerBlock = Block.orange;
    public Block stoneBlock = Block.gray2;

    private string biomeName = "Unknown";

    private float biomeTemp = 65.2f;

    public static BiomeBase biomePlains = new BiomeBase("Plains").setBiomeTemp(71.3f).setBiomeWeight(40).setChunkHeightDifference(8);
    public static BiomeBase biomeSavannah = new BiomeBase("Savannah").setTopBlock(Block.yellow).setBiomeTemp(96.2f).setBiomeWeight(35).setChunkHeightDifference(4);
    public static BiomeBase biomeDesert = new BiomeBase("Desert").setTopBlock(Block.peach).setFillerBlock(Block.lightbrown).setBiomeTemp(102.5f).setBiomeWeight(26).setChunkHeightDifference(2);
    public static BiomeBase biomeMushroomHills = new BiomeBase("Mushroom Hills").setTopBlock(Block.red).setFillerBlock(Block.darkwhite).setBiomeTemp(45.2f).setBiomeWeight(10).setChunkHeightDifference(3);
    public static BiomeBase biomeIcyMeadow = new BiomeBase("Icy Meadow").setTopBlock(Block.ice).setFillerBlock(Block.ice).setBiomeTemp(-2.0f).setBiomeWeight(5).setChunkHeightDifference(8);
    public static BiomeBase biomeTundra = new BiomeBase("Tundra").setTopBlock(Block.white).setFillerBlock(Block.ice).setBiomeTemp(25.6f).setBiomeWeight(6).setChunkHeightDifference(7);
    public static BiomeBase biomeNetherlands = new BiomeBase("Netherlands").setTopBlock(Block.darkred).setFillerBlock(Block.darkbrown).setBiomeTemp(134.7f).setBiomeWeight(5).setChunkHeightDifference(2);
    public static BiomeBase biomeEndlands = new BiomeBase("Endlands").setTopBlock(Block.purple).setFillerBlock(Block.black).setBiomeTemp(62.7f).setBiomeWeight(3).setChunkHeightDifference(8);
    public static BiomeBase biomeCandyLand = new BiomeBase("Candy Land").setTopBlock(Block.candy).setFillerBlock(Block.white).setStoneBlock(Block.chocolate).setBiomeTemp(52.2f).setBiomeWeight(4).setChunkHeightDifference(10);
    public static BiomeBase biomeEnchantedHills = new BiomeBase("Enchanted Hills").setTopBlock(Block.darkblue).setFillerBlock(Block.darkcyan).setBiomeTemp(44.2f).setBiomeWeight(6).setChunkHeightDifference(1);

    private List<BiomeBase> BiomeList = new List<BiomeBase>();
    private int chunkHeightDifference = 0;
    private int biomeWeight = 50;
    public static BiomeBase instance;
    private static System.Random rand = new System.Random();

    public BiomeBase(string strName)
    {
        instance = this;
        this.biomeName = strName;

        Logger.Log("Loaded biome: " + this.getBiomeName());
    }

    //Make sure to add new biomes to BiomeList array or
    //they will not show up.
    public static void init()
    {
        addBiome(biomePlains);
        addBiome(biomeSavannah);
        addBiome(biomeDesert);
        addBiome(biomeMushroomHills);
        addBiome(biomeIcyMeadow);
        addBiome(biomeTundra);
        addBiome(biomeNetherlands);
        addBiome(biomeEndlands);
        addBiome(biomeCandyLand);
        addBiome(biomeEnchantedHills);

        Logger.Log("Loaded " + instance.BiomeList.Count + " biomes.");
    }

    private static void addBiome(BiomeBase biome)
    {
        instance.BiomeList.Add(biome);
    }

    public virtual void decorate(Chunk chunk, int x, int y, int z)
    {
        if ((this.getBiome() == biomePlains || this.getBiome() == biomeSavannah || this.getBiome() == biomeTundra) && Utils.getRNG(20))
        {
            if (chunk.getBlockAt(x, y + 1, z) == Block.air)
            {
                new WorldGenTree().generate(chunk, x, y, z, new System.Random());
            }
        }
    }

    public virtual BiomeBase decorate(int x, int y, int z, Action decorateAction)
    {
        decorateAction();
        return this;
    }

    public virtual BiomeBase getRandomBiome()
    {
        BiomeBase defaultBiome = biomePlains;
        foreach (BiomeBase b in BiomeList)
        {
            int bw = b.getBiomeWeight();
            int randGen = rand.Next(0, 100);
            if (randGen <= bw)
            {
                return b;
            }
        }
        return defaultBiome;
    }

    public virtual BiomeBase getWeightedRandomBiome()
    {
        BiomeBase defaultBiome = biomePlains;
        int randGen = rand.Next(0, 100);
        BiomeBase biome = (from b in BiomeList
                           where b.biomeWeight <= randGen
                           orderby rand.Next()
                           select b).First();
        if (biome != null)
        {
            return biome;
        }
        return defaultBiome;
        //return this.getRandomBiome();
    }

    public List<BiomeBase> getBiomeList()
    {
        return BiomeList;
    }

    public int getBiomeWeight()
    {
        return this.biomeWeight;
    }

    public virtual BiomeBase setBiomeWeight(int bw)
    {
        this.biomeWeight = bw;
        return this;
    }

    public int getChunkHeightDifference()
    {
        return this.chunkHeightDifference;
    }

    public BiomeBase setChunkHeightDifference(int cd)
    {
        this.chunkHeightDifference = cd;
        return this;
    }

    public float getBiomeTemp()
    {
        return this.biomeTemp;
    }

    public virtual BiomeBase setBiomeTemp(float fltTemp)
    {
        this.biomeTemp = fltTemp;
        return this;
    }

    public bool isColdBiome()
    {
        float temp = this.getBiomeTemp();
        if (temp <= 32.0f)
        {
            return true;
        }
        return false;
    }

    public bool isHotBiome()
    {
        float temp = this.getBiomeTemp();
        if (temp >= 95.0f)
        {
            return true;
        }
        return false;
    }

    public string getBiomeName()
    {
        return this.biomeName;
    }

    public virtual BiomeBase setBiomeName(string strName)
    {
        this.biomeName = strName;
        return this;
    }

    public BiomeBase getBiome()
    {
        return this;
    }

    public Block getTopBlock()
    {
        return this.topBlock;
    }

    public virtual BiomeBase setTopBlock(Block tBlock)
    {
        this.topBlock = tBlock;
        return this;
    }

    public Block getFillerBlock()
    {
        return this.fillerBlock;
    }

    public virtual BiomeBase setFillerBlock(Block fBlock)
    {
        this.fillerBlock = fBlock;
        return this;
    }

    public Block getStoneBlock()
    {
        return this.stoneBlock;
    }

    public virtual BiomeBase setStoneBlock(Block sBlock)
    {
        this.stoneBlock = sBlock;
        return this;
    }

}