using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gravity3DText : MonoBehaviour
{
    public Canvas canvas;
    public GameObject gravity3DTextPrefab;
    private Text textMesh;
    private List<GameObject> tempTextList;

    public List<Vector2> charDataList;
    public Vector3 offset;
    public Vector3 targetPos;

    private void Awake()
    {
        Find();
    }
    void Start()
    {
        Init();

    }
    void Find()
    {
        tempTextList = new List<GameObject>();
        //获取UI文本组件 
        textMesh = GetComponent<Text>();
        offset = targetPos - transform.position;
    }

    void Init()
    {
        //调用textMesh（Text组件）的GraphicUpdateComplete方法。
        textMesh.GraphicUpdateComplete();
    }
    public void Update()
    {
        //按下空格
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //获取文本中的数据
            string textStr = textMesh.text;
            //获取文本的长度
            int textLength = textStr.Length;

            TextGenerator textGenerator = new TextGenerator();
            // 获取Text组件的RectTransform的大小
            Vector2 size = textMesh.rectTransform.rect.size;
            // 使用TextGenerator.Populate方法填充文本数据
            textGenerator.Populate(textStr, textMesh.GetGenerationSettings(size));

            // 尝试获取TextGenerator生成的顶点列表
            // 注意：TextGenerator.verts返回的是一个UIVertex数组，而不是List<UIVertex>
            List<UIVertex> vertexList = textGenerator.verts as List<UIVertex>;

            // 遍历顶点列表（注意：这里假设vertexList已经被正确填充）
            for (int i = 0; i <= vertexList.Count - 4; i += 4)
            {


                // 计算四个顶点的平均位置作为字符的中心位置
                Vector3 pos1 = vertexList[i].position;
                Vector3 pos2 = vertexList[i + 1].position;
                Vector3 pos3 = vertexList[i + 2].position;
                Vector3 pos4 = vertexList[i + 3].position;
                Vector3 pos = (pos1 + pos2 + pos3 + pos4) / 4.0f;

                // 调整位置以考虑Canvas的缩放因子
                // 注意：这里假设canvas是一个已知的Canvas组件，并且已经正确设置了scaleFactor
                pos /= canvas.scaleFactor;
                // 将CharData对象添加到列表中
                charDataList.Add(pos);
            }
            float sameY = transform.localPosition.y;

            // 移除文本中的所有空格（这可能不是您想要的行为，因为它会改变文本的内容）
            string trimStr = textStr.Replace(" ", "");
            Debug.Log("trimStr:" + trimStr);

            // 遍历字符数据列表，实例化每个字符
            for (int i = 0; i < trimStr.Length; i++)
            {
                Vector2 textPos = charDataList[i];

                // 将字符位置转换到世界坐标
                Vector3 pos = transform.TransformPoint(textPos);

                // 将所有字符的y位置设置为相同的值（对齐到同一高度）
                pos.y = sameY;
                Debug.Log($"第{i + 1}个字,中心点:{pos}, char:{trimStr[i]}");

                // 实例化预制体（假设gravity3DTextPrefab是一个预制体变量）
                GameObject tempTextGo = GameObject.Instantiate(gravity3DTextPrefab, pos, Quaternion.identity);

                // 获取TextMesh组件并设置文本、字体大小和字符大小
                TextMesh tempTextMesh = tempTextGo.GetComponent<TextMesh>();
                tempTextMesh.text = trimStr[i].ToString();
                tempTextMesh.fontSize = textMesh.fontSize * 5; // 这里乘以5可能是为了放大字体

                // 将实例化对象添加到列表中（假设tempTextList是一个用于存储GameObject的列表）
                tempTextList.Add(tempTextGo);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var v in tempTextList)
        {
            GameObject.Destroy(v);
        }
        tempTextList.Clear();
    }

}