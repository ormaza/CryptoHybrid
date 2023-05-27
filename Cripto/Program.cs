using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Cripto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Entrada entrada = new Entrada();
            entrada.texto = "ormazabal lima";
            entrada.chave = "AAAAAAAAAAAAAAAA";
            entrada.vi = "BBBBBBBBBBBBBBBB";

            string encriptada = Encrypt(entrada);
            entrada.texto = encriptada;
            string decriptada = Decrypt(entrada);

            Console.WriteLine("encriptado: " + encriptada);
            Console.WriteLine("decriptada: " + decriptada);

            DateTime date = DateTime.Now;
            int shift = (date.Year % date.Month) + (date.Day/date.Month);
            encriptada = PoorEncrypt("ormazabal lima do nascimento", shift);
            decriptada = PoorDecrypt(encriptada, shift);

            Console.WriteLine("encriptado: " + encriptada);
            Console.WriteLine("decriptada: " + decriptada);
        }

        static string Decrypt(Entrada entrada)
        {
            try
            {

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;

                byte[] encryptedData = Convert.FromBase64String(entrada.texto);

                byte[] pwdBytes = Encoding.UTF8.GetBytes(entrada.chave);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;

                byte[] pwdBytes2 = Encoding.UTF8.GetBytes(entrada.vi);
                byte[] viBytes = new byte[0x10];
                int len2 = viBytes.Length;
                if (len2 > viBytes.Length)
                {
                    len2 = viBytes.Length;
                }
                Array.Copy(pwdBytes2, viBytes, len2);
                rijndaelCipher.IV = viBytes;

                byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static string Encrypt(Entrada entrada)
        {
            try
            {
                
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;

                byte[] pwdBytes = Encoding.UTF8.GetBytes(entrada.chave);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;

                byte[] pwdBytes2 = Encoding.UTF8.GetBytes(entrada.vi);
                byte[] viBytes = new byte[0x10];
                int len2 = viBytes.Length;
                if (len2 > viBytes.Length)
                {
                    len2 = viBytes.Length;
                }
                Array.Copy(pwdBytes2, viBytes, len2);
                rijndaelCipher.IV = viBytes;

                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(entrada.texto);

                return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static string PoorEncrypt(string texto, int shift)
        {
            try
            {
                Random random = new Random();
                string nova = "";
                for (int i = 0; i < texto.Length; i++)
                {
                    nova += (char) (texto[i] + shift);
                    nova += (char)(random.Next(65, 122 + 1) % 255);
                    nova += (char)(random.Next(65, 122 + 1) % 255);
                }
                return "ios" + nova;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static string PoorDecrypt(string texto, int shift)
        {
            try
            {
                if (texto.Substring(0, 3) == "ios")
                {
                    string texto2;
                    texto2 = texto.Substring(3);
                    string nova = "";
                    for (int i = 0; i < texto2.Length; i+=3)
                    {
                        nova += (char)(texto2[i] - shift);
                    }
                    return nova;
                }
                else
                    return "nao tem ios";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
