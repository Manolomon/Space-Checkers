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
	private SocketManager refSocket;
	public Socket socket;
	private bool isOwner = false;
	private string url = "http://localhost:5000/socket.io/";
	public static ConnectionManager instance;
	public bool ToJoin {get; set;}
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
        SocketOptions options = new SocketOptions ();
		options.Timeout = TimeSpan.FromMilliseconds(15000);
        options.ReconnectionAttempts = 3;
		options.Reconnection = true;
        //options.AutoConnect = true;
        options.ReconnectionDelay = TimeSpan.FromMilliseconds(5000);
        //Server URI
    	refSocket = new SocketManager (new Uri (url));
		socket = refSocket.GetSocket();
		SetAllEvents();
	}

	private void SetAllEvents()
	{
		socket.On("connect", OnConnect);
		socket.On("test", OnTest);
		socket.On("loginCliente", OnLogin);
		socket.On("loginSuccessCliente", OnLoginSuccess);
		socket.On("sendInvitation", OnSendInvitation);
		socket.On("sendActivationCode", OnSendActivationCode);
		socket.On("activationSuccess", OnActivationSuccess);
		socket.On("createLobby", OnCreateLobby);
		socket.On("setLobbyInfo", OnSetLobbyInfo);
		socket.On("getLobbyInfo", OnGetLobbyInfo);
		socket.On("userJoinedRoomCliente", OnUserJoinedRoom);
		socket.On("userSelectedColor", OnUserSelectedColor);
		socket.On("startGameCliente", OnStartGame);
		socket.On("moverPiezaCliente", OnMoverPieza);
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
		Debug.Log("Comprobando pass");
		InputField password = GameObject.Find("TFPassword").GetComponent<InputField>();
		string hashPass = HashManager.GeneratePasswordHash(password.text);
		var datosJugador = JSON.Parse(packet.ToString());
		string passFound = datosJugador[1].ToString().Trim( new Char[] {'"'});
		Debug.Log(passFound + " vs " + hashPass);
		if (passFound.Equals(hashPass))
		{
			ConnectionManager.instance.socket.Emit("loginSuccess");
		} 
		else 
		{
			Debug.Log("Contrasena incorrecta");
		}
	}

	private void OnLoginSuccess(Socket socket, Packet packet, params object[] args)
	{
		var datosJugador = JSON.Parse(packet.ToString());
		string jugadorJSON = datosJugador[1].ToString();
		Debug.Log(jugadorJSON);
		Jugador.instance = JsonConvert.DeserializeObject<Jugador>(jugadorJSON);
		Debug.Log(Jugador.instance.Username);
		SceneManager.LoadScene(4);
	}

	public void OnSendInvitation(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Enviando Invitacion");
		var datosInvitado = JSON.Parse(packet.ToString());
		string invitadoString = datosInvitado[2].ToString();
		Jugador.instance.Correo = invitadoString;
		instance.socket.Emit("sendInvitation");
	}

	public void OnSendActivationCode(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string codigoActivacion = datos[1].ToString().Trim( new Char[] {'"'});
		if (codigoActivacion.Equals("")) // como pasar el codigo ingresado a aqui
		{
			ConnectionManager.instance.socket.Emit("");
		} 
		else 
		{
			Debug.Log("Codigo de activacion incorrecto");
		}
	}

	public void OnActivationSuccess(Socket socket, Packet packet, params object[] args)
	{
		// guardar en la BD los datos del jugador
		SceneManager.LoadScene(4);	
	}
	
	private void OnCreateLobby(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string idlobby = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log(idlobby);
		Lobby.instance.IdLobby = idlobby;
		Debug.Log("Dueno: " + Jugador.instance.Username);
		if (!Lobby.instance.Players.ContainsKey(Jugador.instance.Username))
		{
			Lobby.instance.Players.Add(Jugador.instance.Username, null);
		}
		Lobby.instance.PrintLobby();
		isOwner = true;
	}

	private void OnSetLobbyInfo(Socket socket, Packet packet, params object[] args)
	{
		if (ToJoin)
		{
			var datos = JSON.Parse(packet.ToString());
			string lobbyInfo = datos[1].ToString();
			lobbyInfo = lobbyInfo.Substring(1, lobbyInfo.Length - 2);
			lobbyInfo = lobbyInfo.Replace(@"\", "");
			Debug.Log(lobbyInfo);
			DatosLobby lobby = JsonConvert.DeserializeObject<DatosLobby>(lobbyInfo);
			Lobby.instance.IdLobby = lobby.IdLobby;
			Lobby.instance.Players = lobby.Players;
			Lobby.instance.PrintLobby();
			//Lobby.instance = lobby;
			//Lobby.instance.PrintLobby();
		}
	}

	private void OnGetLobbyInfo(Socket socket, Packet packet, params object[] args)
	{
		if (isOwner)
		{
			Debug.Log("obteniendo info de lobby");
			//string idLobby = Lobby.instance.IdLobby;
			//Dictionary<string, string> players = Lobby.instance.Players;
			DatosLobby datosLobby = new DatosLobby(Lobby.instance.IdLobby, Lobby.instance.Players);
			string dataLobby = JsonConvert.SerializeObject(datosLobby);
			socket.Emit("setLobbyInfo", dataLobby);
		}
	}

	private void OnUserJoinedRoom(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string newUser = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Se unio: " + newUser);
		Lobby.instance.Players.Add(newUser, null);
	}

	private void OnUserSelectedColor(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string playerData = datos[1].ToString();
		KeyValuePair<string, string> playerWithNewColor = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(playerData);
		Lobby.instance.Players.Remove(playerWithNewColor.Key);
		Lobby.instance.Players.Add(playerWithNewColor.Key, playerWithNewColor.Value);
		Toggle button = GameObject.Find("Toggle"+playerWithNewColor.Value).GetComponent<Toggle>();
		// togglear a encendido el boton del color encontrado
		button.isOn = true;
	}

	private void OnStartGame(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Game starting!");
		SceneManager.LoadScene(2);
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		int count = 1;
		foreach (KeyValuePair<string, string> kvp in Lobby.instance.Players)
		{
			if (kvp.Key == Jugador.instance.Username)
			{
				control.MyTurn = count;
				control.Color = kvp.Value;
			}
			count++;
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