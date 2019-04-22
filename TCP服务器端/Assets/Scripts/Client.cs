using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class Client :MonoBehaviour
{
    private Socket clientSocket;
    private Thread t;
    private byte[] data=new byte[1024];

    public Client(Socket s)
    {
        clientSocket = s;
        //启动一个线程 处理客户端的数据接收
        t=new Thread(ReceiveMessage);
        t.Start();
    }

    void Update()
    {
        ReceiveMessage();
    }
    private void ReceiveMessage()
    {
        //在接收数据之前，判断一下Socket链接是否断开
        if (clientSocket.Poll(10,SelectMode.SelectRead))//判断是否能从客户端读取到信息
        {
            clientSocket.Close();
            return;
        }

        int length = clientSocket.Receive(data);//Receive用来接收发送过来的数据
        string message = Encoding.UTF8.GetString(data, 0, length);//将接收到的数据转换为string
        Server.BroadcastMessage(message);
        //接收到数据的时候 广播给所有客户端

    }

    public new void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    public bool Connected
    {
        get { return clientSocket.Connected; }
    }

}
