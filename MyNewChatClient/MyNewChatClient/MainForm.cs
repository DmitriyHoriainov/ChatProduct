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
            CreateClick();
        }

        private void CreateClick()
        {
            dialog = new Dialog("createroom", client, "name", request);
            Thread tr = new Thread(new ThreadStart(OpenDialogForm));
            tr.Start();
            Thread.Sleep(100);
            refresh.RefreshHendler(client.GetStream(), "Rooms", request);
        }
        private void btn_private_Click(object sender, EventArgs e)
        {
            PrivateClick();
        }      

        private void PrivateClick()
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
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            LoginClick();      
        }

        private void LoginClick()
        {
            try
            {
                if (CheckName())
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 8888);
                    auth.LogInHendler(client, txt_name.Text, txt_password.Text, request);
                    listener = new Listener(client, this);
                    Thread.Sleep(500);
                    lst_rooms.DrawItem += new DrawItemEventHandler(lst_rooms_DrawItem);

                    if (lb_name.Text == txt_name.Text)
                    {
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
                        ps_hint.Visible = false;
                        txt_password.Visible = false;
                        btn_reg.Visible = false;

                        refresh.RefreshHendler(client.GetStream(), "Rooms", request);
                        Thread.Sleep(100);
                        refresh.RefreshHendler(client.GetStream(), "clients", request);
                    }
                    else
                    {
                        MessageBox.Show("Incorrect name or password or already logged in");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a connection to the host");
            }
        }

        private bool CheckName()
        {
            Regex rgx = new Regex("^[a-zA-Z0-9]+$");
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
            if (txt_password.Text.Length == 0)
            {
                MessageBox.Show("Please enter the password");
                return false;
            }
            else if (txt_password.Text.Length > 10)
            {
                MessageBox.Show("Very long password! Enter password till 10 symbols.");
                return false;
            }
            else if (!rgx.IsMatch(txt_name.Text.ToString()))
            {
                MessageBox.Show("Username contains only english letters and numbers");
                return false;
            }
            else if (!rgx.IsMatch(txt_password.Text.ToString()))
            {
                MessageBox.Show("Password contains only english letters and numbers");
                return false;
            }
            else if (txt_name.Text.ToString().Contains(" "))
            {
                MessageBox.Show("Username contains only letters and numbers");
                return false;
            }
            else if (txt_password.Text.ToString().Contains(" "))
            {
                MessageBox.Show("Password contains only letters and numbers");
                return false;
            }
            return true;
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            auth.LogoutHendler(client, request);
         
            Application.Restart();
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
            BanClick();
        }

        private void BanClick()
        {
            if (lst_clients.SelectedItem != null)
            {
                dialog = new Dialog("ban", client, lst_clients.SelectedItem.ToString(), request);
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
            UnbanClick();
        }

        private void UnbanClick()
        {
            if (lst_clients.SelectedItem != null)
            {
                dialog = new Dialog("unban", client, lst_clients.SelectedItem.ToString(), request);
                Thread tr = new Thread(new ThreadStart(OpenDialogForm));
                tr.Start();
            }
            else
            {
                MessageBox.Show("Please choose user to unban");
            }
        }

        private void btn_reg_Click(object sender, EventArgs e)
        {
            RegClick();
        }

        private void RegClick()
        {
            if (CheckName())
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 8888);
                auth.RegistationHendler(client, txt_name.Text, txt_password.Text, request);
                listener = new Listener(client, this);
                Thread.Sleep(500);
                lst_rooms.DrawItem += new DrawItemEventHandler(lst_rooms_DrawItem);

                if (lb_name.Text == txt_name.Text)
                {
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
                    ps_hint.Visible = false;
                    txt_password.Visible = false;


                    btn_login.Visible = false;
                    txt_name.Visible = false;
                    lb_hint.Visible = false;
                    btn_reg.Visible = false;
                    refresh.RefreshHendler(client.GetStream(), "Rooms", request);
                    refresh.RefreshHendler(client.GetStream(), "clients", request);

                }
                else
                {
                    MessageBox.Show("User already exists");
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                auth.LogoutHendler(client, request);
                Application.Exit();
            }
            catch(Exception ex)
            {
                Environment.Exit(0);
            }
        }

        public void lst_rooms_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.DrawBackground();

                Brush myBrush = Brushes.Black;

                Font myFont = new Font(lst_rooms.Font.Name, lst_rooms.Font.Size,
                    FontStyle.Bold, lst_rooms.Font.Unit);

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

                string[] tmp = lst_rooms.Items[e.Index].ToString().Split(' ');
                if (!(e.Index == lst_rooms.Items.Count))
                {
                    if (tmp.Length == 1)
                        e.Graphics.DrawString(lst_rooms.Items[e.Index].ToString(),
                        lst_rooms.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                    else
                        e.Graphics.DrawString(lst_rooms.Items[e.Index].ToString(),
                       myFont, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                }
                e.DrawFocusRectangle();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
