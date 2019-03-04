using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecd2Pem
{
    public class PemPart
    {
        public PemPartTypes Type { get; set; }

        public byte[] Body { get; set; }

        public int Length => Body.Length;

        public byte[] Serialize()
        {
            return new[] { (byte)Type, (byte)Length }.Concat(Body).ToArray();
        }

        public PemPart GetPartByType(PemPartTypes type)
        {
            int offset = 0;
            while (offset < Body.Length)
            {
                var currentType = Body[offset];
                if (currentType == (byte)type)
                {
                    return Create(Body, offset);
                }
                offset += 2 + Body[offset + 1];
            }

            throw new KeyNotFoundException(nameof(type));
        }

        public string BuildPublicKeyPem()
        {
            var pk = Convert.ToBase64String(Serialize(),
                Base64FormattingOptions.InsertLineBreaks);
            return $"-----BEGIN PUBLIC KEY-----\r\n{pk}\r\n-----END PUBLIC KEY-----";
        }

        public string BuildPem()
        {
            var ecpPart = GetPartByType(PemPartTypes.EcPart).GetPartByType(PemPartTypes.EcParameter);
            var ecp = Convert.ToBase64String(ecpPart.Serialize(), Base64FormattingOptions.InsertLineBreaks);
            var sk = Convert.ToBase64String(Serialize(), Base64FormattingOptions.InsertLineBreaks);
            var p1 = $"-----BEGIN EC PARAMETERS-----\r\n{ecp}\r\n-----END EC PARAMETERS-----\r\n";
            var p2 = $"-----BEGIN EC PRIVATE KEY-----\r\n{sk}\r\n-----END EC PRIVATE KEY-----";
            return p1 + p2;
        }

        public static PemPart Create(byte[] bytes, int offset)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length < offset + 1 &&
                bytes.Length - offset < 2 + bytes[offset + 1])
                throw new ArgumentOutOfRangeException(nameof(bytes));

            return new PemPart
            {
                Type = (PemPartTypes)bytes[offset],
                Body = bytes.Skip(2 + offset).Take(bytes[1 + offset]).ToArray()
            };
        }

        public static PemPart Create(PemPartTypes type, byte[] body)
        {
            return new PemPart
            {
                Type = type,
                Body = body
            };
        }

        internal static PemPart Create(PemPartTypes type, params PemPart[] parts)
        {
            return new PemPart
            {
                Type = type,
                Body = parts.Select(x => x.Serialize()).SelectMany(x => x).ToArray()
            };
        }

        public static PemPart BuildPemPart(byte[] privateKey, byte[] publicKey)
        {
            var versionPart = Create(PemPartTypes.Version, new byte[] { 1 });
            var privateKeyPart = Create(PemPartTypes.PrivateKey, privateKey);
            var ecpPart = EcParameterPack;
            var publicKeyPart = BuildPublicKeyPack(publicKey);

            return Create(PemPartTypes.Ecdsa,
                versionPart, privateKeyPart, ecpPart, publicKeyPart);
        }

        public static PemPart EcParameter = new PemPart
        {
            Type = PemPartTypes.EcParameter,
            Body = new byte[] { 42, 134, 72, 206, 61, 3, 1, 7 }
        };

        public static PemPart EcParameterPack = Create(PemPartTypes.EcPart, EcParameter);

        public static PemPart BuildPublicKeyPack(byte[] publicKey)
        {
            return Create(PemPartTypes.PublicKeyPack, new byte[] { 3, 66, 0, 4 }.Concat(publicKey).ToArray());
        }

        public static PemPart BuildPublicKey(byte[] publicKey)
        {
            return Create(PemPartTypes.PublicKey, new byte[] { 0, 4 }.Concat(publicKey).ToArray());
        }
    }
}
