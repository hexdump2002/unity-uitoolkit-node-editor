using System.Collections.Generic;
using System.Net;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class NodeConnection {
    NodeSocket srcNodeSocket;
    NodeSocket nodeSocketTarget;
}


[UxmlElement("NodeEditor")]
public partial class NodeEditor : VisualElement
{
    private VisualElement _nodesLayer;
    private VisualElement _connectionsLayer;
    enum NodeEditorState { Idle,ConnectingNodes, DraggingNodes}
    private List<Node> nodes = new List<Node>();
    private Dictionary<NodeSocket, List<NodeSocket>> _outputNodeSocketConnections = new Dictionary<NodeSocket, List<NodeSocket>>();
    private NodeEditorState _state = NodeEditorState.Idle;
    private Node _node = null;
    private NodeSocket _connectingStartingNodeSocket = null;
    private Vector2 _currentMousePos = Vector2.zero;

    public NodeEditor()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/NodeEditor/Lib/UI/NodeEditor.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeEditor/Lib/UI//NodeEditor.uss");
        this.styleSheets.Add(styleSheet);

        //Why do we need this if it comes from the stylesheet?
        this.style.flexGrow=1;

        // Attach the contextual menu manipulator
        this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
       

        //generateVisualContent += OnGenerateVisualContent;

        _nodesLayer = this.Q<VisualElement>("NodesLayer");

        _nodesLayer.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        _nodesLayer.RegisterCallback<MouseDownEvent>(OnMouseDown);
        _nodesLayer.RegisterCallback<MouseUpEvent>(OnMouseUp);
        
