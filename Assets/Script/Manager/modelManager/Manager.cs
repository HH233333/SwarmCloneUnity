using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using Live2D.Cubism.Core;

public enum modelstatu
{
    talking = 1,
    singing = 2
}
public class State
{
    public bool motioncontroller_IsActivate = false;
    public bool textcontroller_IsActivate = false;
    public bool audiocontroller_IsActivate = false;
    public modelstatu modelstatu;
}

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager instance => _instance;

    public tts_queue tts_Queue;
    public sing_queue sing_Queue;
    public danmaku_queue danmaku_Queue;
    public request_socket request_Socket;

    public State state = new State();
    public GameObject audiovisbleupanel;
    public modelstatu startstatu;
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

        state.modelstatu = startstatu;
    }
    private void Start()
    {
        
    }


    private void Update()
    {
        if (state.modelstatu == modelstatu.talking || !tts_Queue.IsEmpty)
        {
            Model_talking();
            audiovisbleupanel.SetActive(false);
        }
        else if (state.modelstatu == modelstatu.singing)
        {
            Model_singing();
            audiovisbleupanel.SetActive(true);
        }
    }

    private void Model_talking()
    {
        if (state.motioncontroller_IsActivate || state.textcontroller_IsActivate || state.audiocontroller_IsActivate)
            return;
        if (tts_Queue.PutTtsData(out ttsdata data))
        {
            motioncontroller.instance.Motion = data.emotion;
            StartCoroutine(textcontroller.instance.PutText(data));
            audiocontroller.instance.PlayAudio(data);
            if (data.isend)
                request_Socket.SendDAudioFinished();
        }
    }

    private void Model_singing()
    {
        if (state.motioncontroller_IsActivate || state.textcontroller_IsActivate || state.audiocontroller_IsActivate)
            return;
        if (sing_Queue.PutSingData(out singdata data))
        {
            StartCoroutine(textcontroller.instance.Putsubtitle(data));
            audiocontroller.instance.PlayAudio(data);
        }
        return;   
    }
}
