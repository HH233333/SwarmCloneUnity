using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

// public class RecvData
// {
//     public string from;
//     public string type;
//     public Dictionary<string, object> payload= new Dictionary<string, object>();
//     public string payload2;
// }

// public class SendData
// {
//     public string from;
//     public string type;
//     public string payload;
// }
public class request_queue : MonoBehaviour
{
    private ConcurrentQueue<Dictionary<string, object>> sharequeue = new ConcurrentQueue<Dictionary<string, object>>();

    public Dictionary<string, object> ShareData{
        get
        {
            Dictionary<string, object> data;
            sharequeue.TryDequeue(out data);
            return data;
        }

        set
        {
            sharequeue.Enqueue(value);
        }
    }

    public bool isempty()
    {
        return sharequeue.IsEmpty;
    }
}
