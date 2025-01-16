using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationValues

{
    public Coroutine coroutine;
    [DisplayOnly]
    public List<int> Action_indices;
    [Header("Ãû³Æ")]
    [DisplayOnly]
    public string name;

    public float changeSpeed;
    public float waitTime;
    public bool isPlaying;

    public AnimationValues(Coroutine coroutine, List<int> action_indices, string name, float changeSpeed, float waitTime, bool isPlaying)
    {
        this.coroutine = coroutine;
        Action_indices = action_indices;
        this.name = name;
        this.changeSpeed = changeSpeed;
        this.waitTime = waitTime;
        this.isPlaying = isPlaying;
    }
}
