using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
[System.Serializable] 
public class DatosLobby { 
 
	public string IdLobby {get; set;} 
	private Dictionary <string, string> players = new Dictionary<string, string>(); 
 
    public DatosLobby (string idlobby, Dictionary<string, string> playersDictionary) 
    { 
        IdLobby = idlobby; 
        players = playersDictionary; 
    } 
	public Dictionary<string, string> Players 
	{ 
		get {return players;} 
		set {players = value;} 
	} 
 
	public void PrintLobby() 
	{ 
		Debug.Log("ID Lobby: " + IdLobby); 
		foreach (KeyValuePair<string, string> kvp in players) 
		{ 
			Debug.Log(string.Format("Player: {0} / Color: {1}", kvp.Key, kvp.Value)); 
		} 
	} 
}