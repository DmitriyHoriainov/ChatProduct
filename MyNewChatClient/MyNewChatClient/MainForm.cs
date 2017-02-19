using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MyNewChatClient
{
    public partial class MainForm : Form
    {
        Dialog dialog;
        TcpClient client;
        Auth auth;
        RefreshListBoxes refresh;
        public Request request;
        Listener listener;
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            auth = new Auth();
            refresh = new RefreshListBoxes();
            request = new Request();

            lst_rooms.DrawMode = DrawMode.OwnerDrawFixed;
            
        }      

        private void btn_create_room_Click(object sender, EventArgs e)
        {
            dialog = new Dialog("createroom",client,"name", request);
            Thread tr = new Thread(new ThreadStart(OpenDialogForm));
            tr.Start();
            Thread.Sleep(100);
            refresh.RefreshHendler(client.GetStream(), "Rooms", request);
        }
        private void btn_private_Click(object sender, EventArgs e)
        {
            if (lst_clients.SelectedItem != null)
            {
                PrivateRoom create = new PrivateRoom();
                create.CreatePrivateHandler(client.GetStream(), lst_clients.SelectedItem.ToString(), request);
            }
            else
            {
                MessageBox.Show("User is not choosen. Please, chose the user first.");
            }
           // refresh.RefreshHendler(client.GetStream(), "Rooms", request);
        }
        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckName())
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 8888);
                    auth.LogInHendler(client, txt_name.Text);

                    listener = new Listener(client, this);
                    lst_rooms.DrawItem += new DrawItemEventHandler(listener.lst_rooms_DrawItem);

                    lst_rooms.Visible = true;
                    btn_create_room.Visible = true;
                    btn_refresh_rooms.Visible = true;
                    btn_room_enter.Visible = true;
                    btn_refresh_clients.Visible = true;
                    btn_private.Visible = true;
                    lst_clients.Visible = true;
                    lb_rooms.Visible = true;
                    lb_clients.Visible = true;
                    btn_logout.Visible = true;
                    lb_name.Text = txt_name.Text;
                    lb_name.Visible = true;



                    btn_login.Visible = false;
                    txt_name.Visible = false;
                    lb_hint.Visible = false;

                    refresh.RefreshHendler(client.GetStream(), "Rooms", request);
                    refresh.RefreshHendler(client.GetStream(), "clients", request);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is a connection to the host");
            }           
        }

        private bool CheckName()
        {
            Regex rgx = new Regex("^[а-яА-ЯёЁa-zA-Z0-9]+$");
            if (txt_name.Text.Length == 0)
            {
                MessageBox.Show("Please enter the username");
                return false;
            }
            else if (txt_name.Text.Length > 10)
            {
                MessageBox.Show("Very long username! Enter username till 10 symbols.");
                return false;
            }
            else if (!rgx.IsMatch(txt_name.Text.ToString()))
            {
                MessageBox.Show("Username contains only letters and numbers");
                return false;
            }           
            return true;
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            auth.LogoutHendler(client, request);

            lst_rooms.Visible = false;
            btn_create_room.Visible = false;
            btn_refresh_rooms.Visible = false;
            btn_room_enter.Visible = false;
            btn_refresh_clients.Visible = false;
            btn_private.Visible = false;
            lst_clients.Visible = false;
            lb_rooms.Visible = false;
            lb_clients.Visible = false;
            btn_logout.Visible = false;
            lb_name.Visible = false;
            btn_unban.Visible = false;
            btn_ban.Visible = false;

            txt_name.Text = "";

            btn_login.Visible = true;
            txt_name.Visible = true;
            lb_hint.Visible = true;
        }

        private void btn_refresh_rooms_Click(object sender, EventArgs e)
        {
            refresh.RefreshHendler(client.GetStream(),"Rooms", request);
        }

        private void btn_refresh_clients_Click(object sender, EventArgs e)
        {
            refresh.RefreshHendler(client.GetStream(), "clients", request);
        }

        private void btn_room_enter_Click(object sender, EventArgs e)
        {
            EnterRoom();
        }

        private void btn_ban_Click(object sender, EventArgs e)
        {
            if (lst_clients.SelectedItem != null)
            {
                dialog = new Dialog("ban",client, lst_clients.SelectedItem.ToString(), request);
                Thread tr = new Thread(new ThreadStart(OpenDialogForm));
                tr.Start();
            }
            else
            {
                MessageBox.Show("Please choose user to ban");
            }
        }

        private void EnterRoom()
        {
            if (lst_rooms.SelectedItem != null)
            {
                string[] str = lst_rooms.SelectedItem.ToString().Split(' ');
                request.modul = "rooms";
                request.command = "enter";
                request.data = str[0];
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(JsonConvert.SerializeObject(request));
                writer.Flush();             
            }
            else
            {
                MessageBox.Show("Room is not choosen. Please, chose the room first.");
            }
        }

        private void OpenDialogForm()
        {
            dialog.ShowDialog();
        }
        public void VisibleBan()
        {
           btn_ban.Visible = true;
        }
        public void VisibeUnban()
        {
            btn_unban.Visible = true;
        }

        private void btn_unban_Click(object sender, EventArgs e)
        {

            if (lst_clients.SelectedItem != null)
            {
                dialog = new Dialog("unban",client, lst_clients.SelectedItem.ToString(),request);
                Thread tr = new Thread(new ThreadStart(OpenDialogForm));
                tr.Start();
            }
        }
    }
}
