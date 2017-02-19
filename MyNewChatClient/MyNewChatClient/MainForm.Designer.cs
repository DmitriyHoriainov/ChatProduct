namespace MyNewChatClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_login = new System.Windows.Forms.Button();
            this.btn_logout = new System.Windows.Forms.Button();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.lst_rooms = new System.Windows.Forms.ListBox();
            this.btn_refresh_rooms = new System.Windows.Forms.Button();
            this.btn_create_room = new System.Windows.Forms.Button();
            this.btn_room_enter = new System.Windows.Forms.Button();
            this.lst_clients = new System.Windows.Forms.ListBox();
            this.btn_private = new System.Windows.Forms.Button();
            this.btn_refresh_clients = new System.Windows.Forms.Button();
            this.btn_ban = new System.Windows.Forms.Button();
            this.btn_unban = new System.Windows.Forms.Button();
            this.lb_hint = new System.Windows.Forms.Label();
            this.lb_rooms = new System.Windows.Forms.Label();
            this.lb_clients = new System.Windows.Forms.Label();
            this.lb_name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(211, 25);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(122, 23);
            this.btn_login.TabIndex = 0;
            this.btn_login.Text = "Log in";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // btn_logout
            // 
            this.btn_logout.Location = new System.Drawing.Point(212, 26);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(122, 23);
            this.btn_logout.TabIndex = 1;
            this.btn_logout.Text = "Log out";
            this.btn_logout.UseVisualStyleBackColor = true;
            this.btn_logout.Visible = false;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(12, 28);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(135, 20);
            this.txt_name.TabIndex = 2;
            // 
            // lst_rooms
            // 
            this.lst_rooms.FormattingEnabled = true;
            this.lst_rooms.Location = new System.Drawing.Point(12, 78);
            this.lst_rooms.Name = "lst_rooms";
            this.lst_rooms.Size = new System.Drawing.Size(184, 238);
            this.lst_rooms.TabIndex = 3;
            this.lst_rooms.Visible = false;
            // 
            // btn_refresh_rooms
            // 
            this.btn_refresh_rooms.Location = new System.Drawing.Point(37, 380);
            this.btn_refresh_rooms.Name = "btn_refresh_rooms";
            this.btn_refresh_rooms.Size = new System.Drawing.Size(135, 23);
            this.btn_refresh_rooms.TabIndex = 4;
            this.btn_refresh_rooms.Text = "Refresh rooms";
            this.btn_refresh_rooms.UseVisualStyleBackColor = true;
            this.btn_refresh_rooms.Visible = false;
            this.btn_refresh_rooms.Click += new System.EventHandler(this.btn_refresh_rooms_Click);
            // 
            // btn_create_room
            // 
            this.btn_create_room.Location = new System.Drawing.Point(37, 322);
            this.btn_create_room.Name = "btn_create_room";
            this.btn_create_room.Size = new System.Drawing.Size(135, 23);
            this.btn_create_room.TabIndex = 5;
            this.btn_create_room.Text = "Create room";
            this.btn_create_room.UseVisualStyleBackColor = true;
            this.btn_create_room.Visible = false;
            this.btn_create_room.Click += new System.EventHandler(this.btn_create_room_Click);
            // 
            // btn_room_enter
            // 
            this.btn_room_enter.Location = new System.Drawing.Point(37, 351);
            this.btn_room_enter.Name = "btn_room_enter";
            this.btn_room_enter.Size = new System.Drawing.Size(135, 23);
            this.btn_room_enter.TabIndex = 6;
            this.btn_room_enter.Text = "Enter the room";
            this.btn_room_enter.UseVisualStyleBackColor = true;
            this.btn_room_enter.Visible = false;
            this.btn_room_enter.Click += new System.EventHandler(this.btn_room_enter_Click);
            // 
            // lst_clients
            // 
            this.lst_clients.FormattingEnabled = true;
            this.lst_clients.Location = new System.Drawing.Point(213, 78);
            this.lst_clients.Name = "lst_clients";
            this.lst_clients.Size = new System.Drawing.Size(120, 238);
            this.lst_clients.TabIndex = 7;
            this.lst_clients.Visible = false;
            // 
            // btn_private
            // 
            this.btn_private.Location = new System.Drawing.Point(211, 336);
            this.btn_private.Name = "btn_private";
            this.btn_private.Size = new System.Drawing.Size(121, 23);
            this.btn_private.TabIndex = 8;
            this.btn_private.Text = "Private room";
            this.btn_private.UseVisualStyleBackColor = true;
            this.btn_private.Visible = false;
            this.btn_private.Click += new System.EventHandler(this.btn_private_Click);
            // 
            // btn_refresh_clients
            // 
            this.btn_refresh_clients.Location = new System.Drawing.Point(213, 380);
            this.btn_refresh_clients.Name = "btn_refresh_clients";
            this.btn_refresh_clients.Size = new System.Drawing.Size(120, 23);
            this.btn_refresh_clients.TabIndex = 9;
            this.btn_refresh_clients.Text = "Refresh clients";
            this.btn_refresh_clients.UseVisualStyleBackColor = true;
            this.btn_refresh_clients.Visible = false;
            this.btn_refresh_clients.Click += new System.EventHandler(this.btn_refresh_clients_Click);
            // 
            // btn_ban
            // 
            this.btn_ban.Location = new System.Drawing.Point(212, 419);
            this.btn_ban.Name = "btn_ban";
            this.btn_ban.Size = new System.Drawing.Size(120, 23);
            this.btn_ban.TabIndex = 10;
            this.btn_ban.Text = "Ban";
            this.btn_ban.UseVisualStyleBackColor = true;
            this.btn_ban.Visible = false;
            this.btn_ban.Click += new System.EventHandler(this.btn_ban_Click);
            // 
            // btn_unban
            // 
            this.btn_unban.Location = new System.Drawing.Point(213, 448);
            this.btn_unban.Name = "btn_unban";
            this.btn_unban.Size = new System.Drawing.Size(119, 23);
            this.btn_unban.TabIndex = 11;
            this.btn_unban.Text = "Unban";
            this.btn_unban.UseVisualStyleBackColor = true;
            this.btn_unban.Visible = false;
            this.btn_unban.Click += new System.EventHandler(this.btn_unban_Click);
            // 
            // lb_hint
            // 
            this.lb_hint.AutoSize = true;
            this.lb_hint.Location = new System.Drawing.Point(10, 9);
            this.lb_hint.Name = "lb_hint";
            this.lb_hint.Size = new System.Drawing.Size(81, 13);
            this.lb_hint.TabIndex = 12;
            this.lb_hint.Text = "Enter username";
            // 
            // lb_rooms
            // 
            this.lb_rooms.AutoSize = true;
            this.lb_rooms.Location = new System.Drawing.Point(9, 62);
            this.lb_rooms.Name = "lb_rooms";
            this.lb_rooms.Size = new System.Drawing.Size(40, 13);
            this.lb_rooms.TabIndex = 14;
            this.lb_rooms.Text = "Rooms";
            this.lb_rooms.Visible = false;
            // 
            // lb_clients
            // 
            this.lb_clients.AutoSize = true;
            this.lb_clients.Location = new System.Drawing.Point(210, 62);
            this.lb_clients.Name = "lb_clients";
            this.lb_clients.Size = new System.Drawing.Size(34, 13);
            this.lb_clients.TabIndex = 15;
            this.lb_clients.Text = "Users";
            this.lb_clients.Visible = false;
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Location = new System.Drawing.Point(14, 31);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(55, 13);
            this.lb_name.TabIndex = 16;
            this.lb_name.Text = "Nickname";
            this.lb_name.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 483);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.lb_clients);
            this.Controls.Add(this.lb_rooms);
            this.Controls.Add(this.lb_hint);
            this.Controls.Add(this.btn_unban);
            this.Controls.Add(this.btn_ban);
            this.Controls.Add(this.btn_refresh_clients);
            this.Controls.Add(this.btn_private);
            this.Controls.Add(this.lst_clients);
            this.Controls.Add(this.btn_room_enter);
            this.Controls.Add(this.btn_create_room);
            this.Controls.Add(this.btn_refresh_rooms);
            this.Controls.Add(this.lst_rooms);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.btn_logout);
            this.Controls.Add(this.btn_login);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btn_login;
        public System.Windows.Forms.Button btn_logout;
        public System.Windows.Forms.TextBox txt_name;
        public  System.Windows.Forms.ListBox lst_rooms;
        public System.Windows.Forms.Button btn_refresh_rooms;
        public System.Windows.Forms.Button btn_create_room;
        public System.Windows.Forms.Button btn_room_enter;
        public  System.Windows.Forms.ListBox lst_clients;
        public System.Windows.Forms.Button btn_private;
        public System.Windows.Forms.Button btn_refresh_clients;
        public System.Windows.Forms.Button btn_ban;
        public System.Windows.Forms.Button btn_unban;
        private System.Windows.Forms.Label lb_hint;
        private System.Windows.Forms.Label lb_rooms;
        private System.Windows.Forms.Label lb_clients;
        private System.Windows.Forms.Label lb_name;
    }
}

