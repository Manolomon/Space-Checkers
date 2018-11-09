using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using UnityEngine.UI;

public class ButtonLogin : MonoBehaviour {

	public void clickLogin()
	{
		InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
		ConnectionManager.instance.socket.Emit("login", username.text );
		ConnectionManager.instance.socket.Emit("createGame");
	}
}
