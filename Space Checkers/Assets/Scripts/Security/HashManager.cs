using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

/// <summary>Gestor de validación de contraseñas a partir de su hash</summary>
public class HashManager : MonoBehaviour {

	/// <summary>Se genera un valor hash para la contraseña ingresada</summary>
    /// <param name="password">Contraseña por cifrar</param>
	/// <returns>Valor hash generado</returns>
	public static string GeneratePasswordHash(string password)
	{
		StringBuilder Sb = new StringBuilder();
		using (var hash = SHA256.Create())            
		{
			Encoding encoding = Encoding.UTF8;
			Byte[] result = hash.ComputeHash(encoding.GetBytes(password));
			foreach (Byte b in result)
			{
				Sb.Append(b.ToString("x2"));
			}
		}

		return Sb.ToString();
	}

	 /// <summary>Validación de contraseñas</summary>
    /// <param name="password">Contraseña ingresada</param>
    /// <param name="hash">Valor hash de la contraseña original</param>
	/// <returns>Verificación de que son iguales</returns>
	public static bool CheckPasswordHash (string password, string hash)
	{
		string passwordHash = GeneratePasswordHash(password);
		return passwordHash == hash;
	}
}