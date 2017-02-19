using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Drawing;

namespace MyNewChatClient
{
    class Listener
    {
        public delegate void InvokeDelegate();
        TcpClient client;
        RoomDialog rd;
        MainForm form1;
        public Listener(TcpClient client,  MainForm form1)
        {
            this.client = client;
            Thread listenthread = new Thread(new ThreadStart(Listen));
            listenthread.Start();
            this.form1 = form1;   
        }
        private void OpenForm()
        {
            try
            {
                rd.ShowDialog();
            }
            catch(Exception ex)
            {

            }
            
        }

        public void Listen()
        {
            while(true)
            {
                try
                {

                    StreamReader reader = new StreamReader(client.GetStream());
                    string message = reader.ReadLine();
                    Request req = JsonConvert.DeserializeObject<Request>(message);
                    switch (req.modul)
                    {
                        case "ok":
                            CaseOk(req.data);
                            break;
                        case "enter":
                            CaseEnter(req.command, req.data, req.time);
                            break;
                        case "leave":
                            rd.Close();
                            break;
                        case "bancreate":
                            MessageBox.Show("You have been banned for " + req.data);
                            break;
                        case "refresh":
                            CaseRefresh(req.data);
                            break;
                        case "refreshclients":
                            CaseRefreshClients(req.data);
                            break;
                        case "message":
                            rd.rtb_message.Text += req.data + "\n";
                            break;
                        case "youbanned":
                            rd.rtb_message.Text += req.data + "\n";
                            break;
                        case "wrongroom":
                            MessageBox.Show("Error, room " + req.data + " has already been created.");
                            break;
                        case "alreadyenter":
                            MessageBox.Show("You are already logged into this room.");
                            break;
                        case "passive":
                            CasePassive(req.command, req.data, req.time);
                            break;
                    }
                }
                catch (Exception ex)
                {                   
                    return;
                }
            }
        } 
        
        private void CaseOk(string dat)
        {
            string[] data = dat.Split(',');
            form1.lb_name.Text = data[0];
            Thread.Sleep(150);
            if (data[0] == "admin")
            {
                form1.btn_ban.BeginInvoke(new InvokeDelegate(form1.VisibleBan));
                form1.btn_unban.BeginInvoke(new InvokeDelegate(form1.VisibeUnban));
            }
        }     

        private void CaseEnter(string cmd, string dat, string tm)
        {
            rd = new RoomDialog(client, form1.request);

            rd.room.name = cmd;
            if (rd.room.name.Contains("+"))
                rd.btn_exit.Enabled = false;
            else
                rd.btn_exit.Enabled = true;
            rd.Text = cmd;
            string[] str = dat.Split(',');
            if (str[0] == "missed")
            {
                string[] tmp = str[1].Split('.');
                for (int i = 0; i < tmp.Length; i++)
                {
                    rd.rtb_message.Text += tmp[i] + "\n";
                }
            }
            Thread tr = new Thread(new ThreadStart(OpenForm));
            tr.Start();
            Thread.Sleep(100);
            if (str.Length > 1)
            {
                string[] tmp = rd.rtb_message.Text.Split('\n');
                for (int i = Convert.ToInt32(tm); i < tmp.Length; i++)
                {
                    string str11 = tmp[i];
                    int j = 0;
                    while (j < rd.rtb_message.Text.Length - str11.Length)
                    {
                        j = rd.rtb_message.Text.IndexOf(str11, j);
                        if (j <= 0) break;
                        rd.rtb_message.SelectionStart = j;
                        rd.rtb_message.SelectionLength = str11.Length;
                        rd.rtb_message.SelectionColor = Color.Red;
                        j += str11.Length;
                    }
                }
            }
        }

        private void CaseRefresh(string dat)
        {
            string[] splitrooms = dat.Split('.');
            if (splitrooms[0] == "")
            {
                return;
            }
            form1.lst_rooms.Items.Clear();
            for (int i = 0; i < splitrooms.Length; i++)
            {
                if (!(splitrooms[i].Contains("+")))
                    form1.lst_rooms.Items.Add(splitrooms[i]);
            }
        }

        private void CaseRefreshClients(string dat)
        {
            string[] splitclients = dat.Split('.');
            form1.lst_clients.Items.Clear();
            if (splitclients[0] != "")
            {
                form1.lst_clients.Items.AddRange(splitclients);
            }
        }

        private void CasePassive(string cmd, string dat, string tm)
        {
            int a = -1;
            for (int i = 0; i < form1.lst_rooms.Items.Count; i++)
            {
                string[] str1 = form1.lst_rooms.Items[i].ToString().Split(' ');
                if (str1[0] == cmd)
                    a = i;
            }
            if (a != -1)
            {
                form1.lst_rooms.Items.RemoveAt(a);
                if (dat == "0" && tm != "True")
                    form1.lst_rooms.Items.Insert(a, cmd + "  ");
                else if (dat != "0")
                    form1.lst_rooms.Items.Insert(a, cmd + "   +" + dat);
            }
            else
                if (dat != "0")
                form1.lst_rooms.Items.Insert(form1.lst_rooms.Items.Count, cmd + "   +" + dat);
        }
    }
}
