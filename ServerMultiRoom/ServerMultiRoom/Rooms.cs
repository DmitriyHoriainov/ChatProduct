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
                    CaseCreateRoom(index, req.data);
                    break;
                case "leave":
                    CaseLeave(index, req.data);
                    break;
                case "exit":
                    CaseExit(index, req.data);
                    break;
                case "enter":
                    CaseEnter(index, req.data);
                    break;
                case "message":
                    CaseMessage(index, req.data, req.time);
                    break;
                case "privateroom":
                    CasePrivateRoom(index, req.data);
                    break;
            }
        }

        private void CaseCreateRoom(int index, string dat)
        {
            if (IsBanned(srv.clientsList.ElementAt(index)))
            {
                Request req2 = new Request("bancreate", null, BanTime(srv.clientsList.ElementAt(index)));
                string responce2 = JsonConvert.SerializeObject(req2);

                StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                writer.WriteLine(responce2);
                writer.Flush();
                return;
            }
            if (File.Exists("logs/" + dat + ".txt"))
            {
                Request req2 = new Request("wrongroom", null, dat);
                string responce2 = JsonConvert.SerializeObject(req2);

                StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                writer.WriteLine(responce2);
                writer.Flush();
                return;
            }
            FileStream fs = File.Create("logs/" + dat + ".txt");
            fs.Close();
            roomList.Add(new Room(dat));
            if (srv.clientsList.Find(c => c.name == "admin") != null)
            {
                roomList.Find(c => c.name == dat).AddPassive(srv.clientsList.Find(c => c.name == "admin"));
            }
            Thread.Sleep(100);
            srv.SetRoom(index);
        }

        private void CaseLeave(int index, string dat)
        {
            roomList.Find(c => c.name == dat).Leave(srv.clientsList.ElementAt(index));

            Request request = new Request("leave", null, null);
            string responce = JsonConvert.SerializeObject(request);

            StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
            writer.WriteLine(responce);
            writer.Flush();
            Thread.Sleep(100);
            srv.SetRoom(index);
        }

        private void CaseExit(int index, string dat)
        {
            roomList.Find(c => c.name == dat).Exit(srv.clientsList.ElementAt(index));

            Request request1 = new Request("leave", null, null);
            string responce1 = JsonConvert.SerializeObject(request1);

            StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
            writer.WriteLine(responce1);
            writer.Flush();
        }

        private void CaseEnter(int index, string dat)
        {
            StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
            writer.WriteLine(roomList.Find(c => c.name == dat).Add(srv.clientsList.ElementAt(index), dat));
            writer.Flush();
            Thread.Sleep(100);
            srv.SetRoom(index);
        }

        private void CaseMessage(int index, string dat, string tm)
        {
            if (!IsBanned(srv.clientsList.ElementAt(index)) || roomList.Find(c => c.name == tm).privateroom)
                roomList.Find(c => c.name == tm).BroadCast(srv.clientsList.ElementAt(index).name, dat);
            else if (IsBanned(srv.clientsList.ElementAt(index)))
            {
                Request req1 = new Request("youbanned", null, "you are banned to " + BanTime(srv.clientsList.ElementAt(index)));
                string resp = JsonConvert.SerializeObject(req1);

                StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                writer.WriteLine(resp);
                writer.Flush();
            }
        }

        private void CasePrivateRoom(int index, string dat)
        {
            if (!File.Exists("logs/" + srv.clientsList.ElementAt(index).name + "+" + dat + ".txt"))
            {
                roomList.Add(new Room(srv.clientsList.ElementAt(index).name + "+" + dat));
                roomList.Last().privateroom = true;
                roomList.Find(c => c.name == srv.clientsList.ElementAt(index).name + "+" + dat).CreatePrivate(srv.clientsList.ElementAt(index), srv.clientsList.Find(c => c.name == dat));
            }
            else
            {
                StreamWriter writer = new StreamWriter(srv.clientsList.ElementAt(index).netStream);
                writer.WriteLine(roomList.Find(c => c.name == srv.clientsList.ElementAt(index).name + "+" + dat).Add(srv.clientsList.ElementAt(index), srv.clientsList.ElementAt(index).name + "+" + dat));
                writer.Flush();
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
