using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ButtonEvent : MonoBehaviour
{

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

    public void ClickSendConfirmation()
    {
        Dictionary<string, string> newUserInfo = new Dictionary<string, string>();
        
        InputField email = GameObject.Find("TFEmail").GetComponent<InputField>();
        InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
        InputField password = GameObject.Find("TFPassword").GetComponent<InputField>();
        InputField confirmationPass = GameObject.Find("TFConfirmation").GetComponent<InputField>();
        
        Debug.Log("Comparacion de contrasenas");
        string hashPass = HashManager.GeneratePasswordHash(password.text);
        string hashPassConfirmation = HashManager.GeneratePasswordHash(confirmationPass.text);
        
        if (hashPassConfirmation.Equals(hashPass))
        {
            newUserInfo.Add("email",email.text);
            newUserInfo.Add("username",username.text);
            newUserInfo.Add("password", hashPass);
        } else {
            Debug.Log("La contraseña y la confirmacion de la contraseña son diferentes");
        }

        Debug.Log("Enviando codigo de activacion a " + email.text);
        ConnectionManager.instance.socket.Emit("activation", newUserInfo);
    }

    public void ClickResend()
    {
        // Si necesito el email de la pantalla anterior deberia guardar el email en una variable fuera del metodo?
        InputField email = GameObject.Find("TFEmail").GetComponent<InputField>();
        Debug.Log("Activation code to: " + email.text);
        ConnectionManager.instance.socket.Emit("activation", email.text);
    }

    public void ClickInvite()
    {
        InputField email = GameObject.Find("TFEmail").GetComponent<InputField>();
        Debug.Log("Invitation to: " + email.text);
        ConnectionManager.instance.socket.Emit("invitation", email.text);
    }

    public void ClickJoinAsGuest()
    {

    }
    /*/	public void CopyToClipboard()
        {
            Text invitationCode = GameObject.Find("TxtCode").GetComponent<Text>();
            EditorGUIUtility.systemCopyBuffer = invitationCode.text;
            SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue ("message_lobby_code_copied"),
                SSTools.Position.bottom, SSTools.Time.oneSecond);
        }*/
}