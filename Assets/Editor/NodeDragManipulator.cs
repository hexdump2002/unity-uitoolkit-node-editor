using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;


public class NodeDragManipulator : PointerManipulator
{
    private Vector2 start;

    public NodeDragManipulator(VisualElement target)
    {
        this.target = target;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private bool IsChildElement(VisualElement element)
    {
        // Check if the element is a child that should not trigger the drag
        return element != null && element != target && target.Contains(element) && element is NodeSocket;
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        //Do not capture doble clicks
        if(evt.clickCount == 2) return;

        if (IsChildElement(evt.target as VisualElement))
        {
            // Handle child element interaction (e.g., start drawing a line)
            return;
        }


        start = evt.localPosition;
        target.CapturePointer(evt.pointerId);
        evt.StopPropagation();
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (target.HasPointerCapture(evt.pointerId))
        {
            Vector2 diff = new Vector2(evt.localPosition.x, evt.localPosition.y) - start;
            target.style.left = target.layout.x + diff.x;
            target.style.top = target.layout.y + diff.y;
            evt.StopPropagation();
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }
    }
}
