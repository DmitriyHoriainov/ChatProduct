using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace ServerMultiRoom
{
    public class Client
    {
        public string name { get; set; }
        public TcpClient client;
        public NetworkStream netStream;
        StreamReader sr;
        public Client(TcpClient client)
        {
            this.client = client;
            netStream = client.GetStream();
            sr = new StreamReader(client.GetStream());
            GetName();           
        }

        public void GetName()
        {
            name = sr.ReadLine();
            StreamWriter sw = new StreamWriter(client.GetStream());
            Request req;
            if (name == "admin")
            {
                req = new Request("ok", null, "admin");
            }
            else
            {
                req = new Request("ok", null, null);
            }
            sw.WriteLine(JsonConvert.SerializeObject(req));
            sw.Flush();

        }

        public string Read()
        {
            return sr.ReadLine();
        }
    }

}
