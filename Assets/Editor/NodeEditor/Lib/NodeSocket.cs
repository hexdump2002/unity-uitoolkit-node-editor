using System;
using UnityEngine;
using UnityEngine.UIElements;


[UxmlElement("NodeSocket")]
public partial class NodeSocket : VisualElement {
	public enum SocketType { Input, Output }
	private SocketType _type;
	public SocketType Type {
		get => _type;
		set {
			_type = value;
			style.backgroundColor = _type == SocketType.Input ? new StyleColor(Color.blue) : new StyleColor(Color.green);
		}
	}


public string ID { get; private set; }

	/*[UxmlAttribute]
	public SocketType Type {
		get => _type;
		set {
			_type = value;
			style.backgroundColor = _type == SocketType.Input ? new StyleColor(Color.blue) : new StyleColor(Color.green);
        }
	}*/

	private void _Initialization()
	{
        style.width = 16;
        style.height = 16;
        style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = new StyleLength(8);

		ID = Guid.NewGuid().ToString();
    }

	public NodeSocket()
	{
		_Initialization();
    }

	public NodeSocket(SocketType st)
	{
		Type = st;
		_Initialization();
	}
	/*
    public override bool Equals(object obj) {
        if (obj is NodeSocket other) {
            return ID == other.ID;
        }
        return false;
    }

    public override int GetHashCode() {
        return ID.GetHashCode();
    }*/

}