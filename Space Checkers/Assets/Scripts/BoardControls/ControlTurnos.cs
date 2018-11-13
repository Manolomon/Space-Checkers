using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ControlTurnos : MonoBehaviour {
	// Referencia de la ficha seleccionada a mover
	private GameObject fichaSeleccionada;
	
	// True si esta en la fase de seleccion de casilla. False si necesita seleccionar ficha primero
	private bool seleccionCasilla = false;

	// Valor del turno actual de la partida
	private int actualTurn;

	// Valor del turno correspondiente al usuario
	private int myTurn;

	// Variable para determinar si es el turno del usuario
	private bool turnActive = false;

	// Lista de las casillas validas para el movimiento a realizar con base a la ficha seleccionada
	private List<GameObject> casillasValidas;
	public GameObject FichaSeleccionada
	{
		get {return fichaSeleccionada;}
		set {fichaSeleccionada = value;}	
	}
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

	public bool TurnActive
	{
		get {return turnActive;}
		set {turnActive = value;}	
	}

	// Metodo para enviar un movimiento especifico a los demas de la sala
	public void EnviarMovimiento(string ficha, string casilla)
	{
		DatosMovimiento movimiento = new DatosMovimiento(ficha,casilla);
		ConnectionManager.instance.socket.Emit("moverPieza", JsonConvert.SerializeObject(movimiento));
	}
}
