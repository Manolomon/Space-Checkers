using System;

[System.Serializable]
public class Message
{
    public string IdLobby { get; set; }
    public string Color { get; set; }
    public string Mensaje { get; set; }

    public Message(string id,string color, string msj) 
    {
        IdLobby = id;
        Color = color;
        Mensaje = msj;
    }

}
