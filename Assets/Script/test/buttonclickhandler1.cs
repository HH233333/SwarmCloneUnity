using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Live2D.Cubism.Core;
public class ButtonClickHandler1 : MonoBehaviour
{
    public motioncontroller motioncontroller;
    public string emotion;

    void Start()
    {
        var model = FindObjectOfType<CubismModel>();
        Debug.Log(model);
        motioncontroller = model.GetComponent<motioncontroller>();
    }
    public void OnButtonClicked()
    {
        Debug.Log("按钮被按下！");
        motioncontroller.Motion = emotion;
    }
}
