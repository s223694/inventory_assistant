using System;
using System.Net.Sockets;
using System.Text;

namespace inventory_assistant.Models
{
    public class Robot
    {
        public const int UrScriptPort = 30002, DashboardPort = 29999;
        public string IpAddress = "127.0.0.1";

        public void SendString(int port, string message)
        {
            using var client = new TcpClient();
            client.Connect(IpAddress, port);
            using var stream = client.GetStream();
            var bytes = Encoding.ASCII.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
            Console.WriteLine($"[Sent] {bytes.Length} bytes to {IpAddress}:{port} — {message.Split('\n')[0]}");
        }

        public void SendUrScript(string urscript)
        {
            SendString(DashboardPort, "brake release\n");
            SendString(UrScriptPort, urscript);
        }
    }
}
