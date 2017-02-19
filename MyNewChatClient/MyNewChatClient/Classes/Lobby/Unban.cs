using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNewChatClient
{
    class Unban
    {
       
        string name;
        public Unban( string name)
        {
           
            this.name = name;
           
        }
        public void UnbanHandler(NetworkStream stream, Request request)
        {
            request.modul = "lobby";
            request.command = "unban";
            request.data = name;
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
        }
    }
}
