using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json;

public class ButtonEvent : MonoBehaviour
{
    /// <summary>
    /// Metodo que se ejecuta al presionar el boton login.
    /// </summary>
    public void ClickLogin()
    {
        InputField username = GameObject.Find("TFUsername").GetComponent<InputField>();
        Debug.Log("Username login: " + username.text);
        ConnectionManager.instance.socket.Emit("login", username.text);
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar Join para unirse a una partida.
    /// </summary>
    public void ClickJoin()
    {
        InputField code = GameObject.Find("TFGameCode").GetComponent<InputField>(); 
		ConnectionManager.instance.socket.Emit("joinGame", code.text); 
        ConnectionManager.instance.ToJoin = true;
    }

    public void ClickColor()
    {
        Toggle boton = this.GetComponent<Toggle>();
        if (boton.isOn)
        {
            DatosColor datoscolor = new DatosColor(Lobby.instance.IdLobby, Jugador.instance.Username, this.gameObject.tag);
            string dataColor = JsonConvert.SerializeObject(datoscolor);
            ConnectionManager.instance.socket.Emit("selectColor",dataColor);
        }
    }

    public void ClickJoinAsGuest()
    {
        InputField code = GameObject.Find("TFGameCode").GetComponent<InputField>(); 
		ConnectionManager.instance.socket.Emit("joinGame", code.text); 
		ConnectionManager.instance.ToJoin = true;
    }

    public void ClicklCreateGame() 
	{ 
		ConnectionManager.instance.socket.Emit("createGame"); 
	} 

    public void ClickStartGame()
    {
        Lobby.instance.StartGame();
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar enviar codigo (envia el codigo de activacion para registrarse)
    /// </summary>
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
        } 
        else 
        {
            Debug.Log("La contraseña y la confirmacion de la contraseña son diferentes");
        }

        Debug.Log("Enviando codigo de activacion a " + email.text);
        ConnectionManager.instance.socket.Emit("activation", newUserInfo);
    }

    // public void ClickResend()
    // {
    //     // Si necesito el email de la pantalla anterior deberia guardar el email en una variable fuera del metodo?
    //     InputField email = GameObject.Find("TFEmail").GetComponent<InputField>();
    //     Debug.Log("Activation code to: " + email.text);
    //     ConnectionManager.instance.socket.Emit("activation", email.text);
    // }

    /// <summary>
    /// Metodo que se ejecuta al presionar invitar (invitar jugadores a la partida)
    /// </summary>
    public void ClickInvite()
    {
        InputField invitado = GameObject.Find("TFEmail").GetComponent<InputField>();
        Debug.Log("Invitation to: " + invitado.text);
        ConnectionManager.instance.socket.Emit("invitation", invitado.text);
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar validar (Valida el codigo ingresado contra el codigo de activacion)
    /// </summary>
    public void ClickValidate()
    {
        // comparacion entre el codigo enviado y el codigo ingresado
        InputField code = GameObject.Find("TFCode").GetComponent<InputField>();
        Debug.Log("Validando codigo");
        ConnectionManager.instance.socket.Emit("sendActivationCode", code.text);
    }

    /*/	public void CopyToClipboard()
        {
            Text invitationCode = GameObject.Find("TxtCode").GetComponent<Text>();
            EditorGUIUtility.systemCopyBuffer = invitationCode.text;
            SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue ("message_lobby_code_copied"),
                SSTools.Position.bottom, SSTools.Time.oneSecond);
        }*/
}