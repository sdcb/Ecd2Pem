using System;
using System.Linq;
using System.Security.Cryptography;

namespace Ecd2Pem
{
    public static class EcdConverter
    {
        public static string FromCngKey(CngKey cngKey)
        {
            KeyPair keyPair = KeyPairFromCngKey(cngKey);
            return FromKeyPair(keyPair.PrivateKey, keyPair.PublicKey);
        }

        public static string FromKeyPair(byte[] privateKey, byte[] publicKey)
        {
            PemPart pem = PemPart.BuildPemPart(privateKey, publicKey);
            return pem.BuildPem();
        }

        public static string FromKeyPair(string privateKey, string publicKey)
        {
            var privateKeyBlob = Convert.FromBase64String(privateKey).Skip(8 + 64).Take(32).ToArray();
            var publicKeyBlob = Convert.FromBase64String(publicKey).Skip(8).Take(64).ToArray();
            PemPart pem = PemPart.BuildPemPart(privateKeyBlob, publicKeyBlob);
            return pem.BuildPem();
        }

        public static string FromPublicKey(byte[] publicKey)
        {
            PemPart pem = PemPart.BuildPublicKeyPack(publicKey);
            return pem.BuildPublicKeyPem();
        }

        public static string FromPublicKey(string publicKey)
        {
            var publicKeyBlob = Convert.FromBase64String(publicKey).Skip(8).Take(64).ToArray();
            PemPart pem = PemPart.BuildPublicKeyPack(publicKeyBlob);
            return pem.BuildPublicKeyPem();
        }

        private static KeyPair KeyPairFromCngKey(CngKey cngKey)
        {
            byte[] blob = cngKey.Export(CngKeyBlobFormat.EccPrivateBlob);
            return new KeyPair
            {
                PublicKey = blob.Skip(8).Take(64).ToArray(), 
                PrivateKey = blob.Skip(8 + 64).Take(32).ToArray(), 
            };
        }
    }
}
