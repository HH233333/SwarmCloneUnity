using Live2D.Cubism.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Model_Manager : MonoBehaviour
{
    // 引用Live2D模型组件
    private CubismModel model;

    //观看方向
    [Header("观看方向")]
    public Vector2 look_pos;
    private Vector2 curr_pos, target_pos;
    bool isLooking;



    //说话
    [Header("说话")]
    public Action_Resources action_talk;
    public bool is_talking;

    public Action_Resources[] actions;
    // 字典用于存储协程的唯一标识符和协程引用
    public Dictionary<string, AnimationValues> coroutineDictionary = new Dictionary<string, AnimationValues>();
    [Header("用于动态修改各个动作属性")]
    public List<AnimationValues> corou_List;

    public List<string> test;

    public bool Is_talking
    {
        get => is_talking;
        set
        {
            is_talking = value;
            if (is_talking)
            {
                coroutineDictionary[action_talk.action_name].isPlaying = true;
            }
            else
            {
                coroutineDictionary[action_talk.action_name].isPlaying = false;
                StartCoroutine(ResetCoroutine(action_talk.action_name));
            }
        }
    }
    private void Start()
    {
        model = GameObject.FindGameObjectWithTag("Live2D_Model").GetComponent<CubismModel>();

        Init();
        foreach (var item in model.Parameters)
        {
            test.Add(item.name);
        }
    }
    private void Init()
    {
        actions = Resources.LoadAll<Action_Resources>("Actions");
        InitActions(actions);
    }
    private void InitActions(Action_Resources[] actions)
    {
        foreach (var item in actions)
        {
            if (item.actionIndies.Count < 1)
            {
                continue;
            }
            if (item.action_name == "说话")
            {
                action_talk = item;
            }
            StartCoroutine_Animation(item.actionIndies, item.action_name, item.action_speed, item.action_wait, item.action_direction, item.isPlaying);
        }
    }


    private void StartCoroutine_Animation(List<int> valueIndex, string id_name, float changeSpeed, float wait_time, int dirction, bool isPlaying)
    {
        AnimationValues Values;
        if (!coroutineDictionary.TryGetValue(id_name, out Values))
        {
            //开启协程传入ID，ID绑定相应的 AnimationValues;
            Coroutine coroutine = StartCoroutine(Fixed_Up_Down(id_name, isPlaying, dirction));
            Values = new AnimationValues(coroutine, valueIndex, id_name, changeSpeed, wait_time, isPlaying);
            coroutineDictionary[id_name] = Values;
            corou_List.Add(Values);
        }
    }

    IEnumerator ResetCoroutine(string id_name)
    {
        AnimationValues values = coroutineDictionary[id_name];
        List<int> curr_indices = values.Action_indices;
        float curr_value = model.Parameters[curr_indices[0]].Value;
        float min_value = model.Parameters[curr_indices[0]].MinimumValue;
        while (curr_value > min_value)
        {
            curr_value -= 0.025f * values.changeSpeed;
            UpdateAnimationValues(values.Action_indices, curr_value);
            yield return new WaitForSeconds(0.05f);
        }
        UpdateAnimationValues(curr_indices, min_value);
        yield break;
    }




    //固定地升降
    IEnumerator Fixed_Up_Down(string id_name, bool isPlaying, int direction)
    {
        AnimationValues values;
        while (!(coroutineDictionary.TryGetValue(id_name, out values) && coroutineDictionary[id_name] != null))
        {
            yield return new WaitForSeconds(0.005f);
        }
        List<int> valueIndex = values.Action_indices;

        int curr_direction = direction;
        float curr_Value = model.Parameters[valueIndex[0]].Value;
        float max_Value = model.Parameters[valueIndex[0]].MaximumValue;
        float min_Value = model.Parameters[valueIndex[0]].MinimumValue;

        float waitTime, changeSpeed;

        int count = 0;
        while (true)
        {
            UpdateValues(id_name, out waitTime, out changeSpeed,out isPlaying);
            if (!isPlaying)
            {
                yield return new WaitForSeconds(0.05f);
                continue;
            }
            
            if (count >= 2)
            {
                yield return new WaitForSeconds(waitTime);
                count = 0;
            }

            curr_Value = Mathf.Clamp(curr_Value + 0.01f * changeSpeed * curr_direction, min_Value, max_Value);
            if (curr_Value >= max_Value || curr_Value <= min_Value)
            {
                curr_direction *= -1;
                if (waitTime > 0) count++;

            }

            UpdateAnimationValues(valueIndex, curr_Value);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void UpdateAnimationValues(List<int> valueIndex, float value)
    {
        //设置数值
        if (valueIndex.Count == 1)
        {
            model.Parameters[valueIndex[0]].Value = value;
        }
        else
        {
            foreach (var item in valueIndex)
            {
                model.Parameters[item].Value = value;
            }
        }
    }
    private void UpdateValues(string id_name, out float curr_waitTime, out float changeSpeed,out bool isPlaying)
    {
        AnimationValues values = coroutineDictionary[id_name];
        curr_waitTime = values.waitTime;
        changeSpeed = values.changeSpeed;
        isPlaying = values.isPlaying;
    }
}
