using UnityEngine.UIElements;

[UxmlElement("EditableLabel")]
public partial class EditableLabel : VisualElement
{
    private Label label;
    private TextField textField;

    public EditableLabel()
    {
        label = new Label("Double-click to edit");
        textField = new TextField();
        textField.style.display = DisplayStyle.None;

        label.RegisterCallback<ClickEvent>(evt =>
        {
            if (evt.clickCount == 2)
            {
                label.style.display = DisplayStyle.None;
                textField.style.display = DisplayStyle.Flex;
                textField.value = label.text;
                textField.Focus();
            }
        });

        textField.RegisterCallback<FocusOutEvent>(evt =>
        {
            label.text = textField.value;
            label.style.display = DisplayStyle.Flex;
            textField.style.display = DisplayStyle.None;
        });

        Add(label);
        Add(textField);
    }
}