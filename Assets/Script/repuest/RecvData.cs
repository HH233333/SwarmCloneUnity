using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class RecvData
{
    public string from;
    public string type;
    public Dictionary<string, object> payload= new Dictionary<string, object>();
    public string payload2;
}

public class SendData
{
    public string from;
    public string type;
    public string payload;
}
