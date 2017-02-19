using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMultiRoom
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.DeleteLogs();
            server.Start();
        }
       
    }
}
