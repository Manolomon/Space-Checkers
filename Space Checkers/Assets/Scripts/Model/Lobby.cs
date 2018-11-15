using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Clase de referencia para interpretacion de informacion
[System.Serializable]
public class PlayerLobby {
	public PlayerLobby(string username)
	{
		this.Username = username;
	}
	public string Username {get; set;}
	public string Color {get; set;}
}
[System.Serializable]
public class Lobby : MonoBehaviour {

	public static Lobby instance;
	private string idLobby;
	private List<PlayerLobby> players = new List<PlayerLobby>();
	public string IdLobby
	{
		get {return idLobby;}
		set {idLobby = value;}
	}
	public List<PlayerLobby> Players
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

	void startGame()
	{
		ConnectionManager.instance.socket.Emit("startGame", "1A2B");
	}
}
