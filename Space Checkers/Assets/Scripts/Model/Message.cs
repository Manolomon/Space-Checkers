using System;

[System.Serializable]
public class Message
{
    public string IdLobby { get; set; }
    public string Color { get; set; }
    public string Mensaje { get; set; }

    /// <summary>
    /// Inicializacion de una nueva instancia de la clase Message
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="color">Color.</param>
    /// <param name="msj">Msj.</param>
    public Message(string id,string color, string msj) 
    {
        IdLobby = id;
        Color = color;
        Mensaje = msj;
    }

}
