using System;
using System.IO;
using System.Net.Sockets;

namespace Transmissions
{
    public class Transmitter
    {

        private string ip;
        private int port;
        private TcpClient client;

        public Transmitter(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.client = new TcpClient();
        }

        public void Send(ITransmissible transmissible)
        {
            try
            {
                client.Connect(ip, port);
                Stream stream = client.GetStream();
                transmissible.Serialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            client.Close();
        }

    }
}
