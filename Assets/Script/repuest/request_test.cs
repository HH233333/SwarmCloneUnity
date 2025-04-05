using System.Collections;
using System.Text;
using System.Net.Sockets;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using Tomlyn;
using Tomlyn.Model;
using System.IO;
using UnityEngine.UIElements;

public class request_test : MonoBehaviour
{   
    private string[] arg = Environment.GetCommandLineArgs();
    private TcpClient client;
    private NetworkStream stream;
    public request_queue request_Queue;
    public string serverIP = "localhost";  // 服务端 IP 地址
    public int serverPort = 8006;
    private string configPath = "config.json";
    SendData modelready = new SendData
        {
            from = "frontend",
            type = "signal",
            payload = "ready"
        };

    void Start()
    {       
            var rootpath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            List<string> configPaths = new List<string>
            {
                Path.Combine(Directory.GetCurrentDirectory(), "server_settings.toml"),
                Path.Combine(Directory.GetParent(rootpath).FullName,"config/server_settings.toml"),
            };
            string configPath;
            foreach(var path in configPaths)
            {
                if (File.Exists(path))
                {
                    configPath = path;
                    string tomlContent = File.ReadAllText(configPath);
                    try
                    {
                        var setting = Toml.ToModel(tomlContent);
                        serverIP = ((TomlTable)(((TomlTable)setting["panel"])["server"]))["host"].ToString();
                        serverPort = Convert.ToInt32(((TomlTable)setting["unity_frontend"])["port"]);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("解析 TOML 文件失败: " + ex.Message);
                    }
                }
                else
                    Debug.LogWarning("配置文件不存在: " + path);
            }
            Debug.Log(serverIP);
            Debug.Log(serverPort);
            StartCoroutine(ConnectToServer());
    }
    IEnumerator ConnectToServer()
    { 
        while(true)
        {
            var success = false;
            try
            {   
                client = new TcpClient(serverIP, serverPort);
                success = true;   
            }
            catch (Exception ex)
            {
                Debug.Log("连接失败：" + ex.Message);
            }

            yield return new WaitForSeconds(5);
            Debug.Log("等待链接");
            if(success)
                break;
        }

        stream = client.GetStream();
        Debug.Log("成功连接到服务器");
        string jsonString = JsonConvert.SerializeObject(modelready);
        SendMessages(jsonString);
        StartCoroutine(ReceiveData());
        yield break;
    }
    void SendMessages(string message)
    {
        // 将消息转换为字节数组
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("发送消息: " + message);
    }
    List<RecvData> JosnToRecv(string Josndata)
    {
        string[] datas = Josndata.Split("%SEP%");
        List<RecvData> recvDatas = new List<RecvData>();
        foreach (var data in datas)
        {
            if(data.Length>0)
            {
                Debug.Log(data);
                RecvData recvdata = new RecvData();
                JObject jobject=  JObject.Parse(data);
                foreach (var property in jobject.Values<JProperty>())
                {
                    string cased = property.Name;
                    switch (cased)
                    {
                        case "from":
                            recvdata.from = property.Value.ToString();
                            break;
                        case "type":
                            recvdata.type = property.Value.ToString();
                            break;
                        case "payload":
                            if(property.Value.Type == JTokenType.String)
                                recvdata.payload2 = property.Value.ToString();
                            else
                                foreach (var property2 in property.Value.Values<JProperty>())
                                {
                                    if (property2.Value.Type == JTokenType.Float)
                                        recvdata.payload.Add(property2.Name, (float)property2.Value);
                                    else if(property2.Value.Type == JTokenType.String)
                                        recvdata.payload.Add(property2.Name, property2.Value.ToString());
                                    else
                                    {
                                        Dictionary<string,float> emotion = new Dictionary<string,float>();
                                        foreach (var property3 in property2.Value.Values<JProperty>())
                                            emotion.Add(property3.Name, (float)property3.Value);
                                        recvdata.payload.Add(property2.Name, emotion);
                                    }    
                                        
                                }
                            break;

                    }
                }
                recvDatas.Add(recvdata);
            }
        }
        
        return recvDatas;
    }
    IEnumerator ReceiveData()
    {
        while(true)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[4098];
                int byteread = stream.Read(buffer, 0, buffer.Length);
                string receivedData = Encoding.UTF8.GetString(buffer, 0, byteread);
                Debug.Log("收到服务器消息: "+receivedData);
                List<RecvData> recvdatas = JosnToRecv(receivedData);
                foreach(var data in recvdatas)
                {
                    request_Queue.ShareData=data;
                }
            }
            yield return null;
        }
    }
}
