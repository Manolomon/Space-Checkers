using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTurnos : MonoBehaviour {

	private GameObject fichaSeleccionada;
	private bool seleccionCasilla = false;
	private int turno;
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
}
