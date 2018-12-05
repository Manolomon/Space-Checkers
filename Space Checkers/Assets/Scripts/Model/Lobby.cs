using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lobby : MonoBehaviour {

	public string IdLobby {get; set;}
	private Dictionary <string, string> players = new Dictionary<string, string>();
	public static Lobby instance;
	public bool Prediction {get; set;}
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
		Prediction = true;
	}

	public void StartGame()
	{
		ConnectionManager.instance.socket.Emit("startGame", IdLobby);
	}

	public void PrintLobby()
	{
		Debug.Log("ID Lobby: " + IdLobby);
		foreach (KeyValuePair<string, string> kvp in players)
		{
			Debug.Log(string.Format("Player: {0} / Color: {1}", kvp.Key, kvp.Value));
		}
	}

	public void LimpiarLobby()
	{
		instance.IdLobby = null;
		instance.Players.Clear();
		instance.Prediction = true;
	}
}
