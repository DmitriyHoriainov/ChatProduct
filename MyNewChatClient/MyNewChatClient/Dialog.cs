using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNewChatClient
{
    public partial class Dialog : Form

    {
        string name;
        TcpClient client;
        string type;
        Request request;
        public Dialog(string type, TcpClient client,string name, Request request)
        {
            InitializeComponent();
            this.name = name;
            this.client = client;
            this.type = type;
            this.request = request;
            SelectType();
            

        }
        private void SelectType()
        {
            switch(type)
            {
                case "ban":
                    IniBan();
                    break;
                case "unban":
                    IniUnban();
                    break;
                case "createroom":
                    IniCreateRoom();
                    break;
            }
        }
        private void IniBan()
        {
            
            lb.Text = "Do you want to ban " + name + " ?";
            btn1.Text = "Permanent";
            btn2.Text = "For some time"; 
        }
        private void IniUnban()
        {
            lb.Text = "Do you want unban " + name + " ?";
            tb.Visible = false;
            btn1.Text = "Cancel";
            btn2.Text = "Unban";
        }
        private void IniCreateRoom()
        {
            lb.Text = "Enter Room name";
            btn1.Text = "Create Room";
            btn2.Text = "Cancel";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            switch (type)
            {
                case "ban":
                    Ban b = new Ban(name);
                    b.ForeverBanHendler(client.GetStream(), request);
                    MessageBox.Show(name + "has been banned for permanent");
                    this.Close();
                    break;
                case "unban":                  
                    this.Close();
                    break;
                case "createroom":
                    if (CheckName())
                    {
                        CreateRoom cr = new CreateRoom();
                        cr.CreateHendler(tb.Text.ToString(), client.GetStream(), request);
                        this.Close();
                    }
                    break;
            }
        }

        private bool CheckName()
        {
            Regex rgx = new Regex("^[а-яА-ЯёЁa-zA-Z0-9]+$");
            if (tb.Text.Length == 0)
            {
                MessageBox.Show("Please enter the room name");
                return false;
            }
            else if (tb.Text.Length > 20)
            {
                MessageBox.Show("Very long room name! Enter room name till 20 symbols.");
                return false;
            }
            else if (!rgx.IsMatch(tb.Text.ToString()))
            {
                MessageBox.Show("Room name contains only letters and numbers");
                return false;
            }
            return true;
        }



        private void btn2_Click(object sender, EventArgs e)
        {
            switch (type)
            {
                case "ban":
                    if (CheckTime())
                    {
                        Ban b = new Ban(name);
                        b.ForTimeBanHendler(Convert.ToInt32(tb.Text.ToString()), client.GetStream(), request);
                        MessageBox.Show(name + "has been banned for " + tb.Text.ToString() + "seconds");
                        this.Close();                       
                    }
                    break;
                case "unban":
                    Unban unb = new Unban( name);
                    unb.UnbanHandler(client.GetStream(), request);
                    MessageBox.Show(name + " has been unbaned");
                    this.Close();
                    break;
                case "createroom":                   
                    this.Close();
                    break;
            }
        }

        private bool CheckTime()
        {
            try
            {
                if (Convert.ToInt32(tb.Text.ToString()) > 9999)
                {
                    MessageBox.Show("Maximum ban time 9999");
                    return true;
                }
                if (Convert.ToInt32(tb.Text.ToString()) < 1)
                {
                    MessageBox.Show("Minimum ban time 60");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter only numbers");
                return true;
            }
            return false;
        }
    }
}
