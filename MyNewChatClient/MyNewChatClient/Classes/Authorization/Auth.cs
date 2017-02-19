using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNewChatClient
{
    class Auth
    {
        public void LogInHendler(TcpClient client, string name, string password, Request request)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            request.modul = "auth";
            request.data = name + "," + password + ",auth";
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
            Thread.Sleep(400);
        }
        public void LogoutHendler(TcpClient client, Request request)
        {
            request.modul = "auth";
            request.command = "exit";
            request.data = "";
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
        }

        public void RegistationHendler(TcpClient client, string name, string password, Request request)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            request.modul = "auth";
            request.data = name + "," + password + ",reg";
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
            Thread.Sleep(400);
        }
    }
}
