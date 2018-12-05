using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
public class LobbyHandler : MonoBehaviour, IPointerClickHandler {

	/// <summary>
    /// Metodo utilizado para determinar si enviar o no el color seleccionado en el lobby
    /// </summary>
	public void OnPointerClick(PointerEventData eventData)
    {
		if (gameObject.GetComponent<Toggle>().isOn)
		{
			DatosColor datoscolor = new DatosColor(Lobby.instance.IdLobby, Jugador.instance.Username, gameObject.tag);
            string dataColor = JsonConvert.SerializeObject(datoscolor);
            ConnectionManager.instance.socket.Emit("selectColor",dataColor);
		}
    }
}
