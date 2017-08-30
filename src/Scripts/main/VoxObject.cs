using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VoxObject : ITickable, IGameInventoryCompatible
{

    int maxStackSize = 99;
    int currentStackSize = 0;
    string name;

    static VoxObject voxInstance;

    public string getName()
    {
        return name;
    }

    public void setName(string strName)
    {
        name = strName;
    }

    public static VoxObject getInstance()
    {
        return voxInstance;
    }

    public virtual int getMaxStackSize()
    {
        return this.maxStackSize;
    }

    public virtual int getCurrentStackSize()
    {
        return this.currentStackSize;
    }

    public virtual void setCurrentStackSize(int size)
    {
        this.currentStackSize = size;
    }

    public virtual void increaseCurrentStackSize(int bySize)
    {
        int s = currentStackSize + bySize;
        if (s > maxStackSize)
        {
            int m = (s - maxStackSize);
            s = maxStackSize;
            Block b = (Block) getInstance();
            b.currentStackSize = m;
            EntityPlayer.instance.Inventory.Add(b);
        }
        currentStackSize = s;
    }

    public virtual void decreaseCurrentStackSize(int bySize)
    {
        int s = currentStackSize - bySize;
        s = (s < 0) ? 0 : s;
        this.currentStackSize = s;
    }

    public virtual bool isInterfaceObject()
    {
        return true;
    }

    public virtual void Start()
    {
        voxInstance = this;
    }

    public virtual void Tick()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnUnityUpdate()
    {

    }

}