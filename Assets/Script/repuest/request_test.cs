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
    private string terminator = "%SEP%";
    private string receivedData;

    void Start()
    {       
            var rootpath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            Debug.Log(rootpath);
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
                        terminator = ((TomlTable)(((TomlTable)setting["panel"])["server"]))["requests_separator"].ToString();
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
        StartCoroutine(ReceiveData());
        yield break;
    }
    List<Dictionary<string, object>> JosnToRecv(string Josndata)
    {
        string[] datas = Josndata.Split("%SEP%");
        List<Dictionary<string, object>> recvDatas = new List<Dictionary<string, object>>();
        foreach (var data in datas)
        {
            if(data.Length>0)
            {
                Debug.Log(data);
                Dictionary<string, object> recvdata = new Dictionary<string, object>();
                JObject jobject=  JObject.Parse(data);
                foreach (var property in jobject.Values<JProperty>())
                    JPropertytodict(recvdata, property);
                recvDatas.Add(recvdata);
            }
        }
        
        return recvDatas;
    }
    void JPropertytodict(Dictionary<string, object> dict, JProperty jProperty)
    {
        JTokenType cased = jProperty.Value.Type;
        switch (cased)
        {
            case JTokenType.String:
                dict.Add(jProperty.Name, jProperty.Value.ToString());
                break;
            case JTokenType.Float:
                dict.Add(jProperty.Name, (float)jProperty.Value);
                break;
            case JTokenType.Integer:
                dict.Add(jProperty.Name, (int)jProperty.Value);
                break;
            case JTokenType.Object:
                Dictionary<string, object> dict2 = new Dictionary<string, object>();
                foreach (var property in jProperty.Value.Values<JProperty>())
                    JPropertytodict(dict2, property);
                dict.Add(jProperty.Name, dict2);
                break;
        }
        return;
    }
    IEnumerator ReceiveData()
    {
        while(true)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                int byteread = stream.Read(buffer, 0, buffer.Length);
                string chunk = Encoding.UTF8.GetString(buffer, 0, byteread);
                receivedData += chunk;
                if(receivedData.EndsWith(terminator))
                {
                    Debug.Log("收到服务器消息: "+receivedData);
                    List<Dictionary<string, object>> recvdatas = JosnToRecv(receivedData);
                    foreach(var data in recvdatas)
                    {
                        request_Queue.ShareData=data;
                    }
                    receivedData = "";
                }
            }
            yield return null;
        }
    }
}
