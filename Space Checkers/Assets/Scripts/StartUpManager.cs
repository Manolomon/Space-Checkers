using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Clase que valida que la carga de datos se realiza correctamente</summary>
public class StartUpManager : MonoBehaviour {

	 /// <summary>Método inicial del StartUpManager, impide que se realize el cambio de escena hasta que todos los elementos hayan cargado</summary>
	private IEnumerator Start()
	{
		while (!LocalizationManager.instance.GetIsReady() || !ConfigManager.instance.GetIsReady())
		{
			yield return null;
		}
		SceneManager.LoadScene(1);
	}
}
