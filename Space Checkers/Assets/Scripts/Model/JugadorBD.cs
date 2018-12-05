using System;

[System.Serializable]
public class JugadorBD
{
    public string Username { get; set; }
    public string Correo { get; set; }
    public string Pass { get; set; }
    public int PartidasJugadas { get; set; }
    public int PartidasGanadas { get; set; }
    public int Id { get; set; }

    public JugadorBD (string username, string correo, string pass, int partidasJugadas, int partidasGanadas)
    {
        Username = username;
        Correo = correo;
        Pass = pass;
        PartidasJugadas = partidasJugadas;
        PartidasGanadas = partidasGanadas;
    }
}
