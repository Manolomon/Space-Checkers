using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jugador : MonoBehaviour{
	private string username;
	private string correo;
	private string pass;
	private int partidasJugadas;
	private int partidasGanadas;
	private int id;
	public static Jugador instance;
	public string Username
	{
		get {return username;}
		set {username = value;}
	}
	public string Correo
	{
		get {return correo;}
		set {correo = value;}
	}
	public string Pass
	{
		get {return pass;}
		set {pass = value;}
	}
	public int PartidasGanadas
	{
		get {return partidasGanadas;}
		set {partidasGanadas = value;}
	}
	public int PartidasJugadas
	{
		get {return partidasJugadas;}
		set {partidasJugadas = value;}
	}
	public int Id
	{
		get {return id;}
		set {id = value;}
	}
	public static Jugador CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Jugador>(jsonString);
    }

	void Awake () 
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
}
