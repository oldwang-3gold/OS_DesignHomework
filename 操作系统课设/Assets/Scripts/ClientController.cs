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
    public Text playerName;
    private Socket clientSocket;
    public InputField inputField;
    public GameObject ConnectPanel;
    public GameObject ChatContext;
    private Thread t;
    private byte[] data = new byte[1024];//接受服务器端消息容器
    private string message = "";

    void Awake()
    {
        playerName.text = PlayerPrefs.GetString("人物名字");
        ChatContext.SetActive(false);
    }

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

        if (Input.GetKey(KeyCode.Return))
        {
            OnBtnSend();
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
            string sucessMessage = "欢迎" + playerName.text + "进入聊天室\n";
            SendMessage(sucessMessage);
            ChatContext.SetActive(true);
            ConnectPanel.SetActive(false);
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
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }
        catch
        {
            context.text += "\n\n服务器端已关闭";
        }
        
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
        if (inputField.text == "") return;
        string message= playerName.text + ":" + inputField.text;
        SendMessage(message);
        inputField.text = "";//清空
    }

    public void OnBtnClearScreen()
    {
        context.text = "";
    }

    public void OnBtnQuit()
    {
        clientSocket.Close();
        Application.Quit();
    }
    
}
