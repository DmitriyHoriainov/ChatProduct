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
    class PrivateRoom
    {
        public void CreatePrivateHandler(NetworkStream stream,string name, Request request)
        {
            request.modul = "rooms";
            request.command = "privateroom";
            request.data = name;
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
        }
    }
}
