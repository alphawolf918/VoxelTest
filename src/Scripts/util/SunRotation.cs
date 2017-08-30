using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SunRotation : MonoBehaviour
{

	public float sunRotationTime = 0.3f;
	public static Vector3 worldTime;
	static float deltaTime;

	//string path = FileManager.getWorldTimeString();

	void Start()
	{
		//if (File.Exists(path))
		//{
		//	WorldTime worldTimeNew = Serializer.Deserialize_FromFile<WorldTime>(path);
		//	worldTime = new Vector3(worldTimeNew.x, worldTimeNew.y, worldTimeNew.z);
		//	deltaTime = worldTimeNew.timeFloat;
		//}
		//else
		//{
		//	deltaTime = Time.deltaTime;
		//	worldTime = new Vector3(0, sunRotationTime, 0) * deltaTime;
		//}
	}

	void Update()
	{

	//	worldTime = new Vector3(worldTime.x, worldTime.y + sunRotationTime, worldTime.z) * deltaTime;
		transform.Rotate(new Vector3(0, sunRotationTime, 0) * Time.deltaTime);
	}
}