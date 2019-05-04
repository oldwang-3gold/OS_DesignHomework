using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unity服务器TCP聊天室
{
    class Client
    {
        private Socket clientSocket;
        private Thread t;
        private  byte[] data=new byte[1024];

        public Client(Socket s)
        {
            clientSocket = s;
            //启动一个线程 处理客户端的数据接受
            t=new Thread(ReceiveMessage);
            t.Start();
        }

        private void ReceiveMessage()
        {
            //一直接收客户端的数据
            while (true)
            {
               // 在接收数据之前 判断一下socket链接是否断开
                //if (clientSocket.Poll(-1, SelectMode.SelectRead))//判断能否从客户端读取消息
                //{
                //    clientSocket.Close();
                //    break;//跳出循环 终止线程的执行
                //}



                try
                {
                    int length = clientSocket.Receive(data); //Receive用来承接发送过来的数据
                    if (length == 0)//接受空消息
                    {
                        clientSocket.Close();
                        break; 
                    }             
                    string message = Encoding.UTF8.GetString(data, 0, length); //将接收到数据转换为string

                    //接收到数据的时候 要把这个数据分发到客户端(聊天室)
                    Program.BroadcastMessage(message);
                    Console.WriteLine("收到了消息:" + message);
                }
                catch (Exception e) //处理Unity客户端强制关闭的异常
                {
                    Console.WriteLine(e.ToString());
                    if (clientSocket != null)
                    {
                        clientSocket.Close();
                        break;
                    }
                }
               
                
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        public bool Connected
        {
            get { return clientSocket.Connected; }
        }
    }
}
