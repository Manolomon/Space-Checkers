using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {
	private bool ocupada;
	private List<GameObject> casillasDisponibles = new List<GameObject>();
	private ControlTurnos control;
	public bool Ocupada
	{
		get {return ocupada;}
		set {ocupada = value;}	
	}

	public List<GameObject> CasillasDisponibles
	{
		get {return casillasDisponibles;}
	}

	// Se declaran publicos para asignarles el valor en el inspector
	public GameObject casillaUI;
	public GameObject casillaUD;
	public GameObject casillaD;
	public GameObject casillaDD;
	public GameObject casillaDI;
	public GameObject casillaI;


	private string nombreCasilla(GameObject casilla)
	{
		if (casilla != null)
		{
			return casilla.name.ToString();
		} else {
			return "none";
		}
	}

	// Obtiene las casillas disponibles arriba a la izquierda y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesUI(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaUI != null && casillaActual.casillaUI.GetComponent<Casilla>().casillaUI != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaUI.GetComponent<Casilla>().casillaUI;
			if(casillaActual.casillaUI.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaUI.GetComponent<Casilla>().casillaUI.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}

	// Obtiene las casillas disponibles arriba a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesUD(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaUD != null && casillaActual.casillaUD.GetComponent<Casilla>().casillaUD != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaUD.GetComponent<Casilla>().casillaUD;
			if(casillaActual.casillaUD.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaUD.GetComponent<Casilla>().casillaUD.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesI(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}

	// Obtiene las casillas disponibles a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesD(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaD != null && casillaActual.casillaD.GetComponent<Casilla>().casillaD != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaD.GetComponent<Casilla>().casillaD;
			if(casillaActual.casillaD.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaD.GetComponent<Casilla>().casillaD.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDI(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}

	// Obtiene las casillas disponibles abajo a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesDD(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaDD != null && casillaActual.casillaDD.GetComponent<Casilla>().casillaDD != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaDD.GetComponent<Casilla>().casillaDD;
			if(casillaActual.casillaDD.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaDD.GetComponent<Casilla>().casillaDD.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesI(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}

	// Obtiene las casillas disponibles abajo a la izquierda y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesDI(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaDI != null && casillaActual.casillaDI.GetComponent<Casilla>().casillaDI != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaDI.GetComponent<Casilla>().casillaDI;
			if(casillaActual.casillaDI.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaDI.GetComponent<Casilla>().casillaDI.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesI(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}
	// Obtiene las casillas disponibles a la izquierda y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> casillasDisponiblesI(GameObject casilla)
	{
		List<GameObject> listaCasillas = new List<GameObject>();
		Casilla casillaActual = casilla.GetComponent<Casilla>();
		if (casillaActual.casillaI != null && casillaActual.casillaI.GetComponent<Casilla>().casillaI != null)
		{
			GameObject casillaSiguiente = casillaActual.casillaI.GetComponent<Casilla>().casillaI;
			if(casillaActual.casillaI.GetComponent<Casilla>().Ocupada == true && casillaActual.casillaI.GetComponent<Casilla>().casillaI.GetComponent<Casilla>().Ocupada == false)
			{
				listaCasillas.Add(casillaSiguiente);
				listaCasillas.AddRange(
					casillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas.AddRange(
					casillasDisponiblesI(casillaSiguiente)
				);
			}
		}
		return listaCasillas;
	}
	// Funcion que llena la lista de las casillas disponibles. Utiliza recursividad para llamar a las
	// funciones que llaman a checar las casillas disponibles a todos los lados posibles
	public List<GameObject> obtenerCasillasDisponibles()
	{
		casillasDisponibles.Clear();
		List<GameObject> casillasAdyacentes = new List<GameObject> {
		casillaUI, casillaUD, casillaD, casillaDD, casillaDI, casillaI
		};
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
		casillasDisponibles.AddRange(
			casillasDisponiblesUI(this.gameObject)
		);
		casillasDisponibles.AddRange(
			casillasDisponiblesUD(this.gameObject)
		);
		casillasDisponibles.AddRange(
			casillasDisponiblesD(this.gameObject)
		);
		casillasDisponibles.AddRange(
			casillasDisponiblesDD(this.gameObject)
		);
		casillasDisponibles.AddRange(
			casillasDisponiblesDI(this.gameObject)
		);
		casillasDisponibles.AddRange(
			casillasDisponiblesI(this.gameObject)
		);
		
		return casillasDisponibles;
	}
	// Itera sobre la lista de casillas disponibles para iluminarlas
	public void iluminarCasillasDisponibles() 
	{
		SpriteRenderer sr;
		obtenerCasillasDisponibles();
		foreach (GameObject casilla in casillasDisponibles)
		{
			sr = casilla.GetComponent<SpriteRenderer>();
			sr.color = Color.black;
			Debug.Log(casilla);
		}
	}
	private void Start () 
	{
		// obtener la referencia del objeto de control de turnos
		control = GameObject.Find("ControlTurnos").GetComponent<ControlTurnos>();
	}

	private void OnMouseDown()
	{
		if (control.ActualTurn == control.MyTurn)
		{
			Debug.Log(this.gameObject.name.ToString());
			if (control.SeleccionCasilla == true)
			{
				bool movimientovalido = false;
				foreach (GameObject casilla in control.CasillasValidas)
				{
					if (casilla.Equals(this.gameObject))
					{
						movimientovalido = true;
						break;
					}
				}
				if (movimientovalido)
				{
					control.FichaSeleccionada.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
					Ficha scriptFicha = control.FichaSeleccionada.GetComponent<Ficha>();
					Casilla scriptCasilla = scriptFicha.casilla.GetComponent<Casilla>();
					scriptCasilla.Ocupada = false;
					scriptFicha.casilla = this.gameObject;
					this.Ocupada = true;
					control.SeleccionCasilla = false;
					SpriteRenderer sr;
					foreach (GameObject casilla in control.CasillasValidas)
					{
						sr = casilla.GetComponent<SpriteRenderer>();
						sr.color = Color.white;
					}
					ConnectionManager.instance.socket.Emit("moverPieza", 
					"{ficha : " + control.FichaSeleccionada.gameObject.name +
					 "casilla : " + this.gameObject.name);
					// terminar turno
				}
			}
		}
	}

}
