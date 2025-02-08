using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

public class request_queue : MonoBehaviour
{
    private ConcurrentQueue<RecvData> sharequeue = new ConcurrentQueue<RecvData>();

    public RecvData ShareData{
        get
        {
            RecvData data;
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
