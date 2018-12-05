using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Gestor del Leaderboard y sus elementos</summary>
public class LeaderboardManager : MonoBehaviour {

    public List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();
    public Toggle toggleBoard;

    /// <summary>Inicialización de los elementos del Leaderboard</summary>
    void Start()
    {
        ConnectionManager.instance.socket.Emit("leaderboardWins");    
    }

    /// <summary>
    /// Se resetea la información de la tabla
    /// </summary>
    /// <param name="leaderData">La información para llenar</param>
    public void SetLeaderData(List<Leader> leaderData)
    {
        for (int i = 0; i < leaderData.Count; i++)
        {
            leaderboard[i].username.text = leaderData[i].username;
            float rate = (float)leaderData[i].partidasGanadas / (float)leaderData[i].partidasJugadas * 100;
            Debug.Log(rate);
            leaderboard[i].barChart.value = rate;
            leaderboard[i].percentageText.text = rate.ToString();
            if (toggleBoard.isOn)
            {
                leaderboard[i].gamesText.text = leaderData[i].partidasGanadas.ToString();
            }
            else
            {
                leaderboard[i].gamesText.text = leaderData[i].partidasJugadas.ToString();
            }
        }
    }

    /// <summary>Actualización por cada frame del GameObject</summary>
    void Update ()
    {
		
	}
}
