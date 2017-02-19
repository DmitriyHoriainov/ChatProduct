
namespace MyNewChatClient
{
    public class Request
    {
        public string modul;
        public string command;
        public string data;
        public string time;
        public Request()
        {
            
        }
        public Request(string modul, string command, string data)
        {
            this.modul = modul;
            this.command = command;
            this.data = data;
        }
        public Request(string modul, string command, string data, string data2)
        {
            this.modul = modul;
            this.command = command;
            this.data = data;
            time = data2;
        }
    }
}
