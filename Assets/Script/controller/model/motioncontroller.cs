using UnityEngine;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Expression;
using System.Collections.Generic;


public class motioncontroller : MonoBehaviour
{
    public bool HasUpdateController { get; set; }
    public bool NeedsUpdateOnEditing
    {
        get { return false; }
    }
    public int ExecutionOrder
    {
        get { return 50; }
    }
    public List<int> emotionduration;

    private int framecount = 0;
    private static motioncontroller _instance;
    public static motioncontroller instance => _instance;
    private CubismExpressionController expressionController;
    private CubismEyeBlinkController eyesController;

    private string motion;
    public string Motion
    {
        get => motion;
        set
        {
            motion = value;
            Debug.Log(motion);
            if (expressionController.CurrentExpressionIndex == 0)
            {
                Manager.instance.state.motioncontroller_IsActivate = true;
                switch (motion)
                {
                    case "like":
                        expressionController.CurrentExpressionIndex = 1;
                        framecount += emotionduration[0];
                        break;
                    case "disgust":
                        expressionController.CurrentExpressionIndex = 2;
                        framecount += emotionduration[1];
                        break;
                    case "anger":
                        expressionController.CurrentExpressionIndex = 3;
                        framecount += emotionduration[2];
                        break;
                    case "happy":
                        expressionController.CurrentExpressionIndex = 4;
                        framecount += emotionduration[3];
                        break;
                    case "sad":
                        expressionController.CurrentExpressionIndex = 5;
                        framecount += emotionduration[4];
                        break;
                    case "neutral":
                        break;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        expressionController = GetComponent<CubismExpressionController>();
        eyesController = GetComponent<CubismEyeBlinkController>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (framecount > 0)
            framecount--;
        else if (expressionController.CurrentExpressionIndex != 0)
            expressionController.CurrentExpressionIndex = 0;
            
        if (expressionController.CurrentExpressionIndex == 0)
        {
            eyesController.enabled = true;
            Manager.instance.state.motioncontroller_IsActivate = false;
        }
        else
        {
            eyesController.EyeOpening = 1.0f;
            eyesController.enabled = false;
        }
    }

}
