using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Decrypt {
    class Program {
        static void Main(string[] args) {
            string enc;
            try {
                enc = args[0];
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Encrypt.exe <data>");
                return;
            }
            Console.WriteLine("Enter Base64 ProtectedData Private Key:");
            string base64priv = Console.ReadLine();
            string data = Unprotect(enc, base64priv);
            Console.WriteLine("Ecrypted Text:");
            Console.WriteLine(data);
        }

        private static string Unprotect(string encrypteddata, string base64priv) {
            string xmlpriv = GetXmlPrivKey(base64priv);
            using (var rsa = new RSACryptoServiceProvider()) {
                rsa.FromXmlString(xmlpriv);
                var encryptedbytes = Convert.FromBase64String(encrypteddata);
                var utf8data = rsa.Decrypt(encryptedbytes, false);
                return Encoding.UTF8.GetString(utf8data);
            }
        }

        private static string GetXmlPrivKey(string base64priv) {
            var encryptedpriv = Convert.FromBase64String(base64priv);
            var utf8privkey = ProtectedData.Unprotect(encryptedpriv, null, DataProtectionScope.LocalMachine);
            return Encoding.UTF8.GetString(utf8privkey);
        }
    }
}
