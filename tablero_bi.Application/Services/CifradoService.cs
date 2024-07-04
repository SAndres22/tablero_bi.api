using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.Application.Services
{
    public class CifradoService : ICifradoService
    {

       private readonly IConfiguration _config;

        public CifradoService(IConfiguration config)
        {
            _config = config;
        }

        public string Encriptar(string cadena)
        {
            if (string.IsNullOrEmpty(cadena)) return string.Empty;

            var claveBytes = Encoding.ASCII.GetBytes(_config["Cifrado:Clave"]);
            var ivBytes = Encoding.ASCII.GetBytes(_config["Cifrado:IV"]);

            var inputBytes = Encoding.ASCII.GetBytes(cadena);
            var cripto = new RijndaelManaged();

            byte[] encripted;

            using (var ms = new MemoryStream(inputBytes.Length))
            {
                using (var objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(claveBytes, ivBytes), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }
    }
}
