using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private byte[] inText;
        private byte[] outText;
        private byte[] keyByte;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;   
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            FileToBitSequence(filename);
            richTextBox1.Text = BytesToBitSequence(inText);
            MessageBox.Show("Файл открыт");
        }
    

        private void FileToBitSequence(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            inText = bytes;
        }

        private bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            try
            {
                Writer = new BinaryWriter(File.OpenWrite(FileName));
             
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        static string BytesToBitSequence(byte[] bytes)
        {
            StringBuilder binaryString = new StringBuilder();

            foreach (byte b in bytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    binaryString.Append((b & (1 << i)) != 0 ? "1" : "0");
                }
                if (binaryString.Length > 1500) break;
            }

            return binaryString.ToString();
        }


        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            textBox1.Clear();
        }

        private void Alg(ulong reg)
        {
            LFSR lfsr = new LFSR(reg);
            //richTextBox3.Text
            keyByte = lfsr.GetKey(inText.Length);
            richTextBox3.Text = BytesToBitSequence(keyByte);
            //richTextBox2.Text=
            outText = lfsr.Shifr(keyByte, inText);
            richTextBox2.Text = BytesToBitSequence(outText);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string regStr = ReadKey();
            textBox1.Text = regStr;
            if (regStr.Length == 36)
            {
                string text = ReadText();
                ulong reg = ConvertRegString(regStr);
                Alg(reg);
            }
            else
            {
                MessageBox.Show("Начальное состояние регистра некорректно", "",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private ulong ConvertRegString(string str)
        {
            return Convert.ToUInt64(str, 2);    
        }

        private string ReadText()
        {
            string res = richTextBox1.Text;
            return res;
        }

        private string ReadKey()
        {
            string res = "";

            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (Regex.IsMatch(textBox1.Text[i].ToString(), "^[0-1]+$"))
                {
                    res += textBox1.Text[i].ToString();
                }
            }
            return res;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            //System.IO.File.WriteAllText(filename, richTextBox2.Text);

            SaveData(filename, outText);
            MessageBox.Show("Файл сохранен");
        }
    }
}
