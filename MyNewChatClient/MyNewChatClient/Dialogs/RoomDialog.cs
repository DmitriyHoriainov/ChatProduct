using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace MyNewChatClient
{
    public partial class RoomDialog : Form
    {
        TcpClient connection;
        public Room room;
        Request request;

        public RoomDialog(TcpClient connection, Request request)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.connection = connection;
            room = new Room(connection);
            this.request = request;
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            if (txt_msg.Text == "")
            {
                MessageBox.Show("Please enter message.");
            }
            if (txt_msg.Text.Length > 500)
            {
                MessageBox.Show("Please enter message lesser then 500 symbols.");
            }
            else
            {
                room.SendHendler(txt_msg.Text, request);
                txt_msg.Text = "";
            }
            MissedMessages();
        }
        private void btn_back_Click(object sender, EventArgs e)
        {
            room.LeaveHendler(sender, request);
        }

        private void RoomDialog_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void MissedMessages()
        {
            string[] tmp = rtb_message.Text.Split('\n');
            if (tmp.Length != 0)
            {
                for (int i = 0; i < tmp.Length; i++)
                {
                    string str11 = tmp[i];
                    int j = 0;
                    try
                    {
                        //while (j <= rtb_message.Text.Length - str11.Length)
                        //{
                            //выделение цветом
                            j = rtb_message.Text.IndexOf(str11, j);
                            if (j < 0) break;
                            rtb_message.SelectionStart = j;
                            rtb_message.SelectionLength = str11.Length;
                            rtb_message.SelectionColor = Color.Black;
                            j += str11.Length;
                        //}
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void RoomDialog_MouseClick(object sender, MouseEventArgs e)
        {
            
        }


        private void txt_msg_MouseClick(object sender, MouseEventArgs e)
        {
            MissedMessages();
        }

        private void rtb_message_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
