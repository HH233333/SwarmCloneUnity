using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;

public class Chat_Manager : MonoBehaviour
{
    public TMP_Text chat_text;

    public class idinform
    {
        public List<string> Text = new List<string>();
        public List<float> Voice = new List<float>();
        public string emotion;
    }
    public Dictionary<string, idinform> IDdict = new Dictionary<string,idinform>();
    public ConcurrentQueue<string>IDlist = new ConcurrentQueue<string>();
    public request_queue request_Queue;
    public float enter_timer;
 
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

    IEnumerator Model_talking(ConcurrentQueue<string> IDlist)
    {   
        motioncontroller.instance.Is_Talking = true;
        while(!IDlist.IsEmpty)
        {
            string id;
            IDlist.TryDequeue(out id);
            motioncontroller.instance.Motion=IDdict[id].emotion;
            for(int j=0;j<IDdict[id].Text.Count;j++)
            {
                chat_text.text += IDdict[id].Text[j];
                enter_timer = 5;
                yield return new WaitForSeconds(IDdict[id].Voice[j]);
            }
            IDdict.Remove(id);
        }
        Debug.Log(IDdict.Count);
        motioncontroller.instance.Is_Talking = false;
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
                    IDdict.Add(msg.payload["id"].ToString(),new idinform());
                    Debug.Log(IDdict.Count);
                    IDdict[msg.payload["id"].ToString()].emotion = getemotion((Dictionary<string, float>)msg.payload["emotion"]);
                }
                else if(msg.from == "tts" && msg.type == "data")
                {
                    IDdict[msg.payload["id"].ToString()].Text.Add(msg.payload["token"].ToString());
                    IDdict[msg.payload["id"].ToString()].Voice.Add((float)msg.payload["duration"]);    
                }
                else if(msg.from == "tts" && msg.type == "signal" && msg.payload2== "finish")
                {
                    Debug.Log("开始输出");
                    StartCoroutine(Model_talking(IDlist));

                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    private string getemotion(Dictionary<string, float> dict)
    {
        string max_key="";float max_value=0;
        foreach(var item in dict)
        {
            if(item.Value>max_value)
            {
                max_key = item.Key;
                max_value = item.Value;
            }
        }
        return max_key;
    }
}
