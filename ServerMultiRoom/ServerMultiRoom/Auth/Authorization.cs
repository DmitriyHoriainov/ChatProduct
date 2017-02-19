using System;
using System.Collections.Generic;
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
        public Authorization(Server server)
        {
            this.server = server;
        }
        public void AddUser(TcpClient tcpclient, List<Client> clientsList, Rooms rooms)
        {
            Client client = new Client(tcpclient);
            clientsList.Add(client);
            if (client.name == "admin")
            {
                Thread tr = new Thread(delegate () { ForAdmin(client, rooms); });
                tr.Start();
            }
        }

        private void ForAdmin(Client client, Rooms rooms)
        {
            foreach (var room in rooms.roomList)
            {
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
