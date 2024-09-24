using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement("RoomNode")]
public partial class RoomNode : Node {

    public RoomNode() {
        AddSocket(new NodeSocket(NodeSocket.SocketType.Input));
        AddSocket(new NodeSocket(NodeSocket.SocketType.Output));
    }

}