// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace AdventureWorks.UILogic.Services
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<string> EncryptMessage(string message)
        {
            var dataProtectionProvider = new DataProtectionProvider("LOCAL=user");

            // Encode the plaintext input message to a buffer.
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8);

            // Encrypt the message.
            IBuffer buffProtected = await dataProtectionProvider.ProtectAsync(buffMsg);
            
            return CryptographicBuffer.EncodeToBase64String(buffProtected);
        }

        public async Task<string> DecryptMessage(string base64EncodedMessage)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedMessage)) return string.Empty;

            var dataProtectionProvider = new DataProtectionProvider("LOCAL=user");
            var messageBuffer = CryptographicBuffer.DecodeFromBase64String(base64EncodedMessage);

            // Decrypt the message
            IBuffer buffUnProtected = await dataProtectionProvider.UnprotectAsync(messageBuffer);

            // Decode the buffer to a plaintext message.
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnProtected);
        }
    }
}
