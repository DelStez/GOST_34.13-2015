using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOST_34._13_2015
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Записываем путь к файлу в textbox
                FilePathBox.Text = openFileDialog1.FileName;
                // Считываем содержимое файла в строку
                string fileContent = File.ReadAllText(openFileDialog1.FileName);
                // Выводим содержимое файла в текстовый элемент управления
                ContentOpenText.Text = fileContent;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BoxManipulate boxManipulate = new BoxManipulate();
            string mode = comboBox1.SelectedItem.ToString();
            byte[] keyBytes = Encoding.UTF8.GetBytes(textBox5.Text);
            ElectronicCodebook codebook = new ElectronicCodebook();
            if (mode == comboBox1.Items[0].ToString())
            {
                List<byte[]> blockOpenText = boxManipulate.splitText(ContentOpenText.Text);
                List<byte[]> cipherText = codebook.cipher(blockOpenText, keyBytes);
                textBox1.Text = "";
                foreach (byte[] block in cipherText)
                {
                    textBox3.Text += Encoding.UTF8.GetString(block);
                }
            }
            else
            {
                List<byte[]> blockCipherText = boxManipulate.splitText(ContentOpenText.Text);
                List<byte[]> OpenTextByte = codebook.decipher(blockCipherText, keyBytes);
                textBox3.Text = "";
                foreach (byte[] block in OpenTextByte)
                {
                    textBox3.Text += Encoding.UTF8.GetString(block);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoxManipulate boxManipulate = new BoxManipulate();
            string mode = comboBox1.SelectedItem.ToString();
            byte[] keyBytes = Encoding.UTF8.GetBytes(textBox1.Text);
            CipherBlockChaining cipherBlockChaining = new CipherBlockChaining();
            if (mode == comboBox1.Items[0].ToString())
            {
                List<byte[]> blockOpenText = boxManipulate.splitText(ContentOpenText.Text);
                List<byte[]> cipherText = cipherBlockChaining.cipher(blockOpenText, keyBytes);
                textBox2.Text = "";
                foreach (byte[] block in cipherText)
                {
                    textBox2.Text += Encoding.UTF8.GetString(block);
                }
            }
            else
            {
                List<byte[]> blockCipherText = boxManipulate.splitText(ContentOpenText.Text);
                List<byte[]> OpenTextByte = cipherBlockChaining.cipher(blockCipherText, keyBytes);
                textBox2.Text = "";
                foreach (byte[] block in OpenTextByte)
                {
                    textBox2.Text += Encoding.UTF8.GetString(block);
                }
            }
        }
    }
}