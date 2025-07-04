using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Live2D.Cubism.Framework.Physics;
using Live2D.Cubism.Core;

public class modelmovebutton : MonoBehaviour
{
    void Start()
    {
    }
    public void OnButtonClicked()
    {
        var text = GetComponentInChildren<TMP_Text>();
        if (text.text == "模型不可移动")
        {
            text.text = "模型可移动";
            positioncontroller.instance.enabled = true;
        }
        else if (text.text == "模型可移动")
        {
            text.text = "模型不可移动";
            positioncontroller.instance.enabled = false;
        }
    }
}
