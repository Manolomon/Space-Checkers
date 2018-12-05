using System;

[System.Serializable]
public class Mensaje {
    public string IdLobby { get; set; }
    public string Color { get; set; }
    public string InformacionMensaje { get; set; }

    /// <summary>
    /// Inicializacion de una nueva instancia de la clase Message
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="color">Color.</param>
    /// <param name="informacionMensaje">Msj.</param>
    public Mensaje(string id, string color, string informacionMensaje) 
    {
        IdLobby = id;
        Color = color;
        InformacionMensaje = informacionMensaje;
    }

}
