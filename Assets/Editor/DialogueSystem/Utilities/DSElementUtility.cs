using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Utilities
{

    using Elements;
    using Enumerations;

    public static class DSElementUtility
    {

        public static Port CreatePort(this DSNode node, string portName="", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }

        public static Button CreateButton(string buttonText, Action onClick = null)
        {
            Button button = new Button(onClick) { text = buttonText };
            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout() { text = title, value = !collapsed };
            return foldout;
        }

        #region TextField
        public static TextField CreateTextField( string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged= null )
        {
            TextField textField = new TextField() { value=value, label=label };

            if (onValueChanged != null) { textField.RegisterValueChangedCallback(onValueChanged); }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null) 
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        #endregion


    }

}
