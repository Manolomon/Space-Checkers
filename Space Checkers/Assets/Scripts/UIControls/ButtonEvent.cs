using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ButtonEvent : MonoBehaviour {

	public void ClickLogin()
	{
		InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
		InputField password = GameObject.Find("TFPassword").GetComponent<InputField>();
		string hashPass = HashManager.GeneratePasswordHash(password.text);
		Debug.Log("Username login: " + username.text);
		Debug.Log("Password login" + hashPass);
		ConnectionManager.instance.socket.Emit("login", username.text);
	}

	public void ClickJoin()
	{
		InputField username = GameObject.Find("TFGameCode").GetComponent<InputField>();
		ConnectionManager.instance.socket.Emit("joinGame", username.text);
	}

	public void CopyToClipboard()
	{
		Text invitationCode = GameObject.Find("TxtCode").GetComponent<Text>();
		EditorGUIUtility.systemCopyBuffer = invitationCode.text;
		SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue ("message_lobby_code_copied"),
            SSTools.Position.bottom, SSTools.Time.oneSecond);
	}
}
