using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerMultiRoom
{
    public class Rooms
    {
        public List<Room> roomList;
        Server srv;
        public Rooms(Server srv)
        {
            roomList = new List<Room>();
            this.srv = srv;
        }
        public void SetCommand(Request req, int index)
        {
            StreamWriter writer;
            switch (req.command)
            {
                case "createroom":
                    if (IsBanned(srv.clientsList.ElementAt(index)))
                    {
                        Request req2 = new Request("bancreate", null, BanTime(srv.clientsList.ElementAt(index)));
                        string responce2 = JsonConvert.SerializeObject(req2);

                        writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                        writer.WriteLine(responce2);
                        writer.Flush();
                        break;
                    }
                    if (File.Exists("logs/" + req.data + ".txt"))
                    {
                        Request req2 = new Request("wrongroom", null, req.data);
                        string responce2 = JsonConvert.SerializeObject(req2);

                        writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                        writer.WriteLine(responce2);
                        writer.Flush();
                        break;
                    }
                    FileStream fs = File.Create("logs/" + req.data + ".txt");
                    fs.Close();
                    roomList.Add(new Room(req.data));
                    if (srv.clientsList.Find(c => c.name == "admin") != null)
                    {
                        roomList.Find(c => c.name == req.data).AddPassive(srv.clientsList.Find(c => c.name == "admin"));
                    }
                    Thread.Sleep(100);
                    srv.SetRoom(srv.clientsList, srv.rooms, index);
                    break;

                case "leave":
                    roomList.Find(c => c.name == req.data).Leave(srv.clientsList.ElementAt(index));

                    Request request = new Request("leave", null, null);
                    string responce = JsonConvert.SerializeObject(request);

                    writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                    writer.WriteLine(responce);
                    writer.Flush();
                    break;
                case "exit":
                    roomList.Find(c => c.name == req.data).Exit(srv.clientsList.ElementAt(index));

                    Request request1 = new Request("leave", null, null);
                    string responce1 = JsonConvert.SerializeObject(request1);

                    writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                    writer.WriteLine(responce1);
                    writer.Flush();
                    break;
                case "enter":
                    writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                    writer.WriteLine(roomList.Find(c => c.name == req.data).Add(srv.clientsList.ElementAt(index), req.data));
                    writer.Flush();
                    Thread.Sleep(100);
                    srv.SetRoom(srv.clientsList, srv.rooms, index);
                    break;
                case "message":
                    if (!IsBanned(srv.clientsList.ElementAt(index)) || roomList.Find(c => c.name == req.time).privateroom)
                        roomList.Find(c => c.name == req.time).BroadCast(srv.clientsList.ElementAt(index).name, req.data);
                    if (IsBanned(srv.clientsList.ElementAt(index)))
                    {
                        Request req1 = new Request("youbanned", null, "you are banned to " + BanTime(srv.clientsList.ElementAt(index)));
                        string resp = JsonConvert.SerializeObject(req1);

                        writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                        writer.WriteLine(resp);
                        writer.Flush();
                    }
                    break;
                case "privateroom":
                    if (!File.Exists("logs/" + srv.clientsList.ElementAt(index).name + "+" + req.data + ".txt"))
                    {
                        roomList.Add(new Room(srv.clientsList.ElementAt(index).name + "+" + req.data));
                        roomList.Last().privateroom = true;
                        roomList.Find(c => c.name == srv.clientsList.ElementAt(index).name + "+" + req.data).CreatePrivate(srv.clientsList.ElementAt(index), srv.clientsList.Find(c => c.name == req.data));
                    }
                    else
                    {
                        writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                        writer.WriteLine(roomList.Find(c => c.name == srv.clientsList.ElementAt(index).name + "+" + req.data).Add(srv.clientsList.ElementAt(index), srv.clientsList.ElementAt(index).name + "+" + req.data));
                        writer.Flush();
                    }
                    break;
            }
        }
        public bool IsBanned(Client client)
        {
            bool flag = false;

            if (File.Exists("banlist.txt"))
            {
                string[] str = File.ReadAllLines("banlist.txt");
                List<string> strnew = new List<string>();
                for (int i = 0; i < str.Length; i++)
                {
                    string[] splitstr = str[i].Split('-');
                    if ((splitstr[1] == "permanent") || !(DateTime.Now > Convert.ToDateTime(splitstr[1])))
                        strnew.Add(str[i]);

                }
                for (int i = 0; i < strnew.Count; i++)
                {
                    string[] splitstr = strnew.ElementAt(i).Split('-');
                    if (client.name == splitstr[0])
                    {
                        flag = true;
                        return flag;
                    }
                }
                FileStream fs = File.Create("banlist.txt");
                fs.Close();
                for (int i = 0; i < strnew.Count; i++)
                {
                    File.AppendAllText("banlist.txt", strnew.ElementAt(i) + "\r\n");
                }
            }
            return flag;
        }
        public string BanTime(Client client)
        {
            string time = "";
            if (File.Exists("banlist.txt"))
            {

                string[] str = File.ReadAllLines("banlist.txt");
                List<string> strnew = new List<string>();
                for (int i = 0; i < str.Length; i++)
                {
                    string[] splitstr = str[i].Split('-');
                    if ((splitstr[1] == "permanent") || !(DateTime.Now > Convert.ToDateTime(splitstr[1])))
                        strnew.Add(str[i]);

                }
                for (int i = 0; i < strnew.Count; i++)
                {
                    string[] splitstr = strnew.ElementAt(i).Split('-');
                    if (client.name == splitstr[0])
                    {
                        time = splitstr[1];
                        return time;
                    }
                }

            }

            return time;
        }
    }
}
