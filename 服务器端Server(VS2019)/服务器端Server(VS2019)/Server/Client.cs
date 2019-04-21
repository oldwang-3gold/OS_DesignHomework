using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace 服务器端Server_VS2019_.Server
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        public Client() { }

        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }

        public void Start()//开启监听服务器端的数据
        {
            clientSocket.BeginReceive(null, 0, 0, SocketFlags.None, ReceiveCallBack,null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0) //接受到数据长度为0 说明断开连接
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
            
        }

        private void Close()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }
        
    }
}
