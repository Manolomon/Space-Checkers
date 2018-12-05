using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	public List<GameObject> CasillasDisponiblesUI(GameObject casilla)
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
					CasillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
			}
		}
		listaCasillas = listaCasillas.Distinct().ToList();
		return listaCasillas;
	}

	// Obtiene las casillas disponibles arriba a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> CasillasDisponiblesUD(GameObject casilla)
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
					CasillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
			}
		}
		listaCasillas = listaCasillas.Distinct().ToList();
		return listaCasillas;
	}

	// Obtiene las casillas disponibles a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> CasillasDisponiblesD(GameObject casilla)
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
					CasillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
			}
		}
		listaCasillas = listaCasillas.Distinct().ToList();
		return listaCasillas;
	}

	// Obtiene las casillas disponibles abajo a la derecha y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> CasillasDisponiblesDD(GameObject casilla)
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
					CasillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
			}
		}
		listaCasillas = listaCasillas.Distinct().ToList();
		return listaCasillas;
	}

	// Obtiene las casillas disponibles abajo a la izquierda y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> CasillasDisponiblesDI(GameObject casilla)
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
					CasillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
			}
		}
		listaCasillas = listaCasillas.Distinct().ToList();
		return listaCasillas;
	}
	// Obtiene las casillas disponibles a la izquierda y vuelve a llamar para confirmar si hay mas disponibles
	public List<GameObject> CasillasDisponiblesI(GameObject casilla)
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
					CasillasDisponiblesUI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesUD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDD(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesDI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
				listaCasillas.AddRange(
					CasillasDisponiblesI(casillaSiguiente)
				);
				listaCasillas = listaCasillas.Distinct().ToList();
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
			CasillasDisponiblesUI(gameObject)
		);
		casillasDisponibles.AddRange(
			CasillasDisponiblesUD(gameObject)
		);
		casillasDisponibles.AddRange(
			CasillasDisponiblesD(gameObject)
		);
		casillasDisponibles.AddRange(
			CasillasDisponiblesDD(gameObject)
		);
		casillasDisponibles.AddRange(
			CasillasDisponiblesDI(gameObject)
		);
		casillasDisponibles.AddRange(
			CasillasDisponiblesI(gameObject)
		);
		casillasDisponibles = casillasDisponibles.Distinct().ToList();
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
		MoverLocal();
	}

	public void MoverLocal()
	{
		if (control.ActualTurn == control.MyTurn)
		{
			Debug.Log(gameObject.name.ToString());
			if (control.SeleccionCasilla == true)
			{
				bool movimientovalido = false;
				foreach (GameObject casilla in control.CasillasValidas)
				{
					if (casilla.Equals(gameObject))
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
					scriptFicha.casilla = gameObject;
					Ocupada = true;
					control.SeleccionCasilla = false;
					SpriteRenderer sr;
					foreach (GameObject casilla in control.CasillasValidas)
					{
						sr = casilla.GetComponent<SpriteRenderer>();
						sr.color = Color.white;
					}
					control.EnviarMovimiento(control.FichaSeleccionada.name, this.gameObject.name);
					control.TerminarTurno();
				}
			}
		}
	}

	public void Mover()
	{
		control.FichaSeleccionada.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
		Ficha scriptFicha = control.FichaSeleccionada.GetComponent<Ficha>();
		Casilla scriptCasilla = scriptFicha.casilla.GetComponent<Casilla>();
		scriptCasilla.Ocupada = false;
		scriptFicha.casilla = this.gameObject;
		this.Ocupada = true;
	}

}
