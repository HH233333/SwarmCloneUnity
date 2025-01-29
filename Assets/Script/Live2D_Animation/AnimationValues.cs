using Live2D.Cubism.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AnimationValues
{
    [Header("动作相关Live2D索引")]
    public List<int> action_indices;
    [Header("名称")]
    public string name;
    [Header("数值变化速度")]
    public float changeSpeed;
    [Header("等待时间和其重置范围")]
    public float waitTime;
    public float waitTime_min;
    public float waitTime_max;
    [Header("是否需要播放")]
    public bool isPlaying;
    protected float timer = 0;

    public float curr_value, min_value, max_value;
    public int count = 0;
    protected CubismModel model;

    public virtual void InitValues(List<int> action_indices, string name, float changeSpeed, float waitTime, float waitTime_min, float waitTime_max, bool isPlaying, CubismModel model)
    {
        this.action_indices = action_indices;
        this.name = name;
        this.changeSpeed = changeSpeed;
        this.waitTime = waitTime;
        this.waitTime_min = waitTime_min;
        this.waitTime_max = waitTime_max;
        this.isPlaying = isPlaying;
        this.model = model;

        this.curr_value = model.Parameters[action_indices[0]].Value;
        this.min_value = model.Parameters[action_indices[0]].MinimumValue;
        this.max_value = model.Parameters[action_indices[0]].MaximumValue;

    }
    public virtual void StartAction()
    {

    }
    protected virtual void Check_WaitTime(float waitTime = 0, float waitTime2 = 0)
    {
        if (count == 1 && waitTime2 > 0)
        {
            Update_waitTime();
        }
        else if (count == 2 && waitTime > 0)
        {
            Update_waitTime();
            count = 0;
        }
    }
    protected void Update_waitTime()
    {
        if (waitTime_min > 0 && waitTime_max > 0 && waitTime_max - waitTime_min > 0)
        {
            waitTime = UnityEngine.Random.Range(waitTime_min, waitTime_max);
        }
        timer = waitTime;

    }
    protected virtual void Update_ModelValues()
    {
        foreach (var item in action_indices)
        {
            model.Parameters[item].Value = curr_value;
        }
    }

    protected void Timer()
    {
        timer -= Time.deltaTime;
    }
}
