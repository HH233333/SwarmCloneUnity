using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Core;
public class positioncontroller : MonoBehaviour
{
    private CubismModel model;
    private CubismRaycaster raycaster;
    private CubismRaycastHit[] cubismRaycastHits;
    private Vector3 offset;// 偏移量    
    private bool isDragging = false; // 是否正在拖拽

    public float zoomSpeed = 1.0f; // 缩放速度
    public float minScale = 0.1f; // 最小缩放比例
    public float maxScale = 200.0f; // 最大缩放比例
    void Start()
    {
        
        model  = GetComponent<CubismModel>();
        raycaster = GetComponent<CubismRaycaster>();
        cubismRaycastHits = new CubismRaycastHit[4];
        enabled=false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 将鼠标位置转换为射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 检测射线与物体的碰撞
            int hitCount = raycaster.Raycast(ray,cubismRaycastHits);
            // 如果点击到了要拖拽的物体，则开始拖拽
            if (hitCount > 0)
                {
                    isDragging = true;
                    offset = model.transform.position - Input.mousePosition;
                }
        }
        // 检测鼠标左键松开事件
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 如果正在拖拽，则更新物体的位置
        if (isDragging)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel"); // 获取鼠标滚轮的滚动值
            Vector3 scale = transform.localScale; // 获取物体的当前缩放比例
            // 根据滚轮的滚动值来计算新的缩放比例
            float newSize = Mathf.Clamp(scale.x - (scroll * zoomSpeed), minScale, maxScale);
            transform.localScale = new Vector3(newSize, newSize, newSize);
            model.transform.position = new Vector3(Input.mousePosition.x+offset.x, Input.mousePosition.y+offset.y, 0.0f);
        }
    }
}
