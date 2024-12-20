#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using DS.Data.Save;
    using DS.Windows;
    using Enumerations;
    using System;
    using System.Linq;
    using Utilities;


    public class DSNode : Node
    {

        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public Group Group { get; set; }

        protected DSGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            DialogueName = nodeName;
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialogue Text.";

            graphView = dsGraphView;

            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual void Draw()
        {

            // ! TITLE CONTAINER

            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {

                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.RepeatedNamesAmount;
                    }
                }else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.RepeatedNamesAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = callback.newValue;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                Group currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                DialogueName = callback.newValue;

                graphView.AddGroupedNode(this, currentGroup);
            });
            
            titleContainer.Insert(0,dialogueNameTextField);

            // ! INPUT CONTAINER

            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputPort.style.flexDirection = FlexDirection.Row;

            inputContainer.Add(inputPort);

            // ! EXTENSIONS CONTAINER

            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = DSElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });
            textTextField.style.flexDirection = FlexDirection.Column; 

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();


        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().FirstOrDefault();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }

}
#endif