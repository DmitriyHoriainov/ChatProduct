using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MyNewChatClient
{
    public class CreateRoom
    {
        

        public CreateRoom()
        {
          
        }

        public void CreateHendler(string name,NetworkStream stream, Request request )
        {
            request.modul = "rooms";
            request.command = "createroom";
            request.data = name;
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
        }

       
    }
}
