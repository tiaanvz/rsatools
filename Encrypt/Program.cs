using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;

namespace Encrypt {
    class Program {
        static void Main(string[] args) {
            string data;
            try {
                data = args[1];
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Encrypt.exe <data>");
                return;
            }
            Console.WriteLine("Enter Base64 Public Key:");
            string base64pub = Console.ReadLine();
            string enc = Encrypt(data, base64pub);
            Console.WriteLine("Ecrypted Text:");
            Console.WriteLine(enc);
        }

        private static string Encrypt(string data, string base64pub) {
            var utf8data = Encoding.UTF8.GetBytes(data);
            using (var rsa = new RSACryptoServiceProvider()) {
                string xmlpub = GetXmlPubKey(base64pub);
                rsa.FromXmlString(xmlpub);
                var encrypteddata = rsa.Encrypt(utf8data, false);
                return Convert.ToBase64String(encrypteddata);
            }
        }

        private static string GetXmlPubKey(string base64pub) {
            var utf8pub = Convert.FromBase64String(base64pub);
            return Encoding.UTF8.GetString(utf8pub);
        }
    }
}
