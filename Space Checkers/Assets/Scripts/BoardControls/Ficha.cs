using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {

	private bool seleccionado;
	public Sprite colorCasilla;
	public GameObject casilla;
	private Casilla sCasilla;
	private ControlTurnos control;

	void Start () 
	{
		sCasilla = casilla.GetComponent<Casilla>();
		sCasilla.Ocupada = true;
		Vector3 posicion = casilla.transform.position;
		this.gameObject.transform.position = new Vector3(posicion.x, posicion.y, posicion.z - 10);
		control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
	}
	
	// Al dar click a la ficha, comprueba la casilla en donde esta e ilumina las casillas disponibles para moverse
	private void OnMouseDown()
	{
		if (control.ActualTurn == control.MyTurn)
		{
			Debug.Log("Click a ficha" + this.gameObject.name.ToString());
			if (control.SeleccionCasilla == false && control.Color == gameObject.tag)
			{
				control.FichaSeleccionada = this.gameObject;
				Casilla scriptCasilla = casilla.GetComponent<Casilla>();
				scriptCasilla.iluminarCasillasDisponibles();
				control.SeleccionCasilla = true;
				control.CasillasValidas = scriptCasilla.CasillasDisponibles;
			} else {
				Debug.Log("Ficha no valida para movimiento");
				Debug.Log("Fase seleccion casilla: " + control.SeleccionCasilla);
				Debug.Log("Color jugador: " +  control.Color + "  /  Color ficha: " + gameObject.tag);
			}
		} else {
			Debug.Log(control.ActualTurn + ": turno actual / turno jugador: " + control.MyTurn);
		}
	}	
}
