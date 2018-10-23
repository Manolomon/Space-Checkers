using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

	private Dictionary<string, string> localizedText;

	// Use this for initialization
	void Start () 
	{

	}

	public void LoadLocalizedText (string filename)
	{
		localizedText = new Dictionary<string, string> ();
		string filePath = Path.Combine(Application.streamingAssetsPath, filename);
		if (File.Exists (filePath))
		{
			string dataAsJason = File.ReadAllText (filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJason);
			
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			Debug.Log ("Data loaded, dictionary contains" + localizedText.Count + " entries");
		}
		else
		{
			Debug.LogError("Cannot find language file");
		}
	}
}
