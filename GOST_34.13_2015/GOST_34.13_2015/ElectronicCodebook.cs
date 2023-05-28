using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace GOST_34._13_2015
{
    public class ElectronicCodebook
    {
        public List<byte[]> cipher(List<byte[]> blocks, byte[] key)
        {
            
            List<byte[]> encryptedBlocks = new List<byte[]>();
            // Создаем экземпляр класса AesManaged
            AesManaged aesAlg = new AesManaged();
            aesAlg.Key = key;
            aesAlg.Mode = CipherMode.ECB;
            
            // Шифруем каждый блок открытого текста
            foreach (byte[] block in blocks)
            {
                // Шифруем блок и добавляем его в список зашифрованных блоков
                byte[] encryptedBlock = aesAlg.CreateEncryptor().TransformFinalBlock(block, 0, block.Length);
                encryptedBlocks.Add(encryptedBlock);
            }

            return encryptedBlocks;
        }

        public List<byte[]> decipher(List<byte[]> encryptedBlocks, byte[] key)
        {
            AesManaged aesAlg = new AesManaged();
            aesAlg.Key = key;
            // Устанавливаем режим шифрования (например, ECB)
            aesAlg.Mode = CipherMode.ECB;

            // Дешифруем каждый блок зашифрованного текста
            List<byte[]> decryptedBlocks = new List<byte[]>();
            foreach (byte[] encryptedBlock in encryptedBlocks)
            {
                // Дешифруем блок и добавляем его в список расшифрованных блоков
                byte[] decryptedBlock = aesAlg.CreateDecryptor().TransformFinalBlock(encryptedBlock, 0, encryptedBlock.Length);
                decryptedBlocks.Add(decryptedBlock);
            }

            return decryptedBlocks;
        }
    }
}