using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

public class ButtonLogin : MonoBehaviour {

	public void clickLogin()
	{
		var socket = IO.Socket("http://localhost:5000");
		socket.On(Socket.EVENT_CONNECT, () =>
		{
			socket.Emit("login");
		});
		socket.On("loginCliente", (datos) =>
		{
			Debug.Log(datos);
		}
		);
	}
}
