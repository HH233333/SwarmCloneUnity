using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using System.Data.Common;
using Unity.VisualScripting;

public class Chat_Manager : MonoBehaviour
{
    public TMP_Text chat_text;
    public float text_wait_time=0.1f;
    public Dictionary<string, List<string>> TextID = new Dictionary<string,List<string>>();
    public Dictionary<string, List<float>>VoiceID = new Dictionary<string, List<float>>();
    public ConcurrentQueue<string>IDlist = new ConcurrentQueue<string>();
    public Model_Manager model_Manager;
    public float enter_timer;
    public request_queue request_Queue;
    private void Start()
    {
        StartCoroutine(GetMSG());
    }


    private void Update()
    {
        if (enter_timer>0)
        {
            enter_timer -= Time.deltaTime;
            if (enter_timer<=0)
            {
                chat_text.text = "";
            }
        }
    }

    public void SetSomeText()
    {
        List<string> ids = new List<string>();
        chat_text.text = "";
        while(!IDlist.IsEmpty)
        {
            string id;
            IDlist.TryDequeue(out id);
            ids.Add(id);
        }
        print("文本输入");
        StartCoroutine(Model_talking(ids));
  
     
    }
    IEnumerator Model_talking(List<string> ids)
    {
        foreach(var id in ids)
        {
            for(int i=0;i<TextID[id].Count;i++)
            {
                chat_text.text += TextID[id][i];

                model_Manager.Is_talking = true;

                enter_timer = 5;
                yield return new WaitForSeconds(VoiceID[id][i]);
            }
            TextID.Remove(id);
            VoiceID.Remove(id);
        }
        model_Manager.Is_talking = false;
        yield break;
    }
    IEnumerator GetMSG()
    {   
        
        while(true)
        {   
            if(!request_Queue.isempty())
            {   
                RecvData msg = request_Queue.ShareData;
                if(msg.from == "llm" && msg.type == "data")
                {
                    IDlist.Enqueue(msg.payload["id"].ToString());
                    Debug.Log("字典添加");
                }
                else if(msg.from == "tts" && msg.type == "data")
                {
                    if(TextID.ContainsKey(msg.payload["id"].ToString())==false)
                        TextID.Add(msg.payload["id"].ToString(),new List<string>());
                    TextID[msg.payload["id"].ToString()].Add(msg.payload["token"].ToString());
                    if(VoiceID.ContainsKey(msg.payload["id"].ToString())==false)
                        VoiceID.Add(msg.payload["id"].ToString(),new List<float>());
                    VoiceID[msg.payload["id"].ToString()].Add((float)msg.payload["duration"]);
                }
                else if(msg.from == "tts" && msg.type == "signal" && msg.payload2 == "finish")
                {   
                    SetSomeText();
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
}
