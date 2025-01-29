using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ShakeAction_Resources : Action_Resources
{
    public float waitTime2 = 0;
    [Header("动作初始变化方向")]
    public int action_direction = 1;
}
