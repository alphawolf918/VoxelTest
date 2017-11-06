using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class EntityPlayer : MonoBehaviour
{

    public GameObject Add;
    public GameObject Delete;

    public List<VoxObject> Inventory = new List<VoxObject>();

    public List<InventorySlot> slottedInventory = new List<InventorySlot>();

    public static EntityPlayer instance;

    private float maxDistance = 25.0f;

    public Block activeBlock = Block.orange;
    public int activeSlot = 0;

    private float maxHealth = 20.0f;
    private float currentHealth = 20.0f;

    private World worldObj;
    private Chunk chunkObj;

    public static Chunk chunkInstance = null;

    public void Start()
    {
        instance = this;
        Add = Instantiate(Resources.Load<GameObject>("Add"), this.transform.position, Quaternion.identity) as GameObject;
        Delete = Instantiate(Resources.Load<GameObject>("Delete"), this.transform.position, Quaternion.identity) as GameObject;

        activeSlot = 0;
        activeBlock = Block.orange;
        //Utils.ToBlock(Inventory[activeSlot]);
    }

    public static EntityPlayer getInstance()
    {
        return instance;
    }

    public World getWorldObj()
    {
        return this.worldObj;
    }

    public void setWorldObj(World world)
    {
        this.worldObj = world;
    }

    public Chunk getChunkObj()
    {
        return this.chunkObj;
    }

    public void setChunkObj(Chunk chunk)
    {
        this.chunkObj = chunk;
    }

    public VoxObject getInventoryItem(int slot)
    {
        return this.Inventory[slot];
    }

    public InventorySlot getInventorySlot(int slot)
    {
        return this.slottedInventory[slot];
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = true;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, this.maxDistance))
            {
                Vector3 rawPosition = hit.point + hit.normal;
                Vector3 roundedPosition = new Vector3(Mathf.RoundToInt(rawPosition.x), Mathf.RoundToInt(rawPosition.y), Mathf.RoundToInt(rawPosition.z));
                Add.transform.position = roundedPosition;
                if (this.activeBlock != null)
                {
                    if (Inventory.Contains(this.activeBlock))
                    {
                        int blockIndex = Inventory.IndexOf(Inventory.Find(x => x.getName() == this.activeBlock.getName()));
                        Block invBlock = (Block) Inventory[blockIndex];
                        MathHelper.AddBlock(roundedPosition, invBlock, instance);
                        int c = invBlock.getCurrentStackSize();
                        if (c > 1)
                        {
                            invBlock.decreaseCurrentStackSize(1);
                            Logger.Log("Removed 1 block of " + invBlock.getBlockName() + " from your inventory.");
                        }
                        else
                        {
                            Inventory.Remove(invBlock);
                            Logger.Log("Removed " + invBlock.getBlockName() + " from your inventory.");
                        }
                    }
                    else
                    {
                        Logger.Log("That block (" + this.activeBlock.getBlockName() + ") is not in your inventory.");
                    }
                }
                else
                {
                    Logger.Log("Active block is null.");
                }
            }
        }
        else
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = true;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, this.maxDistance))
            {
                Vector3 rawPosition = hit.point - hit.normal;
                Vector3 roundedPosition = new Vector3(Mathf.RoundToInt(rawPosition.x), Mathf.RoundToInt(rawPosition.y), Mathf.RoundToInt(rawPosition.z));

                Delete.transform.position = roundedPosition;

                MathHelper.RemoveBlock(roundedPosition, instance);
            }
        }
        else
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 newPos = new Vector3(GameManager.instance.playerPos.x, 100, GameManager.instance.playerPos.z);
            GameManager.instance.PlayerCharacter.transform.position = newPos;
            Logger.Log("Player reset to: " + (int) newPos.x + " " + (int) newPos.y + " " + (int) newPos.z);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.activeSlot = 0;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.orange;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.activeSlot = 1;
            // this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.green;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.activeSlot = 2;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.gray2;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.activeSlot = 3;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.gray;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            this.activeSlot = 4;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.peach;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            this.activeSlot = 5;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.darkgray;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            this.activeSlot = 6;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.black;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            this.activeSlot = 7;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.yellow;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            this.activeSlot = 8;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.white;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            this.activeSlot = 9;
            //this.activeBlock = Utils.ToBlock(Inventory[activeSlot]);
            this.activeBlock = Block.purple;

            Logger.Log("Active block set to " + activeBlock.getBlockName());
        }

        if (GameManager.instance.playerPos.y <= 0)
        {
            if (Utils.getRNG(40))
            {
                this.damagePlayer(1.5f);
            }
        }


        if (this.getHealth() <= 0.0f)
        {
            Vector3 newPos = new Vector3(0, Chunk.getChunkHeight() + 60, 0);
            GameManager.instance.PlayerCharacter.transform.position = newPos;
            this.Inventory.Clear();
            Logger.Log("You have DIED!");
            this.setHealth(this.getMaxHealth());
        }
    }

    public Block getActiveBlock()
    {
        return this.activeBlock;
    }

    public void setActiveBlock(Block block)
    {
        this.activeBlock = block;
    }

    public float getHealth()
    {
        return this.currentHealth;
    }

    public void setHealth(float h)
    {
        this.currentHealth = h;
    }

    public void damagePlayer(float dmg)
    {
        float h = (currentHealth - dmg);
        h = (h < 0) ? 0 : h;
        this.setHealth(h);
    }

    public void healPlayer(float hl)
    {
        float h = (currentHealth + hl);
        h = (h > maxHealth) ? maxHealth : h;
        this.setHealth(h);
    }

    public void setDead()
    {
        this.setHealth(0.0f);
    }

    public float getMaxHealth()
    {
        return this.maxHealth;
    }

    public void setMaxHealth(float m)
    {
        this.maxHealth = m;
    }

    public void increaseMaxHealth(float m)
    {
        this.maxHealth += m;
    }

    public void decreaseMaxHealth(float d)
    {
        if ((maxHealth - d) > 20)
        {
            this.maxHealth -= d;
        }
    }
}