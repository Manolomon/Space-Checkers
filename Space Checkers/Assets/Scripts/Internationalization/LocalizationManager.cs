using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;
	private Dictionary<string, string> localizedText;
	private bool isReady = false;
	private string missingTextString = "Localized text not found";

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
	}

	public void LoadLocalizedText (string filename)
	{
		localizedText = new Dictionary<string, string> ();
		string translationPath = Path.Combine("Translations", filename);
		string filePath = Path.Combine(Application.streamingAssetsPath, translationPath);
		if (File.Exists (filePath))
		{
			string dataAsJason = File.ReadAllText (filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJason);
			
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			Debug.Log ("Data loaded, " + filename + " dictionary contains " + localizedText.Count + " entries");
		}
		else
		{
			Debug.LogError("Cannot find language file");
		}
		isReady = true;
	}

	public string GetLocalizedValue (string key)
	{
		string result = missingTextString;
		if (localizedText.ContainsKey (key))
		{
			result = localizedText[key];
		}
		return result;
	}

	public bool GetIsReady ()
	{
		return isReady;
	}
}
