using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace GUIApp
{
    public class NamedPipes
    {

        private NamedPipeServerStream _server;
        private Form1 MainWindow;
        public bool isConnected = false;
        public bool isConnecting = false;

        public NamedPipes(Form1 sender)
        {
            this._server = new NamedPipeServerStream(@"languageConnectionPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message);
            MainWindow = sender;
        }

        public void WaitForConnection()
        {
            isConnecting = true;
            _server.WaitForConnection();
            isConnected = true;
            isConnecting = true;
            Thread.CurrentThread.Join();
        }

        public bool sendInstruction(string instruction)
        {
            
            byte[] buff = Encoding.UTF8.GetBytes(instruction + ";[END]");

            try 
            {
                _server.Write(buff, 0, buff.Length);

                return true;

            }
            catch
            {
                if(isConnected)
                {
                    _server.Disconnect();
                }
                isConnected = false;
                return false;
            }
        }

        public string receiveResponse()
        {
            byte[] buff = new byte[1024];

            try
            {
                _server.Read(buff, 0, buff.Length);
                return Encoding.UTF8.GetString(buff);

            }catch
            {
                return "";
            }
        }

    }
}