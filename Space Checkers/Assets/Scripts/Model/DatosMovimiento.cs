using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosMovimiento {

	private string ficha;
	private string casilla;

	public DatosMovimiento(string ficha, string casilla)
	{
		this.ficha = ficha;
		this.casilla = casilla;
	}
	public string Ficha
	{
		get {return ficha;}
		set {ficha = value;}
	}
	public string Casilla
	{
		get {return casilla;}
		set {casilla = value;}
	}
}
