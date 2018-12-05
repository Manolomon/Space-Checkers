[System.Serializable]
public class ConfigData 
{
	public ConfigItem[] items;	
}

[System.Serializable]
public class ConfigItem
{
	public string key;
	public string value;
}