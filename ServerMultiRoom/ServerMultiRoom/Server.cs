
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System;

namespace ServerMultiRoom
{
    public class Server
    {
        private int PORT = 8888;
        TcpListener server;
        public List<Client> clientsList;
        public Rooms rooms;
        Authorization auth;
        Lobby lobbys;     

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
            rooms = new Rooms(this);
            auth = new Authorization(this);
            lobbys = new Lobby(this);
            clientsList = new List<Client>();
            server.Start();
            Console.WriteLine("Server started!");
        }
        public void Start()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                auth.AddUser(client, clientsList, rooms);
                Thread tr = new Thread(new ThreadStart(Receive));
                tr.Start();             
            }
        }
        public void Receive()
        {
            while (true)
            {
                try
                {
                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        if (clientsList[i].netStream.DataAvailable)
                        {
                            string message = clientsList[i].Read();
                            Request req = JsonConvert.DeserializeObject<Request>(message);
                            switch (req.modul)
                            {
                                case "rooms":
                                    rooms.SetCommand(req, i);
                                    break;
                                case "lobby":
                                    lobbys.SetCommand(req, i);
                                    break;
                                case "auth":
                                    auth.SetCommand(req, i);
                                    break;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
        public void SetRoom(List<Client> clientsList, Rooms rms, int index)
        {
            string room = "";
            foreach (var item in rms.roomList)
            {
                if (item.privateroom && item.IsHere(clientsList.ElementAt(index)))
                    room += item.name + ".";
                if (!item.privateroom)
                    room += item.name + ".";

            }
            room = room.TrimEnd('.');

            Request req = new Request("refresh", null, room);
            string responce = JsonConvert.SerializeObject(req);

            StreamWriter writer = new StreamWriter(clientsList.ElementAt(index).netStream);
            writer.WriteLine(responce);
            writer.Flush();

            for (int z = 0; z < rooms.roomList.Count; z++)
            {
                if (rooms.roomList[z].IsPassive(clientsList.ElementAt(index)))
                {
                    Thread.Sleep(100);
                    rooms.roomList[z].SendForPassivOne(index);
                }
            }
        }
        public void SetClient(List<Client> clientsList, int index)
        {
            string clients = "";
            foreach (var client in clientsList)
            {
                if (client.name == "admin")
                    continue;
                if (index == clientsList.Count)
                {
                    index = index - 1;
                }
                if(client != clientsList.ElementAt(index))
                    clients += client.name + ".";
            }
            clients = clients.TrimEnd('.');

            Request req = new Request("refreshclients", null, clients);
            string responce = JsonConvert.SerializeObject(req);
            StreamWriter writer = new StreamWriter(clientsList.ElementAt(index).netStream);
            writer.WriteLine(responce);
            writer.Flush();
        }

        public void DeleteLogs()
        {
            DirectoryInfo dirInfo = new DirectoryInfo("logs/");

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
