using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Action_Resources : ScriptableObject
{
    [Header("�ö�������")]
    public string action_name;
    [Header("��ض�����live2D����")]
    public List<int> actionIndies;
    [Header("���������ٶ�")]
    public float action_speed = 10;
    [Header("�������ż��")]
    public float action_wait = 0;
    [Header("������ʼ�仯����")]
    public int action_direction = 1;
    [Header("�����Ƿ�ʼ����")]
    public bool isPlaying = true;

}
