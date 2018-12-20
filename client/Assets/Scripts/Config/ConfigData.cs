/// <summary>Arreglo de los items en el archivo de lectura</summary>
[System.Serializable]
public class ConfigData {
	public ConfigItem[] items;	
}

/// <summary>Elemento único que da forma a la información leída</summary>
[System.Serializable]
public class ConfigItem {
	public string key;
	public string value;
}