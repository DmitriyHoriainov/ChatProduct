using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMultiRoom
{
    class Lobby
    {
        AdminFunc admin;
        Server server;
        public Lobby (Server server)
        {
            admin = new AdminFunc();
            this.server = server;

        }
        public void SetCommand(Request req, int index)
        {
            switch (req.command)
            {
                case "refresh":
                    server.SetRoom(server.clientsList,server.rooms,index);
                    break;
                case "refreshclients":
                    server.SetClient(server.clientsList, index);
                    break;
                case "ban":
                    admin.BanUser(req.data, req.time);
                    break;
                case "unban":
                    admin.Unban(req.data);
                    break;
            }
        }
            

    }
}