        _connectionsLayer = this.Q<VisualElement>("ConnectionsLayer");
        _connectionsLayer.generateVisualContent += OnGenerateVisualContent;
    }

    private void _AddNodeConnection(NodeSocket srcSocket, NodeSocket dstSocket) {
        List<NodeSocket> outputConnections;
        
        if (_outputNodeSocketConnections.TryGetValue(srcSocket, out outputConnections)) {
            outputConnections.Add(dstSocket);
        }
        else {
            _outputNodeSocketConnections.Add(srcSocket, new List<NodeSocket> {dstSocket });
        }
       
    }

    private bool _IsNodeSocket(VisualElement ve) {
        return ve.GetType() == typeof(NodeSocket);
    }

    private bool _IsNode(VisualElement ve, out Node node) {
        node = VisualElementExtensions.GeFirstObjectTypeUp<Node>(ve);
        return node != null;
    }

    private void OnMouseMove(MouseMoveEvent evt) {
        if (_state == NodeEditorState.ConnectingNodes) _HandleOnMouseMoveForConnectingNodesState(evt);
        else if (_state == NodeEditorState.DraggingNodes) _HandleOnMouseMoveForDraggingNodeState(evt);
    }

    private void OnMouseUp(MouseUpEvent evt) {
        if (_state == NodeEditorState.Idle) _HandleOnMouseUpForIdleState(evt);
        else if (_state == NodeEditorState.ConnectingNodes) _HandleOnMouseUpForConnectingNodesState(evt);
        else if (_state == NodeEditorState.DraggingNodes) _HandleOnMouseUpForDraggingNodeState(evt);
    }

    private void OnMouseDown(MouseDownEvent evt) {
        if (_state == NodeEditorState.Idle) _HandleOnMouseDownForIddleState(evt);
    }



    #region Event Handling
    private void _HandleOnMouseMoveForConnectingNodesState(MouseMoveEvent evt) {
        _currentMousePos = evt.localMousePosition;
        MarkDirtyRepaint();
        
        _nodesLayer.MarkDirtyRepaint();
        _connectionsLayer.MarkDirtyRepaint();
    }
    private void _HandleOnMouseUpForConnectingNodesState(MouseUpEvent evt) {
        VisualElement ve = evt.target as VisualElement;
        if (_IsNodeSocket(ve)) {
            _AddNodeConnection(_connectingStartingNodeSocket, ve as NodeSocket);
        }

        _state = NodeEditorState.Idle;

        MarkDirtyRepaint();
    }
    private void _HandleOnMouseDownForConnectingNodesState(MouseDownEvent evt) {
        _currentMousePos = evt.localMousePosition;
        MarkDirtyRepaint();
    }

    private void _HandleOnMouseMoveForIdleState(MouseMoveEvent evt) {
        
    }
    private void _HandleOnMouseUpForIdleState(MouseUpEvent evt) {
       
    }
    private void _HandleOnMouseDownForIddleState(MouseDownEvent evt) {
        Debug.Log("handle Mouse down");
        Node node;
        if (_IsNodeSocket(evt.target as VisualElement)) {
            _connectingStartingNodeSocket = evt.target as NodeSocket;
            _currentMousePos = evt.localMousePosition;
            _state = NodeEditorState.ConnectingNodes;
            MarkDirtyRepaint();
        }
        else if (_IsNode(evt.target as VisualElement, out node)) {
            _state = NodeEditorState.DraggingNodes;
            _node = node;
            MarkDirtyRepaint();
        }
    }

    private void _HandleOnMouseMoveForDraggingNodeState(MouseMoveEvent evt) {
        if (_state==NodeEditorState.DraggingNodes) {
            Vector2 mousePos = evt.mousePosition;
            Vector2 diff = new Vector2(mousePos.x, mousePos.y);
            _node.style.left = mousePos.x;
            _node.style.top = mousePos.y;
            evt.StopPropagation();
            _connectionsLayer.MarkDirtyRepaint();
        }
    }
    private void _HandleOnMouseUpForDraggingNodeState(MouseUpEvent evt) {
        _state = NodeEditorState.Idle;
        MarkDirtyRepaint();
    }
    private void _HandleOnMouseDownForDraggingNodeState(MouseDownEvent evt) {
       
    }
    #endregion

    public void OnGenerateVisualContent(MeshGenerationContext mgc) {
        var painter = mgc.painter2D;
        Debug.Log("Draw curves");
        // Draw all stored curves
        foreach(var nodeConnection in _outputNodeSocketConnections) {
            NodeSocket outputConnection = nodeConnection.Key;
            List<NodeSocket> inputConnections = nodeConnection.Value;
            var outputSocketPosition = new Vector2(outputConnection.worldBound.xMin + outputConnection.worldBound.width / 2, outputConnection.worldBound.yMin - outputConnection.worldBound.height);
            foreach (NodeSocket inputConnection in inputConnections) {
                var inputSocketPosition = new Vector2(inputConnection.worldBound.xMin + inputConnection.worldBound.width / 2, inputConnection.worldBound.yMin - inputConnection.worldBound.height);
                painter.BeginPath();
                painter.strokeColor = Color.yellow;
                painter.lineWidth = 5;
                painter.MoveTo(outputSocketPosition);
                painter.BezierCurveTo(
                    new Vector2((outputSocketPosition.x + inputSocketPosition.x) / 2, outputSocketPosition.y),
                    new Vector2((outputSocketPosition.x + inputSocketPosition.x) / 2, outputSocketPosition.y),
                    inputSocketPosition
                );
                painter.Stroke();
            }
        }

        // Draw the current moving curve
        if (_state == NodeEditorState.ConnectingNodes) {
            //TODO: Why yMin - height. It should be yMin + height/2
            Vector2 startingSocketPosition = new Vector2(_connectingStartingNodeSocket.worldBound.xMin + _connectingStartingNodeSocket.worldBound.width / 2, _connectingStartingNodeSocket.worldBound.yMin - _connectingStartingNodeSocket.worldBound.height );
            painter.BeginPath();
            painter.lineWidth = 5;
            painter.strokeColor = Color.yellow;
            painter.MoveTo(startingSocketPosition);
            painter.BezierCurveTo(
                new Vector2((startingSocketPosition.x + _currentMousePos.x) / 2, startingSocketPosition.y),
                new Vector2((startingSocketPosition.x + _currentMousePos.x) / 2, startingSocketPosition.y),
                _currentMousePos
            );
            painter.Stroke();
        }
    }

    private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        Debug.Log("Adding menu items");
        // Add menu items
        evt.menu.AppendAction("Create Node", Action1);
        evt.menu.AppendAction("Delete Node", Action2);
    }

    private void Action1(DropdownMenuAction action)
    {
        RoomNode node = new RoomNode();
        node.style.position = Position.Absolute;
        node.style.left = action.eventInfo.mousePosition.x;
        node.style.top = action.eventInfo.mousePosition.y;
        _nodesLayer.Add(node);
        nodes.Add(node);
   }

    private void Action2(DropdownMenuAction action)
    {
        Debug.Log("Action 2 selected");
    }


}