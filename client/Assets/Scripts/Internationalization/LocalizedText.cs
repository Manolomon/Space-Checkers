using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que funciona para la asociación con el inspector y la clave del Item
/// </summary>
public class LocalizedText : MonoBehaviour {

	public string key;

    /// <summary>
    /// Inicia la instancia
    /// </summary>
    void Start() {
		Text text = GetComponent<Text>();
		text.text = LocalizationManager.instance.GetLocalizedValue(key);
	}

    /// <summary>
    /// Actualización de instancia
    /// </summary>
    void Update() {
		
	}
}
