using UnityEngine;
using System.Collections;

/// <summary>Clase para evento de salida ya sea del editor o del juego en ejecución</summary>
public class QuitOnClick : MonoBehaviour {

	 /// <summary>Método para cerrar la ejecución del juego</summary>
	public void Quit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

}