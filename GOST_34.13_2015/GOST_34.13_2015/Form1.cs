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
            if (mode == comboBox1.Items[0].ToString())
            {
                ElectronicCodebook c = new ElectronicCodebook(ContentOpenText.Text, textBox5.Text, false);
                string text = ElectronicCodebook.Begin();
                textBox3.Text = "";
                textBox3.Text += text;

            }
            else
            {
                ElectronicCodebook c = new ElectronicCodebook(ContentOpenText.Text, textBox5.Text, true);
                string text = ElectronicCodebook.Begin();
                textBox3.Text = "";
                textBox3.Text += text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoxManipulate boxManipulate = new BoxManipulate();
            CipherBlockChaining cipherBlockChaining = new CipherBlockChaining();
            string mode = comboBox1.SelectedItem.ToString();
            byte[] keyBytes = cipherBlockChaining.CorrectKey(textBox1.Text, 16);
            if (mode == comboBox1.Items[0].ToString())
            {
                byte[] cipherText = cipherBlockChaining.EncryptStringToBytes_Aes(ContentOpenText.Text, keyBytes);
                textBox2.Text = "";
                textBox2.Text += Encoding.UTF8.GetString(cipherText);
            }
            else
            {
                string cipherText = cipherBlockChaining.DecryptStringFromBytes_Aes(Encoding.UTF8.GetBytes(ContentOpenText.Text), keyBytes);
                textBox2.Text = "";
                textBox2.Text += cipherText;
            }
        }
    }
}