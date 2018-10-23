using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {
	private bool ocupada;
	public bool Ocupada
	{
		get {return ocupada;}
		set {ocupada = value;}	
	}
	private SpriteRenderer rend;
	public GameObject casillaUI;
	public GameObject casillaUD;
	public GameObject casillaD;
	public GameObject casillaDD;
	public GameObject casillaDI;
	public GameObject casillaI;
	private List<GameObject> casillasDisponibles = new List<GameObject>();


	private string nombreCasilla(GameObject casilla)
	{
		if (casilla != null)
		{
			return casilla.name.ToString();
		} else {
			return "none";
		}
	}

	public List<GameObject> obtenerCasillasDisponibles(GameObject casilla, GameObject casillaSiguiente)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla tempCasilla = casilla.GetComponent<Casilla>();
		Casilla tempCasillaSiguiente = casillaSiguiente.GetComponent<Casilla>();
		if (tempCasilla.Ocupada == true && tempCasillaSiguiente.Ocupada == false)
		{
			listaCasillas.Add(casillaSiguiente);
			if (tempCasillaSiguiente.casillaUI != null && tempCasillaSiguiente.casillaUI.GetComponent<Casilla>().casillaUI != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaUI, tempCasillaSiguiente.casillaUI.GetComponent<Casilla>().casillaUI
					)
				);
			}
			if (tempCasillaSiguiente.casillaUD != null && tempCasillaSiguiente.casillaUD.GetComponent<Casilla>().casillaUD != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaUD, tempCasillaSiguiente.casillaUD.GetComponent<Casilla>().casillaUD
					)
				);
			}
			if (tempCasillaSiguiente.casillaD != null && tempCasillaSiguiente.casillaD.GetComponent<Casilla>().casillaD != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaD, tempCasillaSiguiente.casillaD.GetComponent<Casilla>().casillaD
					)
				);
			}
			if (tempCasillaSiguiente.casillaDD != null && tempCasillaSiguiente.casillaDD.GetComponent<Casilla>().casillaDD != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaDD, tempCasillaSiguiente.casillaDD.GetComponent<Casilla>().casillaDD
					)
				);
			}
			if (tempCasillaSiguiente.casillaDI != null && tempCasillaSiguiente.casillaDI.GetComponent<Casilla>().casillaDI != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaDI, tempCasillaSiguiente.casillaDI.GetComponent<Casilla>().casillaDI
					)
				);
			}
			if (tempCasillaSiguiente.casillaI != null && tempCasillaSiguiente.casillaI.GetComponent<Casilla>().casillaI != null)
			{
				listaCasillas.AddRange(
					obtenerCasillasDisponibles(
						tempCasillaSiguiente.casillaI, tempCasillaSiguiente.casillaI.GetComponent<Casilla>().casillaI
					)
				);
			}
		}
		return listaCasillas;
	}

	public void iluminarCasillasDisponibles() 
	{
		List<GameObject> casillasAdyacentes = new List<GameObject> {
		casillaUI, casillaUD, casillaD, casillaDD, casillaDI, casillaI
		};
		SpriteRenderer sr;
		foreach (GameObject casilla in casillasAdyacentes)
		{
			if (casilla != null)
			{
				Casilla temp = casilla.GetComponent<Casilla>();
				if (temp.Ocupada == false)
				{
					casillasDisponibles.Add(casilla);
				}
			}
		}
		Casilla tempCasilla = this.GetComponent<Casilla>();
		if (tempCasilla.casillaUI != null && tempCasilla.casillaUI.GetComponent<Casilla>().casillaUI != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaUI, tempCasilla.casillaUI.GetComponent<Casilla>().casillaUI
				)
			);
		}
		if (tempCasilla.casillaUD != null && tempCasilla.casillaUD.GetComponent<Casilla>().casillaUD != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaUD, tempCasilla.casillaUD.GetComponent<Casilla>().casillaUD
				)
			);
		}
		if (tempCasilla.casillaD != null && tempCasilla.casillaD.GetComponent<Casilla>().casillaD != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaD, tempCasilla.casillaD.GetComponent<Casilla>().casillaD
				)
			);
		}
		if (tempCasilla.casillaDD != null && tempCasilla.casillaDD.GetComponent<Casilla>().casillaDD != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaDD, tempCasilla.casillaDD.GetComponent<Casilla>().casillaDD
				)
			);
		}
		if (tempCasilla.casillaDI != null && tempCasilla.casillaDI.GetComponent<Casilla>().casillaDI != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaDI, tempCasilla.casillaDI.GetComponent<Casilla>().casillaDI
				)
			);
		}
		if (tempCasilla.casillaI != null && tempCasilla.casillaI.GetComponent<Casilla>().casillaI != null)
		{
			casillasDisponibles.AddRange(
				obtenerCasillasDisponibles(
					tempCasilla.casillaI, tempCasilla.casillaI.GetComponent<Casilla>().casillaI
				)
			);
		}
		foreach (GameObject casilla in casillasDisponibles)
		{
			sr = casilla.GetComponent<SpriteRenderer>();
			sr.color = Color.green;
		}
	}
	private void Start () 
	{
		rend = GetComponent<SpriteRenderer>();
	}

}
