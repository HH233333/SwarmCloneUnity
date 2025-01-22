using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Action_Resources : ScriptableObject
{
    [Header("该动作名称")]
    public string action_name;
    [Header("相关动作的live2D索引")]
    public List<int> actionIndies;
    [Header("动作播放速度")]
    public float action_speed = 10;
    [Header("动作播放间隔")]
    public float action_wait = 0;
    [Header("动作初始变化方向")]
    public int action_direction = 1;
    [Header("动作是否开始播放")]
    public bool isPlaying = true;

}
