using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    public Text ipAddress;
    public Text port;
    public Text console;
    private static List<Client> clientList;

    public static new void BroadcastMessage(string message)
    {
        var notConnestedList = new List<Client>();
        foreach (Client client in clientList)
        {
            if (client.Connected)
            {
                client.SendMessage(message);
            }
            else
            {
                notConnestedList.Add(client);
            }
        }

        foreach (var tmp in notConnestedList)
        {
            clientList.Remove(tmp);
        }
    }

    public void BtnBindServer()
    {
        Socket tcpServer=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        IPEndPoint ipEndPoint=new IPEndPoint(IPAddress.Parse(ipAddress.text),Int32.Parse(port.text));
        tcpServer.Bind(ipEndPoint);
        tcpServer.Listen(100);//最大监听个数
        console.text = "服务器开始监听\n\n\n";

        while (true)
        {
            Socket clientSocket = tcpServer.Accept();//直到有客户端连进来才会进行下面的操作
            console.text += "一个客户端连接了进来\n\n";
            Client client = new Client(clientSocket);
            clientList.Add(client);
        }
    }
}
