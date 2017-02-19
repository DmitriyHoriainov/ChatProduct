using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMultiRoom
{
    class AdminFunc
    {
        public void BanUser(string user, string time)
        {
            if (time == "permanent")
            {
                File.AppendAllText("banlist.txt", user + "-" + time + "\r\n");
            }
            else
            {
                DateTime date = DateTime.Now;
                date = date.AddSeconds(Convert.ToInt32(time));
                File.AppendAllText("banlist.txt", user + "-" + date.ToString() + "\r\n");
            }
        }

        public void Unban(string user)
        {
            if (File.Exists("banlist.txt"))
            {
                string[] str = File.ReadAllLines("banlist.txt");
                List<string> strnew = new List<string>();
                for (int i = 0; i < str.Length; i++)
                {
                    string[] splitstr = str[i].Split('-');
                    if (!(user==splitstr[0]))
                        strnew.Add(str[i]);

                }
                FileStream fs = File.Create("banlist.txt");
                fs.Close();
                for (int i = 0; i < strnew.Count; i++)
                {
                    File.AppendAllText("banlist.txt", strnew.ElementAt(i) + "\r\n");

                }
            }
        }     
    }
}
