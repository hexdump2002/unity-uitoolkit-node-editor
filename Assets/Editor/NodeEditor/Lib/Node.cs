using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
 
public abstract partial class Node : VisualElement {
    public string ID {get;}
    public NodeSocket InputSockets { get; private set; }
    public NodeSocket OutputSockets { get; private set; }
    
    public Node()
    {
        style.width = 200;
        style.height = 150;
        style.backgroundColor = new StyleColor(Color.gray);

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeEditor/Lib/UI/Node.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeEditor/Lib/UI/Node.uss");
        this.styleSheets.Add(styleSheet);

        ID = Guid.NewGuid().ToString();
    }

    protected void AddSocket(NodeSocket socket) {
        if (socket.Type == NodeSocket.SocketType.Input)
            this.Q("Inputs").Add(socket);
        else
            this.Q("Outputs").Add(socket);
    }


}