using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosColor {

	public string IdLobby {get; set;}
	public string Jugador {get; set;}
    public string Color {get; set;}

	public DatosColor(string lobby, string jugador, string color)
	{
		IdLobby = lobby;
        Jugador = jugador;
        Color = color;
	}

}