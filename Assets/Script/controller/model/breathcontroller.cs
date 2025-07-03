using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Core;
using System.Runtime.CompilerServices;
using UnityEditor.IMGUI.Controls;
public class breathcontroller : MonoBehaviour, ICubismUpdatable
{
    private CubismModel cubismModel;
    private CubismParameter cubismParameter;
    private float timecount;

    public float x;
    public bool HasUpdateController { get; set; }

    public int ExecutionOrder
    {
        get { return 90; }
    }

    public bool NeedsUpdateOnEditing
    {
        get { return false; }
    }

    void Start()
    {
        cubismModel = GetComponent<CubismModel>();
        cubismParameter = cubismModel.Parameters.FindById("ParamBreath");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLateUpdate()
    {
        timecount += Time.deltaTime * x;
        cubismParameter.Value = (Mathf.Sin(timecount)+1) * 0.5f;
    }
}
