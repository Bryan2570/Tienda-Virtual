using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sistema.Venta.BLL.Interfaces;
using System.Security.Cryptography;

namespace Sistema.Venta.BLL.Implementacion
{
    public class UtilidadesService : IUtilidadesService
    {
        
        public string GenerarClave()
        {
            //Nos retorna una cadena de texto aleatoria ("N") ==>> formato N indica que estamos utilizando num y letras
           string clave = Guid.NewGuid().ToString("N").Substring(0,6); // nos retorna cadena de texto aleatoria 'Guid.NewGuid' ,, Substring(0,6) indicamos que 6 digitos
            return clave;
        }

        //Encriptamos nuestro texto en SHA256
        public string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) {  // creamos el objeto para encriptar el texto
            
            Encoding enc = Encoding.UTF8; // formato enconding

                byte[] result = hash.ComputeHash(enc.GetBytes(texto)); //convierte el texto en una array de bytes

                foreach (byte b in result) { 
                
                    sb.Append(b.ToString("X2"));
                
                }
            }
            return sb.ToString();
        }

    }
}
