using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    //An instance of our EntityPlayer class.
    private static EntityPlayer playerInstance = EntityPlayer.instance;

    //The inventory slot number to look at.
    int slotNum = 0;

    //The idea of this is that the EntityPlayer.cs IGameInventoryCompatiable Inventory list could be modified to contain
    //these InventorySlot entries instead, and each of these 'Slots' could contain details regarding what item or block
    //is inside of that slot. Furthermore, it could control how many of each item there are in each slot. For instance,
    //rather than dirt taking up two different slots, it could take up only one, unless there was 99 of it, in which
    //case it would take up another slot and then begin to count again. Those details could be held here. Currently only
    //in the thinking and planning stages. To do.

    public InventorySlot(int numSlot)
    {
        this.slotNum = numSlot;
    }

    public int getInventorySlotNum()
    {
        return this.slotNum;
    }

    public VoxObject getItemInSlot(int index)
    {
        return playerInstance.Inventory[index];
    }

    public int getSlotForItem(VoxObject item)
    {
        foreach (VoxObject v in playerInstance.Inventory){
            if (v.Equals(item))
            {
                return playerInstance.Inventory.IndexOf(v);
            }
        }
        return -1; //This should throw an error.
    }
}