using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace ServerMultiRoom
{
    public class Room
    {
        public List<Client> activeList;
        public Dictionary<Client, int> pasive;
        public string name { get; set; }
        public  bool privateroom = false;
        public Room(string name)
        {
            this.name = name;
            activeList = new List<Client>();
            pasive = new Dictionary<Client, int>();
        }
        public void AddPassive(Client client)
        {
            pasive.Add(client, 0);
        }
        public void CreatePrivate(Client namecreator,Client nameinvited)
        {
            try
            {
                activeList.Add(namecreator);
                pasive.Add(nameinvited, 0);
                Request req = new Request("enter", namecreator.name + "+" + nameinvited.name, "nomissed");
                StreamWriter sq = new StreamWriter(namecreator.netStream);
                sq.WriteLine(JsonConvert.SerializeObject(req));
                sq.Flush();
            }
            catch(Exception ex)
            {
                Request req = new Request("wrongprivateroom", null, null);
                StreamWriter sq = new StreamWriter(namecreator.netStream);
                sq.WriteLine(JsonConvert.SerializeObject(req));
                sq.Flush();
            }
            
        }
        public string Add(Client client, string room)
        {
            Request req = null;
            if (!pasive.ContainsKey(client))
            {
                req = new Request("enter", room, "nomissed");
                if (!activeList.Contains(client))
                {
                    activeList.Add(client);
                    
                    return JsonConvert.SerializeObject(req);
                }
                return JsonConvert.SerializeObject(req);
            }
            else
            {
                int count = pasive[client];
                if (File.Exists(("logs/" + name + ".txt")))
                {
                    string[] str = File.ReadAllLines(("logs/" + name + ".txt"));
                    string response = "";
                    for (int i = count; i < str.Length; i++)
                    {
                        response += str[i] + ".";
                    }
                    response = response.TrimEnd('.');
                    pasive.Remove(client);
                    activeList.Add(client);
                    req = new Request("enter", room, "missed,"+ response);
                    return JsonConvert.SerializeObject(req);
                }
                else
                {
                    pasive.Remove(client);
                    activeList.Add(client);
                    req = new Request("enter", room, "nomissed");
                    return JsonConvert.SerializeObject(req);
                }
            }
        }

        public void Leave(Client client)
        {
            activeList.Remove(client);
            if (File.Exists(("logs/" + name + ".txt")))
            {
                string[] str = File.ReadAllLines(("logs/" + name + ".txt"));
                pasive.Add(client, str.Length);
            }
            else
            {
                pasive.Add(client, 0);
            }
        }


        public void BroadCast(string name, string message)
        {
            Log(name + ":" + message);
            for (int i = 0; i < activeList.Count(); i++)
            {
                Request req = new Request("message",null, name + ":" + message);

                StreamWriter writer = new StreamWriter(activeList[i].netStream);
                writer.WriteLine(JsonConvert.SerializeObject(req));
                writer.Flush();
            }
            SendForPassiv();
        }
        public void SendForPassiv()
        {
            if (File.Exists(("logs/" + name + ".txt")))
            {
                string[] str = File.ReadAllLines("logs/"+name + ".txt");
                for (int i = 0; i < pasive.Count; i++)
                {
                    int a = pasive[pasive.Keys.ElementAt(i)];
                    Request req = new Request("pofig",name,(str.Length - a).ToString());

                    StreamWriter writer = new StreamWriter(pasive.Keys.ElementAt(i).netStream);
                    writer.WriteLine(JsonConvert.SerializeObject(req));
                    writer.Flush();
                }
            }
        }

        public void Exit(Client client)
        {
            activeList.Remove(client);
            pasive.Remove(client);
        }
        public void Log(string message)
        {
            File.AppendAllText(("logs/" + name + ".txt"), message + "\r\n");
        }
        public bool IsPassive(Client client)
        {
            return pasive.ContainsKey(client);
        }
        public bool IsHere(Client client)
        {
            bool flag=false;
            if ( pasive.Keys.Contains(client))
                flag = true;
                
            return flag;
        }

    }
}
