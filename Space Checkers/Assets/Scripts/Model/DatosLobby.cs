using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
[System.Serializable] 
public class DatosLobby { 
 
	private string idLobby; 
	private Dictionary <string, string> players = new Dictionary<string, string>(); 
 
    public DatosLobby (string idlobby, Dictionary<string, string> players) 
    { 
        this.idLobby = idlobby; 
        this.players = players; 
    } 
 
	public string IdLobby 
	{ 
		get {return idLobby;} 
		set {idLobby = value;} 
	} 
	public Dictionary<string, string> Players 
	{ 
		get {return players;} 
		set {players = value;} 
	} 
 
	public void PrintLobby() 
	{ 
		Debug.Log("ID Lobby: " + idLobby); 
		foreach (KeyValuePair<string, string> kvp in players) 
		{ 
			Debug.Log(string.Format("Player: {0} / Color: {1}", kvp.Key, kvp.Value)); 
		} 
	} 
}