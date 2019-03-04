using System;
using System.Security.Cryptography;
using Xunit;

namespace Ecd2Pem.Test
{
    public class EcdPemTests
    {
        [Fact]
        public void CanConvertFromCngKey()
        {
            CngKey cngKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null, new CngKeyCreationParameters
            {
                ExportPolicy = CngExportPolicies.AllowPlaintextExport, 
            });
            var pemString = EcdConverter.FromCngKey(cngKey);
        }

        [Fact]
        public void CanConvertPublicKey()
        {
            // export by CngKey and then Base64 converted.
            string publicKey = "RUNLMSAAAAAE4GoZ96sN5mEJjsrDndtDDg8wP5eJjz0IS/vTucWJEp1yJmdhLEaxJp4it5ZrBRBHvYWUbsA6WncRkwGp/oHZ";
            var pemString = EcdConverter.FromPublicKey(publicKey);
        }
    }
}
