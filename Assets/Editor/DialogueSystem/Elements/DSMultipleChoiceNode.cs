#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using DS.Data;
    using DS.Data.Save;
    using DS.Windows;
    using Enumerations;
    using Utilities;

    public class DSMultipleChoiceNode : DSNode
    {
        public override void Draw()
        {
            base.Draw();

            Button addChoiceBtn = DSElementUtility.CreateButton("Add Choice", () =>
            {

                DSChoiceSaveData choiceData = CreateChoiceData();

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            mainContainer.Insert(1, addChoiceBtn);

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                //Choices.Add("New Choice");

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        public override void Initialize(string nodeName, DSGraphView graphView, Vector2 position)
        {
            base.Initialize(nodeName, graphView, position);

            DialogueType = DSDialogueType.MultipleChoice;

            DSChoiceSaveData choiceData = CreateChoiceData();
            Choices.Add(choiceData);
        }

        #region Element Creation

        private DSChoiceSaveData CreateChoiceData() { return new DSChoiceSaveData() { Text = "New Choice" }; }

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            DSChoiceSaveData choiceData = (DSChoiceSaveData) userData; 

            Button deleteChoiceBtn = DSElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });
            TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceBtn);
            return choicePort;
        }
        #endregion

    }

}
#endif