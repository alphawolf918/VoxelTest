using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager
{

    public static readonly string saveDir = "Saves/" + World.worldName + "/Data/";

    public static readonly string chunkSaveDir = saveDir + "Chunks/";
    public static readonly string playerSaveDir = saveDir + "Player/";
    public static readonly string worldSaveDir = saveDir + "WorldData/";

    public static string saveFileExt = "chk";

    public static void RegisterFiles()
    {
        Serializer.Check_Gen_Folder(chunkSaveDir);
        Serializer.Check_Gen_Folder(playerSaveDir);
        Serializer.Check_Gen_Folder(worldSaveDir);
    }

    public static string getChunkString(int x, int z)
    {
        return string.Format("{0}C{1}_{2}." + saveFileExt, chunkSaveDir, x, z);
    }

    public static string getPlayerPosString()
    {
        return string.Format("{0}PlayerPosition." + saveFileExt, playerSaveDir);
    }

    public static string getPlayerInvString()
    {
        return string.Format("{0}PlayerInventory." + saveFileExt, playerSaveDir);
    }

    public static string getWorldTimeString()
    {
        return string.Format("{0}SunRotation." + saveFileExt, worldSaveDir);
    }
}