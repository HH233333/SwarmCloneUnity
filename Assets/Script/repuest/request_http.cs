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
using UnityEngine.Networking;

public class request_http : MonoBehaviour
{   
    private string[] arg = Environment.GetCommandLineArgs();
    private WWWForm formData ;
    public request_queue request_Queue;
    public string serverIP = "localhost";  // 服务端 IP 地址
    public int serverPort = 8002;

    void Start()
    {       
            formData = new WWWForm();
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
            
            StartCoroutine(StartWebServer($"http://{serverIP}:{serverPort}/"));
    }
    IEnumerator StartWebServer(string url)
    { 
        Debug.Log(url);
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, formData))
            {
                // 设置自定义头信息处理器
                www.SetRequestHeader("source", "");
                www.SetRequestHeader("message_type", "");
                www.uploadHandler = new UploadHandlerRaw(new byte[0]);
                www.downloadHandler = new DownloadHandlerBuffer();

                // 发送请求并等待响应
                yield return www.SendWebRequest();

                // 检查是否有错误
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error receiving data: " + www.error);
                }
                else
                {
                    // 读取请求头信息
                    string source = www.GetResponseHeader("source");
                    string messageType = www.GetResponseHeader("message_type");
                    // 读取响应内容
                    string responseContent = www.downloadHandler.text;
                    byte[] binaryData = www.downloadHandler.data;

                    // 处理接收到的数据
                    request_Queue.ShareData = ProcessReceivedData(source, messageType, responseContent, binaryData);
                }
            }
        }
    }
    Dictionary<string, object> ProcessReceivedData(string source, string messageType, string responseContent, byte[] binaryData)
    {
                Debug.Log(responseContent);
                Dictionary<string, object> recvdata = new Dictionary<string, object>
                {
                    { "source", source },
                    { "messageType", messageType}
                };
                JObject jobject=  JObject.Parse(responseContent);
                foreach (var property in jobject.Values<JProperty>())
                    JPropertytodict(recvdata, property);
                if(binaryData.Length > 0)
                    recvdata.Add("data", binaryData);
        return recvdata;
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
}
