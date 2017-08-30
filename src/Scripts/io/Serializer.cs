using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{

	public static bool Check_Gen_Folder(string path)
	{
		if (Directory.Exists(path))
		{
			return true;
		}
		else
		{
			try
			{
				Directory.CreateDirectory(path);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Log("Exception when creating directory: " + ex.StackTrace);
			}
		}

		return false;
	}

	public static void Serialize_ToFile<T>(string path, string fileName, string extension, T _DATA) where T : class
	{

		if (Check_Gen_Folder(path))
		{
			try
			{
				using (Stream s = File.OpenWrite(string.Format("{0}{1}.{2}", path, fileName, extension)))
				{
					BinaryFormatter f = new BinaryFormatter();
					f.Serialize(s, _DATA);
				}
			}
			catch (Exception ex)
			{
				Logger.Log("Exception when serializing data stream: " + ex.StackTrace);
			}
		}
		else
		{
			throw new Exception("Cannot get correct directory.");
		}
	}

	public static void Serialize_ToFile_FullPath<T>(string path, T _DATA) where T : class
	{
		try
		{
			using (Stream s = File.OpenWrite(path))
			{
				BinaryFormatter f = new BinaryFormatter();
				f.Serialize(s, _DATA);
			}
		}
		catch (Exception ex)
		{
			Logger.Log("Exception when serializing data stream: " + ex.Message + " \r\n " + ex.ToString());
		}
	}

	public static T Deserialize_FromFile<T>(string path) where T : class
	{
		if (File.Exists(path))
		{
			try
			{
				using (Stream s = File.OpenRead(path))
				{
					BinaryFormatter f = new BinaryFormatter();
					return f.Deserialize(s) as T;
				}
			}
			catch (Exception ex)
			{
				Logger.Log("Exception when deserializing data stream: " + ex.StackTrace);
			}
		}
		else
		{
			throw new Exception("File cannot be found.");
		}

		return null;
	}

}