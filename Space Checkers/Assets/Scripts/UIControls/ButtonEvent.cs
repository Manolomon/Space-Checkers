using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {

	public void clickLogin()
	{
		InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
		InputField password = GameObject.Find("TFPassword").GetComponent<InputField>();
		string hashPass = HashManager.GeneratePasswordHash(password.text);
		Debug.Log("Username login: " + username.text);
		Debug.Log("Password login" + hashPass);
		ConnectionManager.instance.socket.Emit("login", username.text);
	}

	public void clickJoin()
	{
		InputField username = GameObject.Find("TFGameCode").GetComponent<InputField>();
		ConnectionManager.instance.socket.Emit("joinGame", username.text);
	}
}
