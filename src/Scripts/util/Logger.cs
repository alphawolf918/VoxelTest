using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : ILoopable
{

	public static Logger mainLog = new Logger();
	private static List<string> logText = new List<string>();

	public static void Instantiate()
	{
		MainLoopable.instance().registerLoopable(mainLog);
	}

	public static void Log(string strMessage)
	{
		mainLog.log(strMessage);
	}

	public static void Log(Exception e)
	{
		mainLog.log(e);
	}

	private void log(string strMessage)
	{
		string strNewMessage = "[" + DateTime.Now + "]: " + strMessage;
		logText.Add(strNewMessage);
		Debug.Log(strNewMessage);
	}

	private void log(Exception e)
	{
		logText.Add(e.StackTrace.ToString());
	}

	public void Start()
	{
		//
	}

	public void Update()
	{
		File.WriteAllLines("logs/Log.txt", new List<string>(logText).ToArray());
	}

	public void OnApplicationQuit()
	{
		//
	}
}