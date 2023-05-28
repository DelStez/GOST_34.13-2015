using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GOST_34._13_2015
{
    public class CipherBlockChaining
    {
        public byte[] CorrectKey(string key, int lengthKey)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            byte[] output = new byte[lengthKey];
            if (bkey.Length > lengthKey)
                for (int i = 0; i < lengthKey; i++)
                    output[i] = bkey[i];
            else
            {
                while (key.Length % lengthKey != 0)
                    key += "\0";
                bkey = Encoding.Default.GetBytes(key);
                for (int i = 0; i < lengthKey; i++)
                    output[i] = bkey[i];
                Encoding.Default.GetBytes(key).CopyTo(output, 0);
            }
            return output;
        }
        public byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
            return combinedIvCt;

        }

        public string DecryptStringFromBytes_Aes(byte[] cipherTextCombined, byte[] Key)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                byte[] IV = new byte[aesAlg.BlockSize/8];
                byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];
                
                Array.Copy(cipherTextCombined, IV, IV.Length);
                Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
        
    }
}