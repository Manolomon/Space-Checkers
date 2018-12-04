using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour {

    public List<LeaderboardItem> leaderboard = new List<LeaderboardItem>();
    // Use this for initialization
    void Start()
    {
        foreach (LeaderboardItem userRow in leaderboard)
        {
            userRow.username.text = "Manolo";
            userRow.barChart.value = 75;
            userRow.percentageText.text = "75%";
            userRow.gamesText.text = "44";
        }
    }

    // Update is called once per frame
	void Update () {
		
	}
}
