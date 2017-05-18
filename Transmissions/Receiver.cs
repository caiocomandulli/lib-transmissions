using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Transmissions
{
    public class Receiver
    {

        private Thread thread;
        private TcpListener tcpListener;
        private int port;
        private bool active;
        private ITransmissionListener listener;
        private byte[] buffer;
        private bool throwError = false;

        public Receiver(int port)
        {
            this.port = port;
            thread = new Thread(new ThreadStart(Service));
        }

        public Receiver StartService(ITransmissionListener listener)
        {
            return StartService(listener, false);
        }

        public Receiver StartService(ITransmissionListener listener, bool throwError)
        {
            this.throwError = throwError;
            this.listener = listener;
            active = true;
            buffer = new byte[listener.InstantiateTransmissible().GetByteLength()];
            thread.Start();
            return this;
        }

        public void HaltService()
        {
            active = false;
        }

        private void Service()
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            while (active)
            {
                Socket socket = tcpListener.AcceptSocket();

                ITransmissible received = listener.InstantiateTransmissible();

                if (socket.Receive(buffer) != received.GetByteLength())
                {
                    if (throwError)
                    {
                        throw new Exception("Wrong Length Received");
                    }
                    return;
                }

                received.Deserialize(buffer);
                listener.OnReceive(received);
            }

            tcpListener.Stop();
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

    }
}
