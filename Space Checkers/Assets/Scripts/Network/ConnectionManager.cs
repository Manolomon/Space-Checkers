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
	public bool IsOwner = false;
	private string url;
	public static ConnectionManager instance;

	public bool ToJoin { get; set; }

	public string Code { get; set; }
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
	
	/// <summary>
    /// Metodo para crear la referencia al servidor
    /// </summary>
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

	/// <summary>
    /// Metodo para registrar todos los eventos ante los que puede responder el cliente
    /// </summary>
	private void SetAllEvents()
	{
		socket.On("connect", OnConnect);
		socket.On("aviso", OnAviso);
		socket.On("loginCliente", OnLogin);
		socket.On("loginSuccessCliente", OnLoginSuccess);
		socket.On("leaderboardWinsCliente", OnLeaderboardWins);
		socket.On("leaderboardGamesCliente", OnLeaderboardGames);
		socket.On("sendActivationCode", OnSendActivationCode);
		socket.On("mensajes", OnMensajes);
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

	/// <summary>
    /// Metodo que se llama para avisos del servidor
    /// </summary>
	private void OnAviso(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string mensaje = datos[1].ToString().Trim( new Char[] {'"'});
		// Mensaje dependiendo lo que traiga el string
		Debug.Log(mensaje);
	}

	/// <summary>
    /// Metodo llamado por servidor que envia el hash de la pass para comprobar que sea correcta
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor cuando el loggeo fue correcto y envia la informacion del usuario
    /// </summary>
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

	/// <summary>
    /// Corutina para actualizar la escena de home (esperando al cambio de escena)
    /// </summary>
	IEnumerator ActualizarHome()
	{
		yield return new WaitForSeconds(1);
		Text username = GameObject.Find("ContentPanel/UserDataPanel/TextUsername").GetComponent<Text>();
		Text correo = GameObject.Find("ContentPanel/UserDataPanel/TextUserMail").GetComponent<Text>();
		Text gamesPlayed = GameObject.Find("ContentPanel/UserDataPanel/UserPerformance/GamesPlayed/TextTotalGamesPlayed").GetComponent<Text>();
		Text gamesWon = GameObject.Find("ContentPanel/UserDataPanel/UserPerformance/GamesWon/TextTotalGamesWon").GetComponent<Text>();
		Text gamesLost = GameObject.Find("ContentPanel/UserDataPanel/UserPerformance/GamesLost/TextTotalGamesLost").GetComponent<Text>();
		Text winrate = GameObject.Find("ContentPanel/UserDataPanel/UserPerformance/Panel/Graph/Bar Chart Component(Clone)/ValueText").GetComponent<Text>();
		username.text = Jugador.instance.Username;
		correo.text = Jugador.instance.Correo;
		gamesPlayed.text = Jugador.instance.PartidasJugadas.ToString();
		gamesWon.text = Jugador.instance.PartidasGanadas.ToString();
		int losses = Jugador.instance.PartidasJugadas - Jugador.instance.PartidasGanadas;
		float winrateP = (float)Jugador.instance.PartidasGanadas / (float)Jugador.instance.PartidasJugadas * 100;
		gamesLost.text = losses.ToString();
		winrate.text = winrateP.ToString();
		GameObject.Find("ContentPanel/UserDataPanel/UserPerformance/Panel/Graph/Bar Chart Component(Clone)").GetComponent<Slider>().value = winrateP;
	}

	/// <summary>
    /// Metodo que llama el servidor para pasar el top de jugadores con mas partidas ganadas
    /// </summary>
	private void OnLeaderboardWins(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string JSONLeaderboard = datos[1].ToString();
	    Debug.Log(JSONLeaderboard);
	    List<Leader> leaderItems = JsonConvert.DeserializeObject<List<Leader>>(JSONLeaderboard);
	    LeaderboardManager board = GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>();
	    board.SetLeaderData(leaderItems);
    }

	/// <summary>
    /// Metodo que llama el servidor para pasar el top de jugadores con mas partidas jugadas
    /// </summary>
	private void OnLeaderboardGames(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string JSONLeaderboard = datos[1].ToString();
		Debug.Log(JSONLeaderboard);
	    List<Leader> leaderItems = JsonConvert.DeserializeObject<List<Leader>>(JSONLeaderboard);
	    LeaderboardManager board = GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>();
        board.SetLeaderData(leaderItems);
	}

	public void OnSendActivationCode(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		Code = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Codigo de activacion " + Code);
	}

	public void OnMensajes(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Usuario envio mensaje");
		var datosMensaje = JSON.Parse(packet.ToString());
		Debug.Log(datosMensaje);
        string jsonMensaje = datosMensaje[1].ToString();
        jsonMensaje = jsonMensaje.Substring(1, jsonMensaje.Length - 2);
        jsonMensaje = jsonMensaje.Replace(@"\", "");
        Mensaje mensaje = JsonConvert.DeserializeObject<Mensaje>(jsonMensaje);
		string color = mensaje.Color;
        string informacionMensaje = mensaje.InformacionMensaje;
        
        ChatManager.instance.ReceiveChatMessage(informacionMensaje);
	}

	/// <summary>
    /// Metodo llamado por servidor para asignar el nombre de invitado a la partida
    /// </summary>
	public void OnGuestUsername(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string username = datos[1].ToString().Trim( new Char[] {'"'});
		Jugador.instance.Username = username;
	}
	
	/// <summary>
    /// Metodo llamado por servidor para pasar el codigo de la partida creada
    /// </summary>
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
		IsOwner = true;
		StartCoroutine(ActualizarLobby());
	}

	/// <summary>
    /// Corutina llamada para actualizar el codigo del lobby en la GUI
    /// </summary>
	IEnumerator ActualizarLobby()
	{
		yield return new WaitForSeconds(1);
		Text codigo = GameObject.Find("TxtCode").GetComponent<Text>();
		codigo.text = Lobby.instance.IdLobby;
	}

	/// <summary>
    /// Metodo llamado por servidor para asignar ña informaciond de lobby a usuario que esta entrando
    /// </summary>
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
			ToJoin = false;
			StartCoroutine(ActualizarLobby());
		}
	}

	/// <summary>
    /// Metodo llamado por servidor para asignar si la prediccion de movimiento esta activada o no
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor para avisar y mandar el nuevo usuario uniendose a lobby
    /// </summary>
	private void OnUserJoinedRoom(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string newUser = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Se unio: " + newUser);
		Lobby.instance.Players.Add(newUser, null);
		if (IsOwner)
		{
			Debug.Log("obteniendo info de lobby");
			DatosLobby datosLobby = new DatosLobby(Lobby.instance.IdLobby, Lobby.instance.Players);
			string dataLobby = JsonConvert.SerializeObject(datosLobby);
			socket.Emit("setLobbyInfo", dataLobby);
		}
		// Mensaje: Usuario se unio a la sala
	}

	/// <summary>
    /// Metodo llamado por servidor para avisar cuando un usuario selecciono un color
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor para marcar el inicio de la partida
    /// </summary>
	private void OnStartGame(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Game starting!");
		SceneManager.LoadScene(2);
		StartCoroutine(ActualizarGameBoard());
	}

	/// <summary>
    /// Metodo llamado por servidor para avisar que un jugador a abandonado la partida o lobby
    /// </summary>
	private void OnLeave(Socket socket, Packet packet, params object[] args)
	{
		var datos = JSON.Parse(packet.ToString());
		string leavingUser = datos[1].ToString().Trim( new Char[] {'"'});
		Debug.Log("Salio de la sala: " + leavingUser);
		Lobby.instance.Players.Remove(leavingUser);
		// mensaje salida
		if (SceneManager.GetSceneByName("Lobby").isLoaded)
		{
			GameObject.Find("Toggle"+Lobby.instance.Players[leavingUser]).GetComponent<Toggle>().isOn = false;
			GameObject.Find(Lobby.instance.Players[leavingUser]+"PlayerName").SetActive(false);
		} else {
			GameObject.Find(Lobby.instance.Players[leavingUser]+"PlayerName").SetActive(false);	
		}
	}

	/// <summary>
    /// Corutina para cargar las fichas y nombres correspondientes a los colores elegidos
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor para mover una pieza a todo el lobby
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor para terminar el turno de un jugador en determinado lobby
    /// </summary>
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

	/// <summary>
    /// Metodo para buscar objetos inactivos en la escena para su activacion
    /// </summary>
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

	/// <summary>
    /// Metodo llamado por servidor para avisar cuando un jugador a ganado la partida
    /// </summary>
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
		Jugador.instance.PartidasJugadas++;
		if (Jugador.instance.Username.Equals(winner.Jugador))
		{
			Jugador.instance.PartidasGanadas++;
		}
	}

	void Start () 
	{
		url = "http://" + ConfigManager.instance.GetConfigValue("address") + "/socket.io/";
		Debug.Log("URL: " + url);
		CreateSocketRef();
	}

}