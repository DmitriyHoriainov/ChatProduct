using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace MyNewChatClient
{
    public class Ban
    {
        string name;
        public Ban( string name)
        {            
            this.name = name;
        }
        public void ForeverBanHendler(NetworkStream stream, Request request)
        {
            request.modul = "lobby";
            request.command = "ban";
            request.data = name;
            request.time = "permanent";
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(request));
            writer.Flush();
        }
        public void ForTimeBanHendler(int time, NetworkStream stream, Request request)
        {
            request.modul = "lobby";
            request.command = "ban";
            request.data = name;
            request.time = time.ToString();
            StreamWriter writer = new StreamWriter(stream);
            if (time != 0)
            {
                writer.WriteLine(JsonConvert.SerializeObject(request));
                writer.Flush();
            }            
        }
    }
}
