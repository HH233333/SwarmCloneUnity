using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
public class ButtonClickHandler1 : MonoBehaviour
{
    public motioncontroller motioncontroller;
    public string emotion;
    public void OnButtonClicked()
    {
        Debug.Log("按钮被按下！");
        motioncontroller.Motion = emotion;
    }
}
