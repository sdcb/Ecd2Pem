namespace Ecd2Pem
{
    public enum PemPartTypes : byte
    {
        Version = 2, 
        PublicKey = 3, 
        PrivateKey = 4, 
        EcParameter = 6, 
        Ecdsa = 48, 
        EcPart = 160, 
        PublicKeyPack = 161, 
    }
}
