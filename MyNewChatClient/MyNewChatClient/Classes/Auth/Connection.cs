using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNewChatClient
{
    public class Connection
    {
        public TcpClient client;

        public Connection()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 8888);
        }

        public void LogInHendler(object sender, EventArgs e)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(sender as string);
            writer.Flush();
        }
        public void LogoutHendler(object sender, EventArgs e)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine("exit,");
            writer.Flush();
            Environment.Exit(0);
        }
    }
}
