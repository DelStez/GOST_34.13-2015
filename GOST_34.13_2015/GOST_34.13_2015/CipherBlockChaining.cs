using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GOST_34._13_2015
{
    public class CipherBlockChaining
    {
        public List<byte[]> cipher(List<byte[]> blocks, byte[] key)
        {
            
            List<byte[]> encryptedBlocks = new List<byte[]>();
            // Создаем экземпляр класса AesManaged
            AesManaged aesAlg = new AesManaged();

            // Устанавливаем ключ шифрования
            aesAlg.Key = key;

            // Устанавливаем режим шифрования (CTR)
            aesAlg.Mode = CipherMode.CBC;

            // Устанавливаем счетчик (nonce) и значение счетчика (counter)
            byte[] nonce = new byte[8]; // 64-битное случайное значение
            byte[] counter = new byte[8]; // начальное значение счетчика (например, 0)
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, nonce);

            // Шифруем каждый блок открытого текста
            foreach (byte[] block in blocks)
            {
                // Получаем значение счетчика для текущего блока
                byte[] counterBlock = new byte[16];
                counter.CopyTo(counterBlock, 0);
                nonce.CopyTo(counterBlock, 8);

                // Шифруем текущий блок
                byte[] encryptedBlock = new byte[block.Length];
                encryptor.TransformBlock(counterBlock, 0, 16, encryptedBlock, 0);
                for (int i = 0; i < block.Length; i++)
                {
                    encryptedBlock[i] ^= block[i];
                }

                // Добавляем зашифрованный блок в список
                encryptedBlocks.Add(encryptedBlock);

                // Увеличиваем значение счетчика на единицу
                for (int i = counter.Length - 1; i >= 0; i--)
                {
                    counter[i]++;
                    if (counter[i] != 0) // если не произошло переполнение, выходим из цикла
                    {
                        break;
                    }
                }
            }

            return encryptedBlocks;
        }
    }
}