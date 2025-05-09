using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
public class ButtonClickHandler : MonoBehaviour
{
    public request_queue request_Queue;
    public void OnButtonClicked()
    {
        Debug.Log("按钮被按下！"); 
        var path = Path.Combine(Directory.GetCurrentDirectory(), "test.json");
        string Data = File.ReadAllText(path);
        List<Dictionary<string, object>> datas = JosnToRecv(Data);
        foreach(var data in datas)
        {
            request_Queue.ShareData=data;
        }
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
                {
                    string cased = property.Name;
                    switch (cased)
                    {
                        case "source":
                            recvdata.Add("source", property.Value.ToString());
                            break;
                        case "message_type":
                            recvdata.Add("message_type", property.Value.ToString());
                            break;

                    }
                }
                recvDatas.Add(recvdata);
            }
        }
        return recvDatas;
    }
}
