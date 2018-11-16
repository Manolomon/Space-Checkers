using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviour {
	public static ConnectionManager instance;
	private SocketManager refSocket;
	public Socket socket;
	private string url = "http://localhost:5000/socket.io/";
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
	
	public void CreateSocketRef()
	{
		TimeSpan miliSecForReconnect = TimeSpan.FromMilliseconds (1000); 

        SocketOptions options = new SocketOptions ();
        options.ReconnectionAttempts = 3;
        options.AutoConnect = true;
        options.ReconnectionDelay = miliSecForReconnect;
        //Server URI
    	refSocket = new SocketManager (new Uri (url), options);
		socket = refSocket.GetSocket();
		SetAllEvents();
	}

	private void SetAllEvents()
	{
		socket.On("connect", OnConnect);
		socket.On("test", OnTest);
		socket.On("loginCliente", OnLogin);
		socket.On("createLobby", OnCreateLobby);
		socket.On("setLobbyInfo", OnSetLobbyInfo);
		socket.On("userJoinedRoomCliente", OnUserJoinedRoom);
		socket.On("userSelectedColor", OnUserSelectedColor);
		socket.On("startGameCliente", OnStartGame);
		socket.On("moverPiezaCliente", OnMoverPieza);
		/*
		// Evento que llama el servidor cuando un usuario selecciona un color para actualizar las opciones de los demas jugadores
		socket.On("userSelectedColor", (datos) =>
		{
			string playerData = datos.ToString();
			PlayerLobby playerWithNewColor = JsonConvert.DeserializeObject<PlayerLobby>(playerData);
			foreach (PlayerLobby player in Lobby.instance.Players)
			{
				if (player.Username == playerWithNewColor.Username)
				{
					player.Color = playerWithNewColor.Color;
				}
			}
			//GameObject button = GameObject.Find("Toggle"+playerWithNewColor.Color);
			// togglear a encendido el boton del color encontrado
		});
		// Evento que llama el servidor cuando un usuario deja el lobby
		socket.On("userLeftRoomCliente", (datos) =>
		{
			
		});
		*/
	}

	private void OnConnect(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Conectado al servidor");
	}

	private void OnTest(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Respuesta a test");
	}

	private void OnLogin(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Llamada a loginCliente");
		var datosJugador = JSON.Parse(packet.ToString());
		string jugadorString = datosJugador[1].ToString();
		Jugador jugador = JsonConvert.DeserializeObject<Jugador>(jugadorString);
		Debug.Log(jugador.Username);
		ConnectionManager.instance.socket.Emit("createGame");
	}

	private void OnCreateLobby(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string idlobby = datos[1].ToString();
		Debug.Log(idlobby);
		Lobby.instance.IdLobby = idlobby;
		Lobby.instance.Players.Add(new PlayerLobby(Jugador.instance.Username));
	}

	private void OnSetLobbyInfo(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string lobbyInfo = datos[1].ToString();
		Lobby lobby = JsonConvert.DeserializeObject<Lobby>(lobbyInfo);
		Lobby.instance = lobby;
		Lobby.instance.PrintLobby();
	}

	private void OnUserJoinedRoom(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string newUser = datos[1].ToString();
		PlayerLobby newPlayer = new PlayerLobby(newUser);
		Lobby.instance.Players.Add(newPlayer);
		Lobby lobby = Lobby.instance;
		string dataLobby = JsonConvert.SerializeObject(lobby);
		socket.Emit("getLobbyInfo", dataLobby);
	}

	private void OnUserSelectedColor(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string playerData = datos[1].ToString();
		PlayerLobby playerWithNewColor = JsonConvert.DeserializeObject<PlayerLobby>(playerData);
		foreach (PlayerLobby player in Lobby.instance.Players)
		{
			if (player.Username == playerWithNewColor.Username)
			{
				player.Color = playerWithNewColor.Color;
			}
		}
		Toggle button = GameObject.Find("Toggle"+playerWithNewColor.Color).GetComponent<Toggle>();
		// togglear a encendido el boton del color encontrado
		button.isOn = true;
	}

	private void OnStartGame(Socket socket, Packet packet, params object[] args)
	{
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		foreach (PlayerLobby player in Lobby.instance.Players)
		{
			if (player.Username == Jugador.instance.Username)
			{
				control.MyTurn = Lobby.instance.Players.IndexOf(player) + 1;
				control.Color = player.Color;
			}
		}
		control.ActualTurn = 1;
	}

	private void OnMoverPieza(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string dataMovement = datos[1].ToString();
		DatosMovimiento movimiento = JsonConvert.DeserializeObject<DatosMovimiento>(dataMovement);
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		control.FichaSeleccionada = GameObject.Find(movimiento.Ficha);
		Casilla casilla = control.FichaSeleccionada.GetComponent<Ficha>().casilla.GetComponent<Casilla>();
		casilla.Mover();
	}

	void Start () 
	{
		CreateSocketRef();
	}

}