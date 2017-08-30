using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{

    private static System.Random rand = new System.Random();

    public static bool getRNG(int max = 10, int seed = 5)
    {
        return (rand.Next(max) <= seed);
    }

    public static Block ToBlock(IGameInventoryCompatible inv)
    {
        return (Block) inv;
    }

    public static Item ToItem(IGameInventoryCompatible inv)
    {
        return (Item) inv;
    }

}