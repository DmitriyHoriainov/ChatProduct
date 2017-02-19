using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerMultiRoom
{
    class Authorization
    {
        Server server;
        DataBaseManager dbmanager;
        public Authorization(Server server)
        {
            this.server = server;
            dbmanager = new DataBaseManager();
        }
        public void AddUser(TcpClient tcpclient, List<Client> clientsList, Rooms rooms)
        {
            Client client = new Client(tcpclient);
            string[] logpass = client.GetLogPass();


            if (dbmanager.CreateNewLogin(logpass[0], logpass[1], logpass[2]))
            {
                for (int i = 0; i < clientsList.Count; i++)
                {
                    if (clientsList[i].name == logpass[0])
                    {
                        return;
                    }
                }
                clientsList.Add(client);

                if (client.name == "admin")
                {
                    Thread tr = new Thread(delegate () { ForAdmin(client, rooms); });
                    tr.Start();
                }
                StreamWriter sw = new StreamWriter(tcpclient.GetStream());
                Request req;

                if (logpass[0] == "admin")
                {
                    req = new Request("ok", null, "admin");
                }
                else
                {
                    req = new Request("ok", null, logpass[0]);
                }
                sw.WriteLine(JsonConvert.SerializeObject(req));
                sw.Flush();
            }
        }

        private void ForAdmin(Client client, Rooms rooms)
        {
            foreach (var room in rooms.roomList)
            {
                if(room.privateroom != true)
                    room.AddPassive(client);
            }
        }
        public void SetCommand(Request req, int index)
        {
            switch (req.command)
            {
                case "exit":
                    for (int j = 0; j < server.rooms.roomList.Count; j++)
                    {
                        server.rooms.roomList[j].Exit(server.clientsList.ElementAt(index));
                    }
                    server.clientsList.Remove(server.clientsList.ElementAt(index));
                    break;
            }
        }
    }
}
