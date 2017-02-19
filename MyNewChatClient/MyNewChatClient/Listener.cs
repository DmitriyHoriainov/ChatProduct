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
                            if (req.data == "admin")
                            {
                                form1.btn_ban.BeginInvoke(new InvokeDelegate(form1.VisibleBan));
                                form1.btn_unban.BeginInvoke(new InvokeDelegate(form1.VisibeUnban));
                            }
                            break;
                        case "enter":
                            rd = new RoomDialog(client, form1.request);
                            rd.room.name = req.command;
                            rd.Text = req.command;
                            string[] str = req.data.Split(',');
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
                                string[] tmp = str[1].Split('.');
                                for (int i = 0; i < tmp.Length; i++)
                                {
                                    string str11 = tmp[i];
                                    int j = 0;
                                    j = rd.rtb_message.Text.IndexOf(str11, j);
                                    if (j < 0) break;
                                    rd.rtb_message.SelectionStart = j;
                                    rd.rtb_message.SelectionLength = str11.Length;
                                    rd.rtb_message.SelectionColor = Color.Red;
                                    j += str11.Length;

                                }
                            }
                            break;
                        case "leave":
                            rd.Close();
                            break;
                        case "bancreate":
                            MessageBox.Show("You have been banned for " + req.data);
                            break;
                        case "refresh":
                            string[] splitrooms = req.data.Split('.');
                            if (splitrooms[0] == "")
                            {
                                break;
                            }
                            form1.lst_rooms.Items.Clear();
                            form1.lst_rooms.Items.AddRange(splitrooms);
                            break;
                        case "refreshclients":
                            string[] splitclients = req.data.Split('.');
                            form1.lst_clients.Items.Clear();
                            if (splitclients[0] != "")
                            {
                                form1.lst_clients.Items.AddRange(splitclients);
                            }
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
                        case "wrongprivateroom":
                            MessageBox.Show("Error, privateroom " + req.data + " has already been created.");
                            break;
                        case "pofig":
                            int a = -1;
                            for (int i = 0; i < form1.lst_rooms.Items.Count; i++)
                            {
                                string[] str1 = form1.lst_rooms.Items[i].ToString().Split(' ');
                                if (str1[0] == req.command)
                                    a = i;
                            }
                            if (a != -1)
                            {
                                form1.lst_rooms.Items.RemoveAt(a);
                                if (req.data == "0")
                                    form1.lst_rooms.Items.Insert(a, req.command + "  ");
                                else
                                    form1.lst_rooms.Items.Insert(a, req.command + "   +" + req.data);
                            }
                            else
                                form1.lst_rooms.Items.Insert(form1.lst_rooms.Items.Count, req.command + "   +" + req.data);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Host lost connection");
                    return;
                }
            }
        }


        public void lst_rooms_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.
            Brush myBrush = Brushes.Black;

            Font myFont = new Font(form1.lst_rooms.Font.Name, form1.lst_rooms.Font.Size,
                FontStyle.Bold, form1.lst_rooms.Font.Unit);

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.
            switch (e.Index)
            {
                case 0:
                    myBrush = Brushes.Red;
                    break;
                case 1:
                    myBrush = Brushes.Orange;
                    break;
                case 2:
                    myBrush = Brushes.Purple;
                    break;
            }

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            string[] tmp = form1.lst_rooms.Items[e.Index].ToString().Split(' ');
            if (!(e.Index == form1.lst_rooms.Items.Count))
            {
                if (tmp.Length == 1)
                    e.Graphics.DrawString(form1.lst_rooms.Items[e.Index].ToString(),
                    form1.lst_rooms.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                else
                    e.Graphics.DrawString(form1.lst_rooms.Items[e.Index].ToString(),
                   myFont, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            }
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }
    }
}
