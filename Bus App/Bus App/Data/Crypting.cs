using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BusApp.Data
{
    internal class Crypting
    {
        public static string ToSha256(string password)
        {
            var sha256 = SHA256.Create();//New sha256 var 
            //Encrypts the given parameter and stores it in the sha256 var
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            var sb = new StringBuilder();

            for (int index = 0; index < bytes.Length; index++)
            {
                sb.Append(bytes[index].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
