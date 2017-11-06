using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private List<Delegate> _Delegates = new List<Delegate>();

    private MainLoopable main;

    public Camera splashCamera;
    public Image logoImage;
    public Text healthText;
    public Text biomeText;
    public Image healthImage;

    public static float Sdx = 50;
    public static float Sdz = 50;
    public static float Smy = MathHelper.GenRandFloat(0.26f, 0.32f);
    public static float Scutoff = MathHelper.GenRandFloat(2.1f, 2.2f);
    public static float Smul = 1.2f;

    public static GameManager instance;

    public GameObject PlayerCharacter;
    public Vector3 playerPos;

    private bool IsPlayerLoaded = false;

    public static Chunk chunkInstance;

    public void registerDelegate(Delegate d)
    {
        _Delegates.Add(d);
    }

    public void StartPlayer(Vector3 startVector)
    {
        if (splashCamera != null)
        {
            Destroy(splashCamera);
        }

        if (logoImage != null)
        {
            Destroy(logoImage);
        }

        try
        {
            GameObject t = Instantiate(Resources.Load<GameObject>("Player"), startVector, Quaternion.identity) as GameObject;
            t.transform.position = startVector;

            PlayerCharacter = t;
        }
        catch (Exception ex)
        {
            Logger.Log("Error when initializing Player: " + ex.Message + " \r\n " + ex.StackTrace);
        }

        healthImage.enabled = true;
        healthText.enabled = true;
        biomeText.enabled = true;
    }

    void Start()
    {
        FileManager.RegisterFiles();
        instance = this;
        TextureAtlas._Instance.CreateAtlas();
        MainLoopable.Instantiate();
        main = MainLoopable.instance();
        init();
        main.Start();
    }

    public static void init()
    {
        Block.init();
        Item.init();
        BiomeBase.init();
    }

    void Update()
    {
        if (PlayerCharacter != null)
        {
            playerPos = PlayerCharacter.transform.position;
            IsPlayerLoaded = true;
        }

        main.Update();

        foreach (Delegate d in new List<Delegate>(_Delegates))
        {
            d.DynamicInvoke();
            _Delegates.Remove(d);
        }


        if (EntityPlayer.instance != null)
        {
            EntityPlayer characterInstance = EntityPlayer.instance;
            this.healthText.text = characterInstance.getHealth().ToString();
        }
    }

    public void exitGame1()
    {
        OnApplicationQuit();
    }

    void OnApplicationQuit()
    {
        this.saveData();
        main.OnApplicationQuit();
    }

    void saveData()
    {
        try
        {
            Logger.Log("Saving Player location at X: " + (int) playerPos.x + " Y: " + (int) playerPos.y + " Z: " + (int) playerPos.z + " ...");
            Serializer.Serialize_ToFile_FullPath(FileManager.getPlayerPosString(), new Int3((int) playerPos.x, (int) playerPos.y, (int) playerPos.z));
            Logger.Log("Saved Player location.");
        }
        catch (Exception ex)
        {
            Logger.Log("Error when saving Player coordinates: " + ex.Message + " \r\n " + ex.StackTrace);
        }

        try
        {
            Logger.Log("Saving world time...");
            Serializer.Serialize_ToFile_FullPath(FileManager.getWorldTimeString(), new WorldTime(new Int3(SunRotation.worldTime), Time.deltaTime));
            Logger.Log("Saved world time.");
        }
        catch (Exception ex)
        {
            Logger.Log("Error when saving world time: " + ex.Message + " \r\n " + ex.StackTrace);
        }
    }

    internal static void exitGame()
    {
        instance.exitGame1();
    }

    internal static bool playerLoaded()
    {
        return instance.IsPlayerLoaded;
    }
}