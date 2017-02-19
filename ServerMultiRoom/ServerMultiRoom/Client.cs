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
        }
    

        public string[] GetLogPass()
        {
            string message = sr.ReadLine();
            Request name = JsonConvert.DeserializeObject<Request>(message);
            StreamWriter sw = new StreamWriter(client.GetStream());

            string[] logpass = name.data.Split(',');
            this.name = logpass[0];
            return logpass;
        }

        public string Read()
        {
            return sr.ReadLine();
        }
    }

}
