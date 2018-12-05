using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigManager : MonoBehaviour {

	public static ConfigManager instance;
	private Dictionary<string, string> settings;
	private bool isReady = false;
	private string missingTextString = "Setting not found";

	void Awake () 
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
		LoadSettings("config.json");
	}
	

	public void LoadSettings (string filename)
	{
		settings = new Dictionary<string, string> ();
		string settingsPath = Path.Combine("Config", filename);
		string filePath = Path.Combine(Application.streamingAssetsPath, settingsPath);
		if (File.Exists (filePath))
		{
			string dataAsJason = File.ReadAllText (filePath);
			ConfigData loadedData = JsonUtility.FromJson<ConfigData>(dataAsJason);
			
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				settings.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			Debug.Log ("Config data loaded, " + filename + " dictionary contains " + settings.Count + " entries");
		}
		else
		{
			Debug.LogError("Cannot find language file");
		}
		isReady = true;
	}

	public string GetConfigValue (string key)
	{
		string result = missingTextString;
		if (settings.ContainsKey (key))
		{
			result = settings[key];
		}
		return result;
	}

	public bool GetIsReady ()
	{
		return isReady;
	}
}
