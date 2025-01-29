using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chat_Manager : MonoBehaviour
{
    public TMP_Text chat_text;
    public TMP_InputField chat_input;
    public float text_wait_time=0.1f;

    public Model_Manager model_Manager;
    public float enter_timer;
    

    public void UpdateInputField_Enter()
    {
        SetSomeText(chat_input.text);
        print("文本输入");
    }
    private void Update()
    {
        if (enter_timer>0)
        {
            enter_timer -= Time.deltaTime;
            if (enter_timer<=0)
            {
                chat_text.text = "";
            }
        }
    }
    public void SetSomeText(string text)
    {
        StartCoroutine(Model_talking(text));
        chat_input.text = "";
    }
    IEnumerator Model_talking(string text)
    {
        foreach (var item in text)
        {
            chat_text.text += item;

            model_Manager.Is_talking = true;

            enter_timer = 5;
            yield return new WaitForSeconds(text_wait_time);
        }
        model_Manager.Is_talking = false;
        yield break;
    }
}
