using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Gestor del elemento gráfico del switch en el leaderboard</summary>
public class ToggleSwitch : MonoBehaviour {
    Toggle toggleView;

	public Text firstLabel;
	public Color activeColor;
	public GameObject secondState;
	public Text secondLabel;
	public Color disactiveColor;


    /// <summary>Inicialización del switch y su estado</summary>
    void Start()
	{
		toggleView = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleView.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggleView);
            });
		if (!toggleView.isOn) 
		{
			firstLabel.color = disactiveColor;
			secondLabel.color = activeColor;
			secondState.SetActive(true);
		}
        Debug.Log(toggleView.isOn);
	}

    /// <summary>Evento de cambio de estado del switch</summary>
    /// <param name="change">Instancia del ToggleSwitch en cuestión</param>
    void ToggleValueChanged(Toggle change)
    {
        Debug.Log(toggleView.isOn);
		if (toggleView.isOn)
		{
			firstLabel.color = activeColor;
			secondLabel.color = disactiveColor;
			secondState.SetActive(false);
		    ConnectionManager.instance.socket.Emit("leaderboardWins");
        }
		else
		{
			firstLabel.color = disactiveColor;
			secondLabel.color = activeColor;
			secondState.SetActive(true);
		    ConnectionManager.instance.socket.Emit("leaderboardGames");
        }
    }
}

