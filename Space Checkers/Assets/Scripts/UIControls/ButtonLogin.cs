using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class ButtonLogin : MonoBehaviour {

	public void clickLogin()
	{
		ConnectionManager.instance.socket.Emit("login", "{ username: Deklok }");
	}
}
