using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class rightclickManager : MonoBehaviour
{
    private static rightclickManager _instance;
    public static rightclickManager instance => _instance;

    public GameObject rightClickMenu; // 菜单背景 Panel
    public danmakuvisible danmakuvisible;
    public modelmovebutton modelmovebutton;
    public singmode singmode;
    
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
    void Start()
    {
        rightClickMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0.0f; // 设置一个合适的 Z 值，根据你的 UI 深度设置
            RectTransform rectTransform = rightClickMenu.GetComponent<RectTransform>();
            mousePosition.x += rectTransform.sizeDelta.x/2;
            mousePosition.y -=  rectTransform.sizeDelta.y/2;
            // 将菜单位置设置为鼠标位置
            rightClickMenu.transform.position = mousePosition;

            // 显示菜单
            rightClickMenu.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 鼠标点击位置
            Vector2 mouseClickPosition = Input.mousePosition;

            // 检查点击是否在菜单范围内
            if (!IsPointerOverUIObject(mouseClickPosition))
                // 关闭菜单
                rightClickMenu.SetActive(false);
        }
    }
    bool IsPointerOverUIObject(Vector2 clickPosition)
    {
        // 创建一个射线
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(clickPosition.x, clickPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // 检查是否有 UI 对象被点击
        for (int i = 0; i < results.Count; i++)
        {
            // 如果点击的对象是菜单背景或其子对象（菜单项）
            if (results[i].gameObject == rightClickMenu || results[i].gameObject.transform.IsChildOf(rightClickMenu.transform))
                return true;
        }
        return false;
    }

}
