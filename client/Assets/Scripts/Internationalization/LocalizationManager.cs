using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>Gestor de lectura y asignación de los valores internacionalizados</summary>
public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;
	private Dictionary<string, string> localizedText;
	private bool isReady = false;
	private string missingTextString = "Localized text not found";

	 /// <summary>Función que valida que únicamente se cuente con una instancia en ejecución del GameObject LocalizationManager</summary>
	void Awake () 
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	 /// <summary>Se carga la información almacenada en el archivo .json</summary>
    /// <param name="filename">Nombre del archivo a cargar</param>
	public void LoadLocalizedText(string filename)
	{
		localizedText = new Dictionary<string, string> ();
		string translationPath = Path.Combine("Translations", filename);
		string filePath = Path.Combine(Application.streamingAssetsPath, translationPath);
		if (File.Exists(filePath))
		{
			string dataAsJason = File.ReadAllText(filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJason);
			
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}

			Debug.Log("Data loaded, " + filename + " dictionary contains " + localizedText.Count + " entries");
		}
		else
		{
			Debug.LogError("Cannot find language file");
		}
		isReady = true;
	}

	/// <summary>A partir de la información almacenada, se realiza una búsqueda de la llave y su valor asignado</summary>
    /// <param name="key">Clave del elemento</param>
    /// <returns>Valor encontrado en la información cargada</returns>
	public string GetLocalizedValue(string key)
	{
		string result = missingTextString;
		if (localizedText.ContainsKey(key))
		{
			result = localizedText[key];
		}
		return result;
	}

	/// <summary>Verificación de que la carga de datos se realizó correctamente</summary>
    /// <returns>Estado de la carga</returns>
	public bool GetIsReady()
	{
		return isReady;
	}
}
