using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosMovimiento {
	public string Lobby {get; set;}
	public string Ficha {get; set;}
	public string Casilla {get; set;}

	public DatosMovimiento(string lobby,string ficha, string casilla)
	{
		Lobby = lobby;
		Ficha = ficha;
		Casilla = casilla;
	}

}
