using System;
using System.Collections.Generic;
using System.Text;

namespace GOST_34._13_2015
{
    public class BoxManipulate
    {
        public List<byte[]> splitText(string openText)
        {
            // список блоков
            List<byte[]> blocks = new List<byte[]>(); 
            
            // размер блока в байтах (128 бит = 16 байт)
            int blockSize = 16; 
            
            // определяем необходимое количество байтов для дополнения
            int padBytes = blockSize - (openText.Length % blockSize);
            
            
            // проверяем необходимость дополнения
            if (padBytes < blockSize)
            {
                byte pad = 0x80; // единичный бит для дополнения
                if (padBytes == 1)
                {
                    openText += "\x00"; // добавляем пустой байт
                }
                else
                {
                    // добавляем единичный бит
                    openText += new string('\0', padBytes - 1) + Convert.ToChar(pad);
                }
            }

            for (int i = 0; i < openText.Length; i += blockSize)
            {
                int length = Math.Min(blockSize, openText.Length - i); // определяем количество символов для текущего блока
                string block = openText.Substring(i, length); // получаем подстроку для текущего блока
                blocks.Add(Encoding.UTF8.GetBytes(block)); // добавляем текущий блок в список, преобразуя его в массив байтов
            }

            return blocks;
        }
    }
}