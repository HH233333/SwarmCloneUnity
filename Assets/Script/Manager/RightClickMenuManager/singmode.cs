using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class singmode : MonoBehaviour
{
    public GameObject audiovisbleupanel;
    private TMP_Text text;
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        switch ((int)Manager.instance.state.modelstatu)
        {
            case 1: text.text = "对话模式"; break;
            case 2: text.text = "歌回模式"; break;
        }

    }
    public void OnButtonClicked()
    {
        if (text.text == "对话模式")
        {
            text.text = "歌回模式";
            Manager.instance.state.modelstatu = modelstatu.singing;
        }
        else if (text.text == "歌回模式")
        {
            text.text = "对话模式";
            Manager.instance.state.modelstatu = modelstatu.talking;
        }
    }
}
