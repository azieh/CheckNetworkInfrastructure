using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceChecker
{
    class TestConnection
    {
        bool _isConnected;
        bool _isNotVisible;
        public Task PingPortAsync(string fqdn, int port)
        {
            return Task.Factory.StartNew(() => PingPort(fqdn, port));
        }

        public void PingPort(string fqdn, int port)
        {
            while (true)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect(fqdn, port);
                        if (!_isConnected)
                            Console.WriteLine($"{fqdn}:{port} open");
                        _isConnected = true;
                        _isNotVisible = false;
                    }
                    catch (Exception)
                    {
                        if (!_isNotVisible)
                            Console.WriteLine($"{fqdn}:{port} closed");
                        _isConnected = false;
                        _isNotVisible = true;
                    }
                }

                Thread.Sleep(5000);
            }

        }
    }
}
