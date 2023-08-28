using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;

namespace SerialClient
{
    public partial class Form1 : Form
    {
        byte[] strDataToSend;
        SerialPort serialPort;
        delegate void labelTextDelegate(string text);
        public Form1()
        {
            InitializeComponent();
            //timer1 = new Timer();
            timer1.Interval = 1000;

        }
        public void toTB(string strDataToSend)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new labelTextDelegate(toTB), new object[] { strDataToSend });
                return;
            }
            else
            {
            
            textBoxAnswer.Text = strDataToSend;
        }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            PortForm portForm = new PortForm(serialPort);
            if (portForm.ShowDialog() == DialogResult.OK)
            {
                serialPort = portForm.getSerialPort();
                buttonStart.Enabled = true;
            }
            serialPort.DataReceived += SerialPort_DataReceived;

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            byte[] bytes = new byte[serialPort.BytesToRead];
            serialPort.Read(bytes, 0, serialPort.BytesToRead);
            toTB(System.Text.Encoding.UTF8.GetString(bytes));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                serialPort = PortConfiguration.CreatePort();
                if (serialPort == null )
                {
                    throw new Exception("settings is empty");
                }
            }

            catch {
                PortForm portForm = new PortForm();
                if (portForm.ShowDialog() == DialogResult.OK)
                {
                    serialPort = portForm.getSerialPort();
                }
                else
                {
                    MessageBox.Show("Next time set a port");
                    Close();
                }

            }
            serialPort.DataReceived += SerialPort_DataReceived;
            buttonStart.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PortConfiguration.SavePort(serialPort);
        }

        private void buttonStop_Clicked(object sender, EventArgs e)
        {
            try
            {
                //strDataToSend = formDataToSend(textBoxMessage.Text, Convert.ToInt32(numericUpDownStart.Value), Convert.ToInt32(numericUpDownStop.Value));

                timer1.Stop();
                serialPort.Close();
                buttonSettings.Enabled = true;
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Тут-то что могло пойти не так?!");
            }
        }

        private void buttonStart_Clicked(object sender, EventArgs e)
        {
            try
            {
                //strDataToSend = formDataToSend(textBoxMessage.Text, Convert.ToInt32(numericUpDownStart.Value), Convert.ToInt32(numericUpDownStop.Value));
                serialPort.Open();
                timer1.Start();
                buttonSettings.Enabled = false;
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
            } catch
            {
                MessageBox.Show("Seems like Serial Port is wrong or busy");
            }
        }

        private byte[] formDataToSend(string str, int v1, int v2)
        {
            Random rnd = new Random();
            int counter = 0;
            //throw new NotImplementedException();
            byte[] data= Encoding.UTF8.GetBytes(str);
            int length = data.Length;
            byte[] bytes= new byte[length+v1+v2+2];
            for(int i = 0; i < v1; i++)
            {
                bytes[i] = getRandByte(rnd);
                counter = i+1;
            }
            bytes[counter] = 0xA;//Добавить тот самый байт
            counter += 1;
            data.CopyTo(bytes, counter);
            counter += length;
            bytes[counter] = 0xB;//Добавить тот самый байт
            counter += 1;
            for (int i = counter; i < counter+v2; i++)
            {
                bytes[i] = getRandByte(rnd);

            }

            return bytes;


        }
        byte getRandByte(Random rnd)
        {
            byte res = 0xA;
            while (res == 0xA || res == 0xB)
            {
                res = (byte)(rnd.Next(255));
            }
            return res;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] cmd = formDataToSend(textBoxMessage.Text, Convert.ToInt32(numericUpDownStart.Value), Convert.ToInt32(numericUpDownStop.Value));
            serialPort.Write(cmd,0,cmd.Length);
        }
    }
}

