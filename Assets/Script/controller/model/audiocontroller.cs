using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiocontroller : MonoBehaviour
{
    private static audiocontroller _instance;
    public static audiocontroller instance => _instance;
    private AudioSource audioSource;

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
        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();

        // 检查 AudioSource 是否存在
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 未找到！");
            return;
        }
    }
    void Update()
    {
        if(audioSource.isPlaying)
            Manager.instance.state.audiocontroller_IsActivate = true;
        else
            Manager.instance.state.audiocontroller_IsActivate = false;
    } 

    public void PlayAudio(ttsdata data)
    {
        var audioClip = data.Audio;
        // 检查音频剪辑是否已设置
        if (audioClip == null)
        {
            Debug.LogError("AudioClip 为空!");
            return;
        }

        // 设置音频剪辑到 AudioSource
        audioSource.clip = audioClip;

        // 播放音频
        audioSource.Play();
    }

    public void Playsong()
    {
        if(audioSource.clip != null)
            audioSource.Play();
    }

    public void StopAudio()
    {
        // 停止播放音频
        audioSource.Stop();
    }

    public void PauseAudio()
    {
        // 暂停播放音频
        audioSource.Pause();
    }

    public void ResumeAudio()
    {
        // 恢复播放音频
        audioSource.UnPause();
    }
}
