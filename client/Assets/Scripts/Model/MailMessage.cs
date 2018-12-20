using System;

[System.Serializable]
public class MailMessage {
    public string Sender { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Codigo { get; set; }

    /// <summary>
    /// Inicializa nueva instancia de la clase MailMessage
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="nombre">Nombre.</param>
    /// <param name="codigo">Codigo.</param>
    public MailMessage(string sender, string nombre, string codigo)
    {
        Sender = sender;
        Name = nombre;
        Codigo = codigo;
    }

    /// <summary>
    /// Inicializa nueva instancia de la clase MailMessage
    /// </summary>
    /// <param name="nombre">Nombre.</param>
    /// <param name="correo">Correo.</param>
    public MailMessage(string nombre, string correo)
    {
        Name = nombre;
        Email = correo;
    }

}
