using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DS.Elements
{

    using Enumerations;
    using UnityEngine.UIElements;

    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public void Initialize()
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue Text.";
        }

        public void Draw()
        {

            // ! TITLE CONTAINER

            TextField dialogueNameTextField = new TextField()
            {
                value = DialogueName
            };

            titleContainer.Insert(0,dialogueNameTextField);

            // ! INPUT CONTAINER

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(string));
            inputPort.portName = "Input";

            inputContainer.Add(inputPort);

            // ! EXTENSIONS CONTAINER

            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"
            };

            TextField textTextField = new TextField() { value = Text };

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    }

}
