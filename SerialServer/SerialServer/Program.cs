using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SerialServer
{
    internal class Program
    {
        static SerialPort serialPort;
        
        static void Main(string[] args)
        {
            serialPort = new SerialPort();
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.DataReceived += SerialPort_DataReceived;
            while (true)
            {
                configure();
                writeConfiguration();
                serialPort.Open();
                Console.WriteLine("If you want to configure tap Enter");
                Console.ReadLine();
                serialPort.Close();
            }
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            byte[] bytes = new byte[serialPort.BytesToRead];
            serialPort.Read(bytes, 0, serialPort.BytesToRead);

            int k = 0;
            int u = -1;
            for (k = 0; k < bytes.Length; k++)
            {
                if (bytes[k] == 0xA)
                {
                    u = k;
                }
                if (bytes[k] == 0xB)
                {
                    break;
                }
            }
            if (k < u || u == -1)
            {
                serialPort.WriteLine("0");
                return;
            }
            byte[] res = new byte[k - u - 1];
            Array.Copy(bytes, u + 1, res, 0, k - u - 1);
            serialPort.WriteLine(res.Length.ToString());
        }

        static void configure()
        {
            try
            {

                string[] spNames = SerialPort.GetPortNames();
                Parity[] parities = (Parity[])Enum.GetValues(typeof(Parity));
                StopBits[] stopBits = (StopBits[])Enum.GetValues(typeof(StopBits));
                Console.WriteLine("Select a SerialPort");
                for (int i = 0; i < spNames.Length; i++)
                {
                    Console.WriteLine(" {0}:  {1}", i, spNames[i]);
                }
                serialPort.PortName = spNames[Int32.Parse(Console.ReadLine())];

                Console.WriteLine("Enter a Baud Rate");
                serialPort.BaudRate = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Select a Parity");
                for (int i = 0; i < parities.Length; i++)
                {
                    Console.WriteLine(" {0}:  {1}", i, parities[i]);
                }
                serialPort.Parity = parities[Int32.Parse(Console.ReadLine())];

                Console.WriteLine("Enter a Data Bits");
                serialPort.DataBits = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Select a Stop Size");
                for (int i = 0; i < stopBits.Length; i++)
                {
                    Console.WriteLine(" {0}:  {1}", i, stopBits[i]);
                }
                serialPort.StopBits = stopBits[Int32.Parse(Console.ReadLine())];
            }catch (Exception e)
            {
                Console.WriteLine("Something wrong. Try Again");
                configure();
            }
        }
        static void writeConfiguration()
        {
            Console.WriteLine("Port has settings:");
            Console.WriteLine(" Name:  {0}", serialPort.PortName);
            Console.WriteLine(" Baud Rate:  {0}", serialPort.BaudRate.ToString());
            Console.WriteLine(" Parity:  {0}", serialPort.Parity.ToString());
            Console.WriteLine(" Data Bits:  {0}", serialPort.DataBits.ToString());
            Console.WriteLine(" Stop Bits:  {0}", serialPort.StopBits.ToString());
        }
    }
}
