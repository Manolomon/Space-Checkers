using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lobby : MonoBehaviour {

	private string idLobby;
	private Dictionary <string, string> players = new Dictionary<string, string>();
	public static Lobby instance;
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

	public void StartGame()
	{
		ConnectionManager.instance.socket.Emit("startGame", idLobby);
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
