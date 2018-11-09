using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


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
		// Evento que llama el servidor al terminar de recuperar el usuario
		socket.On("loginCliente", (datos) =>
		{
			Debug.Log("login existoso");
			string datosJugador = datos.ToString();
			Debug.Log(datosJugador);
			Jugador.instance = JsonConvert.DeserializeObject<Jugador>(datosJugador);
			Debug.Log(Jugador.instance.Username);
		});
		// Evento que llama el servidor luego de unirlo al room
		socket.On("createLobby", (datos) =>
		{
			string idlobby = datos.ToString();
			Lobby.instance.IdLobby = idlobby;
			Lobby.instance.Players.Add(new PlayerLobby(Jugador.instance.Username));
		});
		// Evento que llama el servidor al recibir la informacion del lobby al cual intenta entrar el cliente
		socket.On("setLobbyInfo", (datos) =>
		{
			string lobbyInfo = datos.ToString();
			Lobby lobby = JsonConvert.DeserializeObject<Lobby>(lobbyInfo);
			Lobby.instance = lobby;
		});
		// Evento que llama el servidor para obtener los datos del lobby para proporcionarselo a un nuevo jugador
		socket.On("userJoinedRoomCliente", (datos) =>
		{
			string newUser = datos.ToString();
			PlayerLobby newPlayer = new PlayerLobby(newUser);
			Lobby.instance.Players.Add(newPlayer);
			Lobby lobby = Lobby.instance;
			string dataLobby = JsonConvert.SerializeObject(lobby);
		});
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
			GameObject button = GameObject.Find("Toggle"+playerWithNewColor.Color);
			// togglear a encendido el boton del color encontrado
		});
		// Evento que llama el servidor cuando un usuario deja el lobby
		socket.On("userLeftRoomCliente", (datos) =>
		{
			
		});
		// Evento que llama el servidor cuando el owner inicia la partida
		socket.On("startGameCliente", () =>
		{
			foreach (PlayerLobby player in Lobby.instance.Players)
			{
				if (player.Username == Jugador.instance.Username)
				{
					ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
					control.MyTurn = Lobby.instance.Players.IndexOf(player) + 1;
					control.ActualTurn = 1;
				}
			}
		});
		// Evento que llama el servidor cuando el jugador en turno mueve su pieza y termina el turno
		// TO DO: determinar la clase para interpretar la informacion de movimiento
		socket.On("moverPiezaCliente", (datos) =>
		{
			
		});
	}

}
