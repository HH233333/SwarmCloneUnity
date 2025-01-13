// ����UnityEditor�����ռ䣬��������ռ������Unity�༭������չ���ܡ�
using UnityEditor;
using UnityEngine;

/// <summary>
/// ��������ֻ��
/// </summary>
// ����һ���Զ����������DisplayOnly�����ڱ��ĳ������Ϊֻ����
public class DisplayOnly : PropertyAttribute { }

// ʹ��CustomPropertyDrawer������ָ���������ΪDisplayOnly���Ի��Ƶ��Զ�����루Drawer����
[CustomPropertyDrawer(typeof(DisplayOnly))]
// ����һ����ΪReadOnlyDrawer���࣬���̳���PropertyDrawer�������Զ������ԵĻ��Ʒ�ʽ��
public class ReadOnlyDrawer : PropertyDrawer
{
    // ��дGetPropertyHeight���������ڻ�ȡ�����ڱ༭���еĸ߶ȡ�
    // ����ֱ�ӵ���EditorGUI.GetPropertyHeight�������������ԡ���ǩ���Ƿ���������ԵĲ��������ؼ���õ��ĸ߶ȡ�
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // ��дOnGUI�����������ڱ༭���л������ԡ�
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // ����GUI��ʹ�ý������Ļ��Ʋ������������ֶΣ���Ϊ���ɽ�����ֻ������
        GUI.enabled = false;

        // ʹ��EditorGUI.PropertyField�������������ֶΣ�����λ�á����ԡ���ǩ���Ƿ���������ԵĲ�����
        EditorGUI.PropertyField(position, property, label, true);

        // ��������GUI���ָ��佻���ԣ�����Ӱ��������Ʋ�����
        GUI.enabled = true;
    }
}
