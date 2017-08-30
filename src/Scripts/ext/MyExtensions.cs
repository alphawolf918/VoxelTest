using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MyExtensions
{

    public static int[,,] ToIntArray(this Block[,,] _ChunkData)
    {

        int lx = _ChunkData.GetLength(0);
        int ly = _ChunkData.GetLength(1);
        int lz = _ChunkData.GetLength(2);

        int[,,] data = new int[lx, ly, lz];

        for (int x = 0; x < lx; x++)
        {
            for (int y = 0; y < ly; y++)
            {
                for (int z = 0; z < lz; z++)
                {
                    data[x, y, z] = _ChunkData[x, y, z].getBlockID();
                }
            }
        }

        return data;
    }


    public static Block[,,] ToBlockArray(this int[,,] _data)
    {

        int lx = _data.GetLength(0);
        int ly = _data.GetLength(1);
        int lz = _data.GetLength(2);

        Block[,,] ChunkData = new Block[lx, ly, lz];

        for (int x = 0; x < lx; x++)
        {
            for (int y = 0; y < ly; y++)
            {
                for (int z = 0; z < lz; z++)
                {
                    ChunkData[x, y, z] = BlockRegistry.getBlockFromID(_data[x, y, z]);
                }
            }
        }
        return ChunkData;
    }

    private static System.Random rng = new System.Random();

    public static T RandomElement<T>(this IList<T> list)
    {
        return list[rng.Next(list.Count)];
    }

    public static T RandomElement<T>(this T[] array)
    {
        return array[rng.Next(array.Length)];
    }

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static float NextFloat(this System.Random random)
    {
        double mantissa = (random.NextDouble() * 2.0) - 1.0;
        double exponent = Math.Pow(2.0, random.Next(1, 2));
        return (float) (mantissa * exponent);
    }

    public static T GetRandomItem<T>(this System.Random _random, IEnumerable<T> itemsEnumerable, Func<T, int> weightKey)
    {
        var items = itemsEnumerable.ToList();

        var totalWeight = items.Sum(x => weightKey(x));
        var randomWeightedIndex = _random.Next(totalWeight);
        var itemWeightedIndex = 0;
        foreach (var item in items)
        {
            itemWeightedIndex += weightKey(item);
            if (randomWeightedIndex < itemWeightedIndex)
                return item;
        }
        throw new ArgumentException("Collection count and weights must be greater than 0");
    }

}