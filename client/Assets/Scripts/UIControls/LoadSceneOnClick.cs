using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>Clase de gestión para el cambio entre escenas desde el Unity Inspector</summary>
public class LoadSceneOnClick : MonoBehaviour {

	/// <summary>Método que cambia de escena a partir de su índice</summary>
    /// <param name="sceneIndex">Índice de la escena por cargar</param>
	public void LoadByIndex(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
