using Live2D.Cubism.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationValues_Change : AnimationValues
{
    float target_value;

    public AnimationValues_Change(List<int> action_indices, string name, float changeSpeed, float waitTime, float waitTime_min, float waitTime_max, bool isPlaying, CubismModel model)
    {
        InitValues(action_indices, name, changeSpeed, waitTime, waitTime_min, waitTime_max, isPlaying, model);
        target_value = UnityEngine.Random.Range(min_value, max_value);
    }

    public override void StartAction()
    {
        if (isPlaying)
        {
            if (timer > 0)
            {
                Timer();
                return;
            }
            curr_value = Mathf.Lerp(curr_value, target_value, changeSpeed);
            if (Mathf.Abs(curr_value - target_value) < 0.02f)
            {
                target_value = UnityEngine.Random.Range(min_value, max_value);
                Update_waitTime();
            }
            Update_ModelValues();
        }
    }

    protected override void Update_ModelValues()
    {

        model.Parameters[action_indices[0]].Value = curr_value;
        model.Parameters[action_indices[1]].Value = curr_value/3;

    }
}
