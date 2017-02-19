using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNewChatClient
{
    class RefreshListBoxes
    {
       

        public RefreshListBoxes()
        {
           
        }

        public void RefreshHendler(NetworkStream stream, string type, Request request)
        {
            StreamWriter writer = new StreamWriter(stream);
            
            if (type == "Rooms")
            {
                request.modul = "lobby";
                request.command = "refresh";
                request.data = "";
                writer.WriteLine(JsonConvert.SerializeObject(request));
            }
            else
            {
                request.modul = "lobby";
                request.command = "refreshclients";
                request.data = "";
                writer.WriteLine(JsonConvert.SerializeObject(request));
            }
            writer.Flush();
        }
    }
}
