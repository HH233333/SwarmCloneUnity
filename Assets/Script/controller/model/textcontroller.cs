using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
public class textcontroller : MonoBehaviour
{
    private static textcontroller _instance;
    public static textcontroller instance => _instance;
    private TMP_Text chat_text;
    private float timecount;
    public float enter_timer;
    
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
    // Start is called before the first frame update
    void Start()
    {
        chat_text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timecount>0)
        {
            timecount -= Time.deltaTime;
            if (timecount<=0)
            {
                chat_text.text = "";
            }
        }
    }
    public IEnumerator PutText(ttsdata data)
        {
            Manager.instance.state.textcontroller_IsActivate = true;
            for(int j=0;j<data.Text.Count;j++)
            {
                timecount = enter_timer;
                chat_text.text += data.Text[j];
                yield return new WaitForSeconds(data.Duration[j]);
            }
            Manager.instance.state.textcontroller_IsActivate = false;
        }
}

