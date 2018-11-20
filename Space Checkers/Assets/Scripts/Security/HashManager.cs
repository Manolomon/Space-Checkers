using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

public class HashManager : MonoBehaviour 
{

	public static string GeneratePasswordHash (string password)
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

	public static bool CheckPasswordHash (string password, string hash)
	{
		string passwordHash = GeneratePasswordHash(password);
		return passwordHash == hash;
	}
}