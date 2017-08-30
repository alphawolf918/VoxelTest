using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : VoxObject
{

    public static Item coal = new Item("coal");
    public static Item ironIngot = new Item("iron_ingot");
    public static Item goldIngot = new Item("gold_ingot");
    public static Item redstone = new Item("redstone_dust");
    public static Item lapisLazuli = new Item("lapis_lazuli");
    public static Item diamond = new Item("diamond");
    public static Item emerald = new Item("emerald");

    public static Item instance;

    public List<Item> itemList = new List<Item>();

    public string itemName = "Undefined";

    public Item(string name)
    {
        instance = this;
        itemName = name;
        //string itemTextureName = "textures/items/" + name;
    }

    public static void init()
    {
        addItem(coal);
        addItem(ironIngot);
        addItem(goldIngot);
        addItem(redstone);
        addItem(lapisLazuli);
        addItem(diamond);
        addItem(emerald);
    }

    public static void addItem(Item item)
    {
        instance.itemList.Add(item);
    }

    public List<Item> getItemList()
    {
        return this.itemList;
    }

    public string getItemName()
    {
        return this.itemName;
    }

    public Item setItemName(string strName)
    {
        this.itemName = strName;
        return this;
    }

    public override void OnUnityUpdate()
    {

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

    public override bool isInterfaceObject()
    {
        return true;
    }

    public override int getMaxStackSize()
    {
        return 99;
    }
}
