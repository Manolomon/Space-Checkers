using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {

	private bool seleccionado;
	private SpriteRenderer ren;
	public Sprite colorCasilla;
	public Color colorC;
	public GameObject casilla;
	private Casilla sCasilla;

	void Start () 
	{
		ren = this.GetComponent<SpriteRenderer>();
		ren.color = colorC;
		ren.sprite = colorCasilla;
		sCasilla = casilla.GetComponent<Casilla>();
		sCasilla.Ocupada = true;
		this.transform.position = casilla.transform.position;
	}
	
	private void OnMouseEnter()
	{
		ren.color = Color.gray;
	}

	private void OnMouseExit()
	{
		ren.color = colorC;
	}
	private void OnMouseDown()
	{
		Debug.Log("Click");
		Debug.Log(casilla.name.ToString());
		sCasilla.iluminarCasillasDisponibles();
		this.seleccionado = true;
	}	

}
