using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ControlTurnos : MonoBehaviour {
	// Referencia de la ficha seleccionada a mover
	public GameObject FichaSeleccionada {get; set;}
	
	// True si esta en la fase de seleccion de casilla. False si necesita seleccionar ficha primero
	private bool seleccionCasilla = false;

	// Valor del turno actual de la partida
	private int actualTurn;

	// Valor del turno correspondiente al usuario
	private int myTurn;

	// Lista de las casillas validas para el movimiento a realizar con base a la ficha seleccionada
	private List<GameObject> casillasValidas;

	// Color correspondiente al jugador
	public string Color {get; set;}

	public string ColorMeta {get; set;}
	private List<Ficha> fichasJugador = new List<Ficha>();

	public bool SeleccionCasilla
	{
		get {return seleccionCasilla;}
		set {seleccionCasilla = value;}	
	}
	public List<GameObject> CasillasValidas
	{
		get {return casillasValidas;}
		set {casillasValidas = value;}	
	}
	public int MyTurn
	{
		get {return myTurn;}
		set {myTurn = value;}
	}
	public int ActualTurn
	{
		get {return actualTurn;}
		set {actualTurn = value;}
	}


	public void IniciarControl()
	{
		SetColorMeta();
		ObtenerCasillasJugador();
	}

	public void SetColorMeta()
	{
		if (Color.Equals("Blue"))
		{
			ColorMeta = "Purple";
		} else if (Color.Equals("Red"))
		{
			ColorMeta = "Yellow";
		} else if (Color.Equals("Green"))
		{
			ColorMeta = "Orange";
		} else if (Color.Equals("Orange"))
		{
			ColorMeta = "Green";
		} else if (Color.Equals("Purple"))
		{
			ColorMeta = "Blue";
		} else 
		{
			ColorMeta = "Red";
		}
	}

	private void ObtenerCasillasJugador()
	{
		GameObject[] fichas = GameObject.FindGameObjectsWithTag(Color);
		for (int i = 0; i < fichas.Length; i++)
		{
			if (fichas[i].GetComponent<Ficha>() != null)
			{
				fichasJugador.Add(fichas[i].GetComponent<Ficha>());
			}
		}
	}

	// Metodo para enviar un movimiento especifico a los demas de la sala
	public void EnviarMovimiento(string ficha, string casilla)
	{
		DatosMovimiento movimiento = new DatosMovimiento(Lobby.instance.IdLobby,ficha,casilla);
		ConnectionManager.instance.socket.Emit("moverPieza", JsonConvert.SerializeObject(movimiento));
	}

	private bool JugadorActualWinner()
	{
		bool result = true;
		foreach (Ficha ficha in fichasJugador)
		{
			if (!ficha.casilla.tag.Equals(ColorMeta))
			{
				result = false;
			}
		}
		return result;
	}

	public void TerminarTurno()
	{
		if (JugadorActualWinner())
		{
			//emit winner jugador actual
		} else {
			ConnectionManager.instance.socket.Emit("terminarTurno");
		}
	}
		
}
