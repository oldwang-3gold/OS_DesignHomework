using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace 服务器端Server_VS2019_.Server
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientLists;
        public Server(){ }

        public Server(string ipStr, int port)
        {
            SetIpAndPort(ipStr,port);
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);//创建 连接ip地址和端口号
        }

        public void Start()//启动监听
        {
            serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);//绑定ip地址和端口号
            serverSocket.Listen(0);//监听
            serverSocket.BeginAccept(AcceptCallBack, null); //接受客户端的连接
        }
        /// <summary>
        /// 回调函数
        /// </summary>
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);//接受到与服务器端连接的客户端
            Client client=new Client(clientSocket,this);
            client.Start();
            clientLists.Add(client);
        }

        public void RemoveClient(Client client)
        {
            lock (clientLists)//防止线程之间处理异常
            {
                clientLists.Remove(client);
            }
            
        }
    }
}
