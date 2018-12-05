/// <summary>Arreglo de los items en el archivo de lectura</summary>
[System.Serializable]
public class LocalizationData {
	public LocalizationItem[] items;	
}

/// <summary>Elemento único que da forma a la información leída</summary>
[System.Serializable]
public class LocalizationItem {
	public string key;
	public string value;
}