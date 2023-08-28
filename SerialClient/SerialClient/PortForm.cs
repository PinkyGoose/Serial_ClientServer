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
using System.Collections;

namespace SerialClient
{

    public partial class PortForm : Form
    {
        StopBits getStop(int k)
        {
            k++;
            if (k == 0)
            {
                return StopBits.None;
            }
            if (k == 1)
            {
                return StopBits.One;
            }
            if ((k == 2))
            {
                return StopBits.Two;
            }
            if (k == 3)
            {
                return StopBits.OnePointFive;
            }
            throw new Exception("No stop");
            return StopBits.None;
        }
        Parity getParity(int k)
        {
            if (k == 0)
            {
                return Parity.None;
            }
            if (k == 1)
            {
                return Parity.Odd;
            }
            if ((k == 2))
            {
                return (Parity.Even);
            }
            if (k == 3)
            {
                return Parity.Mark;
            }
            if (k == 4){
                return Parity.Space;
            }
            throw new Exception("No parity");
            return Parity.None;
        }
        SerialPort port;
        public SerialPort getSerialPort()
        {
            return port;
        }
        public PortForm()
        {
            InitializeComponent();
            try
            {
                setComboValues();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString());Close(); }
            comboBoxPort.SelectedIndex = 0;
            comboBoxData.SelectedIndex = 0;
            comboBoxStop.SelectedIndex = 0;
            comboParity.SelectedIndex = 0;
            textBoxBaud.Text = "9600";
            
        }
        public PortForm(SerialPort port)
        {
            InitializeComponent();
            try
            {
                setComboValues();
                comboBoxData.SelectedIndex=comboBoxData.Items.IndexOf(port.DataBits);
                comboBoxPort.SelectedItem = port.PortName;
                comboBoxStop.SelectedItem = port.StopBits;
                comboParity.SelectedItem = port.Parity;
                textBoxBaud.Text = port.BaudRate.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); Close(); }
        }
        private void setComboValues()
        {

            comboBoxPort.Items.AddRange( SerialPort.GetPortNames());
            if(comboBoxPort.Items.Count <= 0)
            {
                throw new Exception("No Ports in System");
                
            }
            for(int i = 0; i < 5; i++)
            {
                comboParity.Items.Add(getParity(i));
            }

            for (int i = 0; i < 3; i++)
            {
                comboBoxStop.Items.Add(getStop(i));
            }
            comboBoxData.Items.Add(7);
            comboBoxData.Items.Add (8);
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                


                port = new SerialPort(comboBoxPort.Text, Int32.Parse(textBoxBaud.Text),
                    getParity(comboParity.SelectedIndex), Int32.Parse(comboBoxData.Text),getStop(comboBoxStop.SelectedIndex));
                port.DtrEnable = true;
                port.RtsEnable = true;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            Close();
        }

        private void PortForm_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
