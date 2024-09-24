using NUnit.Framework.Internal.Filters;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeEditorWindow : EditorWindow
{
    [MenuItem("Window/Node Editor")]
    public static void ShowWindow()
    {
        NodeEditorWindow wnd = GetWindow<NodeEditorWindow>();
        wnd.titleContent = new GUIContent("Node Editor");
    }

    public void CreateGUI()
    {
        NodeEditor nodeEditor = new NodeEditor();
        rootVisualElement.style.flexGrow = 1;
        rootVisualElement.style.width = new StyleLength(Length.Percent(100));
        rootVisualElement.style.height = new StyleLength(Length.Percent(100));
        rootVisualElement.Add(nodeEditor);
    }



}
