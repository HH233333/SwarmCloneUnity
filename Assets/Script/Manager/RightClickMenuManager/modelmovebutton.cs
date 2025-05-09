using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class modelmovebutton : MonoBehaviour
{
    public positioncontroller positioncontroller;
    public void OnButtonClicked()
    {
        var text = GetComponentInChildren<TMP_Text>();
        if(text.text == "模型不可移动")
        {
            text.text = "模型可移动";
            positioncontroller.enabled = true;
        }
        else if(text.text == "模型可移动")
        {
            text.text = "模型不可移动";
            positioncontroller.enabled = false;
        }
    }
}
