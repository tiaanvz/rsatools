using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenRsa {
    class Program {

        static void Main(string[] args) {
            var (xmlPriv, xmlPub) = GenKeys();
            var base64Priv = Protect(xmlPriv);
            Console.WriteLine("DP Base64");
            Console.WriteLine("==========");
            Console.WriteLine(base64Priv);
            File.WriteAllText($"PrivKey_{Environment.MachineName}", base64Priv);

            var utf8 = Encoding.UTF8.GetBytes(xmlPub);
            var base64Pub = Convert.ToBase64String(utf8);
            File.WriteAllText($"PubKey", base64Pub);
        }

        private static string Protect(string xml) {
            var utf8 = Encoding.UTF8.GetBytes(xml);
            var encypted = ProtectedData.Protect(utf8, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encypted);
        }

        private static (string, string) GenKeys() {
            using (var rsa = new RSACryptoServiceProvider()) {
                rsa.PersistKeyInCsp = false;
                var xmlPriv = rsa.ToXmlString(true);
                var xmlPub = rsa.ToXmlString(false);
                Console.WriteLine("new keys");
                Console.WriteLine("========");
                Console.WriteLine("private key:");
                Console.WriteLine("------------");
                Console.WriteLine(xmlPriv);
                File.WriteAllText($"PrivKeyXml", xmlPriv);
                Console.WriteLine();
                Console.WriteLine("public key:");
                Console.WriteLine("-----------");
                Console.WriteLine(xmlPub);
                File.WriteAllText($"PubKeyXml", xmlPub);
                return (xmlPriv, xmlPub);
            }
        }
    }
}
