﻿using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using SigningServer.Core;

namespace SigningServer.Server.SigningTool;

internal class ManagedHashSigningTool : IHashSigningTool
{
    public SignHashResponse SignHash(SignHashRequest signHashRequest)
    {
        if (!TryGetHashAlgorithm(signHashRequest.HashAlgorithm, out var algorithmName))
        {
            return new SignHashResponse
            {
                Status = SignHashResponseStatus.HashNotSignedUnsupportedFormat,
                ErrorMessage = $"Hash algorithm '{signHashRequest.HashAlgorithm}' is not known"
            };
        }

        return new SignHashResponse
        {
            Signature = SignInternal(signHashRequest, algorithmName), Status = SignHashResponseStatus.HashSigned,
        };
    }

    private static bool TryGetHashAlgorithm(string hashAlgorithm, out HashAlgorithmName hashAlgorithmName)
    {
        hashAlgorithm = hashAlgorithm.ToUpperInvariant();
        switch (hashAlgorithm)
        {
            case "MD5":
            case "MD-5":
                hashAlgorithmName = HashAlgorithmName.MD5;
                return true;
            case "SHA1":
            case "SHA-1":
                hashAlgorithmName = HashAlgorithmName.SHA1;
                return true;
            case "SHA256":
            case "SHA-256":
                hashAlgorithmName = HashAlgorithmName.SHA256;
                return true;
            
            case "SHA386":
            case "SHA-386":
                hashAlgorithmName = HashAlgorithmName.SHA384;
                return true;            
            case "SHA512":
            case "SHA-512":
                hashAlgorithmName = HashAlgorithmName.SHA512;
                return true;
            default:
                try
                {
                    hashAlgorithmName = HashAlgorithmName.FromOid(hashAlgorithm);
                    return true;
                }
                catch (CryptographicException)
                {
                    hashAlgorithmName = HashAlgorithmName.SHA256;
                    return false;
                }
        }
    }

    private static byte[] SignInternal(SignHashRequest signHashRequest, HashAlgorithmName hashAlgorithmName)
    {
        return signHashRequest.PrivateKey switch
        {
            RSA rsa => rsa.SignHash(signHashRequest.InputHash, hashAlgorithmName, RSASignaturePadding.Pkcs1),
            DSA dsa => dsa.SignData(signHashRequest.InputHash, hashAlgorithmName),
            ECDsa ecdsa => ecdsa.SignHash(signHashRequest.InputHash),
            _ => throw new ArgumentOutOfRangeException(
                $"Unsupported private key {signHashRequest.PrivateKey.GetType().FullName}")
        };
    }
}
