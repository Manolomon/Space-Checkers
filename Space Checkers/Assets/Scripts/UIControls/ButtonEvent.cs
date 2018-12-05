using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {

    private GameObject BuscarObjetoInactivo(string nombre)
    {
        GameObject resultado = null;
        GameObject[] objetos = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in objetos)
        {
            if (go.name == nombre)
            {
                resultado = go;
            }
        }
        return resultado;
   }

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
        InputField code = GameObject.Find("TFCode").GetComponent<InputField>(); 
        Dictionary<string, string> join = new Dictionary<string, string>();
        join.Add("IdLobby", code.text);
        join.Add("Jugador", Jugador.instance.Username);
        string json = JsonConvert.SerializeObject(join);
		ConnectionManager.instance.socket.Emit("joinGame", json); 
        ConnectionManager.instance.ToJoin = true;
    }

    /// <summary>
    /// Metodo que se ejecuta para unirse a una partida como invitado
    /// </summary>
    public void ClickJoinAsGuest()
    {
        InputField code = GameObject.Find("TFGameCode").GetComponent<InputField>(); 
		ConnectionManager.instance.socket.Emit("joinGameGuest", code.text); 
		ConnectionManager.instance.ToJoin = true;
    }

    /// <summary>
    /// Metodo para crear el juego al dar click a crear juego
    /// </summary>
    public void ClicklCreateGame() 
	{ 
		ConnectionManager.instance.socket.Emit("createGame"); 
	} 

    /// <summary>
    /// Metodo para salir de la sala o partida
    /// </summary>
    public void ClickLeave()
    {
        if (Jugador.instance.Correo != null)
        {
            SceneManager.LoadScene(4);
        } else {
            SceneManager.LoadScene(1);
        }
        Dictionary<string, string> leave = new Dictionary<string, string>();
        leave.Add("IdLobby", Lobby.instance.IdLobby);
        leave.Add("Jugador", Jugador.instance.Username);
        string json = JsonConvert.SerializeObject(leave);
        ConnectionManager.instance.socket.Emit("leaveGame", json);
        Lobby.instance.LimpiarLobby();
    }

    /// <summary>
    /// Metodo para iniciar la partida en el lobby determinado
    /// </summary>
    public void ClickStartGame()
    {
        Lobby.instance.StartGame();
    }

    /// <summary>
    /// Metodo para enviar la informacion de si la prediccion esta activada o no
    /// </summary>
    public void ClickPrediction()
    {
        Dictionary<string, string> predictionn = new Dictionary<string, string>();
        predictionn.Add("IdLobby",Lobby.instance.IdLobby);
        string prediccion = GameObject.Find("TogglePrediction").GetComponent<Toggle>().isOn.ToString();
        predictionn.Add("Prediccion", prediccion);
        string json = JsonConvert.SerializeObject(predictionn);
        ConnectionManager.instance.socket.Emit("prediction",json);
        Debug.Log(json);
    }

    /// <summary>
    /// Metodo para actualizar el score de los jugadores al terminar las partidas
    /// </summary>
    public void ActualizarScore()
    {
        if (Jugador.instance.Correo != null)
        {
            Dictionary<string, int> score = new Dictionary<string, int>();
            score.Add("jugadas", Jugador.instance.PartidasJugadas);
            score.Add("ganadas", Jugador.instance.PartidasGanadas);
            score.Add("idPlayer", Jugador.instance.Id);
            string json = JsonConvert.SerializeObject(score);
            ConnectionManager.instance.socket.Emit("updateJugador", json);
        }
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar enviar codigo (envia el codigo de activacion para registrarse)
    /// </summary>
    public void ClickSendConfirmation()
    {
        InputField username = GameObject.Find("/Canvas/SignUpPanel/TFUsername").GetComponent<InputField>();
        Debug.Log(username);
        InputField correo = GameObject.Find("/Canvas/SignUpPanel/TFEmail").GetComponent<InputField>();
        InputField password = GameObject.Find("/Canvas/SignUpPanel/TFPassword").GetComponent<InputField>();
        InputField confirmationPass = GameObject.Find("/Canvas/SignUpPanel/TFConfirmation").GetComponent<InputField>();
        
        Debug.Log("Comparando contraseñas");
        string hashPass = HashManager.GeneratePasswordHash(password.text);
        string hashPassConfirmation = HashManager.GeneratePasswordHash(confirmationPass.text);
        
        if (username.text == null || correo.text == null || password.text == null || confirmationPass == null)
        {
           Debug.Log("Hay campos vacios");       
           SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue("message_empty_fields"),
                SSTools.Position.bottom, SSTools.Time.oneSecond);
        } else   {
            if (hashPassConfirmation.Equals(hashPass))
            {
                GameObject.Find("/Canvas/SignUpPanel").SetActive(false);
                GameObject VerificationSignUpPanel = BuscarObjetoInactivo("VerificationSignUpPanel");
                VerificationSignUpPanel.SetActive(true);
                
                string nombre = username.text;
                string email = correo.text;
                
                Jugador.instance.Username = nombre;
                Jugador.instance.Pass = hashPass;
                Jugador.instance.Correo = email;
                Jugador.instance.PartidasGanadas = 0;
                Jugador.instance.PartidasJugadas = 0;
                
                Debug.Log("Datos antes de clase mail = " + nombre + ", " + email);
                MailMessage mail = new MailMessage(nombre, email);
                string mailData = JsonConvert.SerializeObject(mail);
                Debug.Log("JSON = " + mailData);
                
                Debug.Log("Enviando codigo de activacion a " + correo.text);
                ConnectionManager.instance.socket.Emit("sendActivationCode", mailData);
            }  else  {
                Debug.Log("La contraseña y la confirmacion de la contraseñ son diferentes");
                SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue("passwords_diferent"),
                    SSTools.Position.bottom, SSTools.Time.oneSecond);
            }
        }
    }

    public void ClickResend()
    {
        //     // Si necesito el email de la pantalla anterior deberia guardar el email en una variable fuera del metodo?
        //     InputField email = GameObject.Find("TFEmail").GetComponent<InputField>();
        string nombre = Jugador.instance.Username;
        string email = Jugador.instance.Correo;

        Debug.Log("Reenviando correo a " + email);
        MailMessage mail = new MailMessage(nombre, email);
        string mailData = JsonConvert.SerializeObject(mail);
        Debug.Log("JSON = " + mailData);

        ConnectionManager.instance.socket.Emit("sendActivationCode", mailData);
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar invitar (invitar jugadores a la partida)
    /// </summary>
    public void ClickInvite()
    {
        InputField invitado = GameObject.Find("/Canvas/InvitationPanel/TFEmail").GetComponent<InputField>();
       Debug.Log("Invitation to: " + invitado.text);
        string sender = Jugador.instance.Username;
        string codigo = Lobby.instance.IdLobby;
        MailMessage mail = new MailMessage(sender, invitado.text, codigo);
        string mailData = JsonConvert.SerializeObject(mail);
        ConnectionManager.instance.socket.Emit("sendInvitation", mailData);
    }

    /// <summary>
    /// Metodo que se ejecuta al presionar validar (Valida el codigo ingresado contra el codigo de activacion)
    /// </summary>
    public void ClickValidate()
    {
        // comparacion entre el codigo enviado y el codigo ingresado
        InputField code = GameObject.Find("/Canvas/VerificationSignUpPanel/TFCode").GetComponent<InputField>();
        Debug.Log("Validando codigo");
        string codigo = ConnectionManager.instance.Code;
        
        if (codigo.Equals(code.text))
        {
            Debug.Log("Codigo valido");
            
            string username = Jugador.instance.Username;
            string correo = Jugador.instance.Correo;
            string pass = Jugador.instance.Pass;
            int partidasGanadas = Jugador.instance.PartidasGanadas;
            int partidasJugadas = Jugador.instance.PartidasJugadas;
            
            JugadorBD nuevoJugador = new JugadorBD(username, correo, pass, partidasJugadas, partidasGanadas);
            
            string jugador = JsonConvert.SerializeObject(nuevoJugador);
            Debug.Log("JSON = " + jugador);
            ConnectionManager.instance.socket.Emit("registro", jugador);

            GameObject VerificationSignUpPanel = BuscarObjetoInactivo("VerificationSignUpPanel");
            VerificationSignUpPanel.SetActive(false);
            
            GameObject Blur = BuscarObjetoInactivo("Blurred Sheet");
            Blur.SetActive(false);
            
            }  else  {
                Debug.Log("Codigo incorrecto");
                SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue("activation_code_diferent"),
                    SSTools.Position.bottom, SSTools.Time.oneSecond);
            }
       }

    /// <summary>
    /// Metodo que se ejecuta al presionar el código para copiar en Portapapeles
    /// </summary>
    public void CopyToClipboard()
    {
        Text invitationCode = GameObject.Find("TxtCode").GetComponent<Text>();
        //EditorGUIUtility.systemCopyBuffer = invitationCode.text;
        SSTools.ShowMessage(LocalizationManager.instance.GetLocalizedValue("message_lobby_code_copied"),
            SSTools.Position.bottom, SSTools.Time.oneSecond);
    }
}