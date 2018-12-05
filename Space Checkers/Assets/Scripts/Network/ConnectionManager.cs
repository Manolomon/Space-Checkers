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
using System.Linq;


public class ConnectionManager : MonoBehaviour {
	private SocketManager refSocket;
	public Socket socket;
	private bool isOwner = false;
	private string url;
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
		socket.On("aviso", OnAviso);
		socket.On("loginCliente", OnLogin);
		socket.On("loginSuccessCliente", OnLoginSuccess);
		socket.On("leaderboardWinsCliente", OnLeaderboardWins);
		socket.On("leaderboardGamesCliente", OnLeaderboardGames);
		socket.On("sendInvitation", OnSendInvitation);
		socket.On("sendActivationCode", OnSendActivationCode);
		socket.On("guestUsername", OnGuestUsername);
		socket.On("createLobby", OnCreateLobby);
		socket.On("setLobbyInfo", OnSetLobbyInfo);
		socket.On("userJoinedRoomCliente", OnUserJoinedRoom);
		socket.On("predictionCliente", OnPrediction);
		socket.On("userSelectedColor", OnUserSelectedColor);
		socket.On("startGameCliente", OnStartGame);
		socket.On("leaveGameCliente", OnLeave);
		socket.On("moverPiezaCliente", OnMoverPieza);
		socket.On("terminarTurnoCliente", OnTerminarTurno);
		socket.On("winnerCliente", OnWinner);
	}

	private void OnConnect(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Conectado al servidor");
	}

	private void OnAviso(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string mensaje = datos[1].ToString().Trim( new Char[] {'"'});
		// Mensaje dependiendo lo que traiga el string
		Debug.Log(mensaje);
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
		socket.Emit("leaderboardWins");
		StartCoroutine(ActualizarHome());
	}
	IEnumerator ActualizarHome()
	{
		yield return new WaitForSeconds(1);
		Text username = GameObject.Find("ContentPanel/UserDataPanel/TextUsername").GetComponent<Text>();
		Text correo = GameObject.Find("ContentPanel/UserDataPanel/TextUserMail").GetComponent<Text>();
		username.text = Jugador.instance.Username;
		correo.text = Jugador.instance.Correo;
	}

	private void OnLeaderboardWins(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string JSONLeaderboard = datos[1].ToString();
	    Debug.Log(JSONLeaderboard);
	    List<Leader> leaderItems = JsonConvert.DeserializeObject<List<Leader>>(JSONLeaderboard);
	    LeaderboardManager board = GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>();
	    board.SetLeaderData(leaderItems);
    }

	private void OnLeaderboardGames(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string JSONLeaderboard = datos[1].ToString();
		Debug.Log(JSONLeaderboard);
	    List<Leader> leaderItems = JsonConvert.DeserializeObject<List<Leader>>(JSONLeaderboard);
	    LeaderboardManager board = GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>();
        board.SetLeaderData(leaderItems);
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

	public void OnGuestUsername(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string username = datos[1].ToString().Trim( new Char[] {'"'});
		Jugador.instance.Username = username;
	}

	public void OnActivationSuccess(Socket socket, Packet packet, params object[] args)
	{
		// guardar en la BD los datos del jugador
		SceneManager.LoadScene(4);	
	}
	
	private void OnCreateLobby(Socket socket, Packet packet, params object[] args)
	{
		SceneManager.LoadScene(3);
		var datos = JSON.Parse(packet.ToString());
		string idlobby = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log(idlobby);
		Lobby.instance.IdLobby = idlobby;
		Debug.Log("Dueno: " + Jugador.instance.Username);
		Lobby.instance.Players.Add(Jugador.instance.Username, null);
		Lobby.instance.PrintLobby();
		isOwner = true;
		StartCoroutine(ActualizarLobby());
	}

	IEnumerator ActualizarLobby()
	{
		yield return new WaitForSeconds(1);
		Text codigo = GameObject.Find("TxtCode").GetComponent<Text>();
		codigo.text = Lobby.instance.IdLobby;
	}

	private void OnSetLobbyInfo(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Llamada a setlobbyinfo");
		if (ToJoin)
		{
			SceneManager.LoadScene(3);
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
			ToJoin = false;
			StartCoroutine(ActualizarLobby());
		}
	}

	private void OnPrediction(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string prediction = datos[1].ToString().Trim( new Char[] {'"'});
		if (prediction.Equals("True"))
		{
			GameObject.Find("TogglePrediction").GetComponent<Toggle>().isOn = true;
			Lobby.instance.Prediction = true;
		} else {
			GameObject.Find("TogglePrediction").GetComponent<Toggle>().isOn = false;
			Lobby.instance.Prediction = false;
		}
	}

	private void OnUserJoinedRoom(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string newUser = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Se unio: " + newUser);
		Lobby.instance.Players.Add(newUser, null);
		if (isOwner)
		{
			Debug.Log("obteniendo info de lobby");
			DatosLobby datosLobby = new DatosLobby(Lobby.instance.IdLobby, Lobby.instance.Players);
			string dataLobby = JsonConvert.SerializeObject(datosLobby);
			socket.Emit("setLobbyInfo", dataLobby);
		}
		// Mensaje: Usuario se unio a la sala
	}

	private void OnUserSelectedColor(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Usuario selecciono color");
		var datos = JSON.Parse(packet.ToString());
		string colorInfo = datos[1].ToString();
		colorInfo = colorInfo.Substring(1, colorInfo.Length - 2);
		colorInfo = colorInfo.Replace(@"\", "");
		Debug.Log(colorInfo);
		DatosColor color = JsonConvert.DeserializeObject<DatosColor>(colorInfo);
		Debug.Log("Color elegido " + color.Color + " por " + color.Jugador);
		if (Lobby.instance.Players[color.Jugador] != null)
		{
			Debug.Log("Cambiando color de jugador");
			GameObject.Find("Toggle"+Lobby.instance.Players[color.Jugador]).GetComponent<Toggle>().isOn = false;
			GameObject.Find(Lobby.instance.Players[color.Jugador]+"PlayerName").SetActive(false);
		}
		Lobby.instance.Players[color.Jugador] = color.Color;
		Toggle button = GameObject.Find("Toggle"+color.Color).GetComponent<Toggle>();
		GameObject user = BuscarObjetoInactivo(color.Color+"PlayerName");
		user.SetActive(true);
		user.GetComponent<Text>().text = color.Jugador;
		// togglear a encendido el boton del color encontrado
		button.isOn = true;
		Lobby.instance.PrintLobby();
	}

	private void OnStartGame(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Game starting!");
		SceneManager.LoadScene(2);
		StartCoroutine(ActualizarGameBoard());
	}

	private void OnLeave(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string leavingUser = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Salio de la sala: " + leavingUser);
		Lobby.instance.Players.Remove(leavingUser);
		if (SceneManager.GetSceneByName("Lobby").isLoaded)
		{
			GameObject.Find("Toggle"+Lobby.instance.Players[leavingUser]).GetComponent<Toggle>().isOn = false;
			GameObject.Find(Lobby.instance.Players[leavingUser]+"PlayerName").SetActive(false);
		} else {
			GameObject.Find(Lobby.instance.Players[leavingUser]+"PlayerName").SetActive(false);	
		}
	}

	IEnumerator ActualizarGameBoard()
	{
		yield return new WaitForSeconds(1);
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		int count = 1;
		var list = Lobby.instance.Players.Keys.ToList();
		list.Sort();
		GameObject[] objetos = Resources.FindObjectsOfTypeAll<GameObject>();
		foreach (string player in list)
		{
			foreach (GameObject go in objetos)
			{
				if (go.name == Lobby.instance.Players[player]+"Triangle")
				{
					go.SetActive(true);
				}
				if (go.name == Lobby.instance.Players[player]+"PlayerName")
				{
					go.SetActive(true);
					go.GetComponent<Text>().text = player;
				}
			}
			if (player == Jugador.instance.Username)
			{
				control.MyTurn = count;
				control.Color = Lobby.instance.Players[player];
			}
			count++;
		}
		
		control.ActualTurn = 1;
		control.IniciarControl();
	}

	private void OnMoverPieza(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string movementInfo = datos[1].ToString();
		movementInfo = movementInfo.Substring(1, movementInfo.Length - 2);
		movementInfo = movementInfo.Replace(@"\", "");
		DatosMovimiento movimiento = JsonConvert.DeserializeObject<DatosMovimiento>(movementInfo);
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		control.FichaSeleccionada = GameObject.Find(movimiento.Ficha);
		Debug.Log("Ficha a mover " + control.FichaSeleccionada);
		Casilla casilla = GameObject.Find(movimiento.Casilla).GetComponent<Casilla>();
		casilla.Mover();
		Debug.Log("Casilla destino " + casilla.name);
	}

	private void OnTerminarTurno(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Terminando Turno");
		ControlTurnos control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
		if (control.ActualTurn + 1 > Lobby.instance.Players.Count)
		{
			control.ActualTurn = 1;
		} else {
			control.ActualTurn++;
		}
	}

	private GameObject BuscarObjetoInactivo(string nombre)
	{
		GameObject resultado = null;
		GameObject[] objetos = Resources.FindObjectsOfTypeAll<GameObject>();
		foreach (GameObject go in objetos)
		{
			if (go.name == nombre)
			{
				resultado = go;
			}
		}
		return resultado;
	}

	private void OnWinner(Socket socket, Packet packet, params object[] args)	
	{
		var datos = JSON.Parse(packet.ToString());
		string winnerInfo = datos[1].ToString();
		winnerInfo = winnerInfo.Substring(1, winnerInfo.Length - 2);
		winnerInfo = winnerInfo.Replace(@"\", "");
		Debug.Log(winnerInfo);
		DatosColor winner = JsonConvert.DeserializeObject<DatosColor>(winnerInfo);
		BuscarObjetoInactivo("Blurred Sheet").SetActive(true);
		BuscarObjetoInactivo("Game Over Panel").SetActive(true);
		BuscarObjetoInactivo("Winner Name Text").GetComponent<Text>().text = winner.Jugador;
	}

	void Start () 
	{
		url = "http://" + ConfigManager.instance.GetConfigValue("address") + "/socket.io/";
		Debug.Log("URL: " + url);
		CreateSocketRef();
	}

}