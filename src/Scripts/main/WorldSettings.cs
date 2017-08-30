using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettings : MonoBehaviour {

    public string worldName = "world";
    private static WorldSettings INSTANCE;

	void Start () {
        INSTANCE = this;
	}

    public static WorldSettings instance()
    {
        return INSTANCE;
    }
}