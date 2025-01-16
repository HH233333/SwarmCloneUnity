// 引入UnityEditor命名空间，这个命名空间包含了Unity编辑器的扩展功能。
using UnityEditor;
using UnityEngine;

/// <summary>
/// 设置属性只读
/// </summary>
// 定义一个自定义的属性类DisplayOnly，用于标记某个属性为只读。
public class DisplayOnly : PropertyAttribute { }

// 使用CustomPropertyDrawer特性来指定这个类是为DisplayOnly属性绘制的自定义抽屉（Drawer）。
[CustomPropertyDrawer(typeof(DisplayOnly))]
// 定义一个名为ReadOnlyDrawer的类，它继承自PropertyDrawer，用于自定义属性的绘制方式。
public class ReadOnlyDrawer : PropertyDrawer
{
    // 重写GetPropertyHeight方法，用于获取属性在编辑器中的高度。
    // 这里直接调用EditorGUI.GetPropertyHeight方法，传入属性、标签和是否包括子属性的参数，返回计算得到的高度。
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // 重写OnGUI方法，用于在编辑器中绘制属性。
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 禁用GUI，使得接下来的绘制操作（即属性字段）变为不可交互（只读）。
        GUI.enabled = false;

        // 使用EditorGUI.PropertyField方法绘制属性字段，传入位置、属性、标签和是否包括子属性的参数。
        EditorGUI.PropertyField(position, property, label, true);

        // 重新启用GUI，恢复其交互性，以免影响后续绘制操作。
        GUI.enabled = true;
    }
}
