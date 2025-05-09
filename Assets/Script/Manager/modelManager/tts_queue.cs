using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
public class ttsdata
{
    public List<string> Text = new List<string>();
    public List<float> Duration = new List<float>();
    public string emotion;
    public AudioClip Audio;
}


public class tts_queue : MonoBehaviour
{
    private Dictionary<string,ttsdata> ttsdatadict = new Dictionary<string,ttsdata>();
    private ConcurrentQueue<string>IDlist = new ConcurrentQueue<string>();  
    private string ID;
    public void GetTtsData(Dictionary<string, object> data)
    {
        ID = data.ContainsKey("id") ? data["id"].ToString() : ID;
        if(ID!=null && !ttsdatadict.ContainsKey(ID))
        {
            ttsdatadict.Add(data["id"].ToString(),new ttsdata());
            Debug.Log("putin");
        }
        if ((string)data["source"] == "LLM" && (string)data["message_type"] != "Signal")
        {
            ttsdatadict[ID].emotion = getmax((Dictionary<string, object>)data["emotion"]);
        }
        else if((string)data["source"] == "TTS" && data.ContainsKey("data"))
        {
            if(data["data"] is string)
                ttsdatadict[ID].Audio = WavUtility.ToAudioClip(data["data"].ToString());
            else
                ttsdatadict[ID].Audio = WavUtility.ToAudioClip((byte[])data["data"]);
        }
        else if((string)data["source"] == "TTS" && data.ContainsKey("token"))
        {   
            ttsdatadict[ID].Duration.Add((float)data["duration"]); 
            ttsdatadict[ID].Text.Add(data["token"].ToString());
        }
        else if((string)data["source"] == "TTS" && (string)data["message_type"] == "Signal")
        {
            IDlist.Enqueue(ID);
        }
        Debug.Log(IDlist.Count);
    }
    private string getmax(Dictionary<string, object> dict)
    {
        string max_key="";float max_value=0;
        foreach(var item in dict)
        {
            if((int)item.Value>max_value)
            {
                max_key = item.Key;
                max_value = (int)item.Value;
            }
        }
        return max_key;
    }
    public bool PutTtsData(out ttsdata data)
    {
        if(IDlist.TryDequeue(out string ID))
        {
            data = ttsdatadict[ID];
            ttsdatadict.Remove(ID);
            return true;    
        }
        else
        {    
            data = null;
            return false;
        }
    }

}