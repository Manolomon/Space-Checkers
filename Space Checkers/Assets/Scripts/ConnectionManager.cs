using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class ConnectionManager : MonoBehaviour {
	public static ConnectionManager instance;
	public Socket socket;
	private string url = "http://localhost:5000";
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
	void Start () 
	{
		socket = IO.Socket(url);
		socket.On(Socket.EVENT_CONNECT, () =>
		{
			Debug.Log("Conectado al servidor");
		});
		socket.On("loginCliente", (datos) =>
		{
			Debug.Log("login existoso");
			string jugador = datos.ToString();
			Jugador.instance = JsonConvert.DeserializeObject<Jugador>(jugador);
			Debug.Log(Jugador.instance.Username);			
		});		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
