using System;

[System.Serializable]
public class Message
{
    public string Color { get; set; }
    public string Mensaje { get; set; }

    public Message(string color, string msj) 
    {
        Color = color;
        Mensaje = msj;
    }

}
