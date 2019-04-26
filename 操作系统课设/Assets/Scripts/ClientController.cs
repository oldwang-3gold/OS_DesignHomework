using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{

    public Text ipAddress;
    public Text port;
    public Text context;
    private Socket clientSocket;
    public InputField inputField;
    private Thread t;
    private byte[] data = new byte[1024];//接受服务器端消息容器
    private string message = "";

    void Update()
    {
        if (message != null && message != "")
        {
            context.text += "\n" + message;
            message = "";//清空消息
        }

        if (Input.GetKey("escape"))//关闭客户端
        {
            clientSocket.Close();
            Application.Quit();
        }
    }

    public void BtnConnected()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress.text), int.Parse(port.text));
            clientSocket.Connect(ipEndPoint);
            context.text = "成功连接上服务器端" + "\n";
        }
        catch (Exception e)
        {
            context.text += e.ToString();
            return;
        }
        finally
        {
            t = new Thread(ReceiveMessage);
            t.Start();
        }
        
    }

    new void SendMessage(string message)
    {
        string name = PlayerPrefs.GetString("人物名字");
        message = name + ":" + message;
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                return;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            Debug.Log("接收到的信息为:"+message);
        }         
    }

    public void OnBtnSend()
    {
        SendMessage(inputField.text);
        inputField.text = "";//清空
    }

    
}
