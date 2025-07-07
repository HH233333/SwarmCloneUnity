using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class danmakuvisible : MonoBehaviour
{
    public danmaku_queue danmaku_Queue;
    public danmakucontroller danmakucontroller;
    public GameObject danmakupanel;

    private TMP_Text text;

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.text = "不显示弹幕";
        danmaku_Queue.enabled = false;
        danmakucontroller.enabled = false;
        danmakupanel.SetActive(false);
    }
    public void OnButtonClicked()
    {
        if (text.text == "不显示弹幕")
        {
            text.text = "显示弹幕";
            danmaku_Queue.enabled = true;
            danmakucontroller.enabled = true;
            danmakupanel.SetActive(true);

        }
        else if (text.text == "显示弹幕")
        {
            text.text = "不显示弹幕";
            danmaku_Queue.enabled = false;
            danmakucontroller.enabled = false;
            danmakupanel.SetActive(false);
        }
    }
}
