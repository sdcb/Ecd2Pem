## Ecd2Pem [![NuGet](https://img.shields.io/badge/nuget-0.15.0-blue.svg)](https://www.nuget.org/packages/sdmap)

## How to use?
Install nuget package `Ecd2Pem`

# If you have CngKey: 
```string pemString = EcdConverter.FromCngKey(cngKey);```
and you'll get: 
`-----BEGIN EC PARAMETERS-----
BggqhkjOPQMBBw==
-----END EC PARAMETERS-----
-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIGjyyaEK0W+ErAsgeSEux7weYuR69Twn2aJSmz0CAsR7oAoGCCqGSM49AwEHoUQDQgAE
nu3lr+fdixqx7+4FSTfUWyIuwHlE1NzVHR+IkKLKqRGdQqAsy2GVmi99MI75ZbqyWGuXtpacBHjz
5uNTRpUJCQ==
-----END EC PRIVATE KEY-----`

# If you have a public key(generated from base64 converted public key): 
`string pemString = EcdConverter.FromPublicKey("RUNLMSAAAAAE4GoZ96sN5mEJjsrDndtDDg8wP5eJjz0IS/vTucWJEp1yJmdhLEaxJp4it5ZrBRBHvYWUbsA6WncRkwGp/oHZ")```
And you'll get:
```-----BEGIN PUBLIC KEY-----
oUQDQgAEBOBqGferDeZhCY7Kw53bQw4PMD+XiY89CEv707nFiRKdciZnYSxGsSaeIreWawUQR72F
lG7AOlp3EZMBqf6B2Q==
-----END PUBLIC KEY-----`
