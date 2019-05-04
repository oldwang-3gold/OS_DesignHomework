using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Unity服务器TCP聊天室
{
    class Program
    {
        static List<Client> clientList=new List<Client>();

        public static void BroadcastMessage(string message)
        {
            var notConnectedList = new List<Client>();
            foreach (Client client in clientList)
            {
                if (client.Connected)
                {
                    client.SendMessage(message);
                }
                else
                {
                    notConnectedList.Add(client);
                }
            }
            //移除断开连接的客户端
            foreach (var temp in notConnectedList)
            {
                clientList.Remove(temp);
            }
        }

        public static string GetLocalIP()
        {
            try
            {
                string hostName = Dns.GetHostName(); //获取主机名
                IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipHostEntry.AddressList.Length; i++) //从IP地址列表筛选出ipv4类型的IP地址
                {
                    if (ipHostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine("本机ip地址为:"+ipHostEntry.AddressList[i].ToString());
                        return ipHostEntry.AddressList[i].ToString();
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine("获取本机IP出错:"+e.ToString());
                return "";
            }
            
        }
        static void Main(string[] args)
        {
            Socket tcpServer=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            string ipAddress = GetLocalIP();
         //   string ipAddress = "192.168.43.171";
            //绑定IP地址使用本地的 通过控制台cmd中 ipconfig找到ipv4地址 端口号设置成四位数最好
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), 7788));
            tcpServer.Listen(100);
            Console.WriteLine("server running");


            while (true)//死循环等待客户端连接进来
            {
                Socket clientSocket = tcpServer.Accept();//直到有客户端连进来才会进行下面操作
                Console.WriteLine("a client connected!");
                Client client=new Client(clientSocket);//把与每个客户端通信的逻辑在client类里实现               
                clientList.Add(client);//添加到集合中
            }      
        }
    }
}
