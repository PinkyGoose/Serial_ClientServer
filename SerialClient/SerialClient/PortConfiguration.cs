using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SerialClient
{
    internal class PortConfiguration
    {


        public static SerialPort CreatePort() {

            SerialPort serialPort = new SerialPort();
            serialPort.PortName = PortSettings.Default.PortName;
            serialPort.BaudRate = PortSettings.Default.BaudRate;
            serialPort.DataBits = PortSettings.Default.DataBits;
            serialPort.StopBits = PortSettings.Default.StopBits;
            serialPort.Parity = PortSettings.Default.Parity;

            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            return serialPort;
        }
         public static void SavePort(SerialPort serialPort)
        {
            PortSettings.Default.PortName = serialPort.PortName;
            PortSettings.Default.BaudRate = serialPort.BaudRate;
            PortSettings.Default.DataBits = serialPort.DataBits;
            PortSettings.Default.StopBits = serialPort.StopBits;
            PortSettings.Default.Parity = serialPort.Parity;
            Properties.Settings.Default.Save(); //Saves settings 
            PortSettings.Default.Save ();
            //Properties.Settings

        }

    }

}
