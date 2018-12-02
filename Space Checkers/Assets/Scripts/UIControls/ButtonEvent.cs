using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ButtonEvent : MonoBehaviour {

	public void ClickLogin()
	{
		InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
		Debug.Log("Username login: " + username.text);
		ConnectionManager.instance.socket.Emit("login", username.text);
	}

	public void ClickJoin()
	{
		InputField username = GameObject.Find("TFGameCode").GetComponent<InputField>();
		ConnectionManager.instance.socket.Emit("joinGame", username.text);
		ConnectionManager.instance.ToJoin = true;
	}

	public void ClickColor()
	{
		
	}

	public void ClickStartGame()
	{
		Lobby.instance.StartGame();
	}

/*/	public void CopyToClipboard()
	{
		Text invitationCode = GameObject.Find("TxtCode").GetComponent<Text>();
		EditorGUIUtility.systemCopyBuffer = invitationCode.text;
		SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue ("message_lobby_code_copied"),
            SSTools.Position.bottom, SSTools.Time.oneSecond);
	}*/
}
