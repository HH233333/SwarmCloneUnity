using Live2D.Cubism.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class AnimationValues_Shake : AnimationValues
{
    public int changeDrection = 1;
    public float waitTime2;

    public AnimationValues_Shake(List<int> action_indices, string name, float changeSpeed,  float waitTime,  float waitTime2, float waitTime_min, float waitTime_max, bool isPlaying, CubismModel model,int changeDrection)
    {
        this.changeDrection = changeDrection;
        InitValues(action_indices, name, changeSpeed, waitTime, waitTime_min, waitTime_max, isPlaying, model);
    }

    public override void StartAction()
    {
        if (isPlaying)
        {
            Check_WaitTime(waitTime);
            if (timer > 0)
            {
                Timer();
                return;
            }
            curr_value = Mathf.Clamp(curr_value + (0.01f * changeSpeed * changeDrection), min_value, max_value);
            if (curr_value <= min_value || curr_value >= max_value)
            {
                changeDrection *= -1;
                count++;
            }
            Update_ModelValues();

        }
        else if (curr_value > 0)
        {
            curr_value -= 0.01f * changeSpeed;
            if (curr_value<=0)
            {
                curr_value = 0;
            }
            Update_ModelValues();
        }
        
    }


}
