#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
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
                Port choicePort = CreateChoicePort("New Choice");

                Choices.Add("New Choice");

                outputContainer.Add(choicePort);
            });

            mainContainer.Insert(1, addChoiceBtn);

            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort("New Choice");

                //Choices.Add("New Choice");

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        public override void Initialize(DSGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = DSDialogueType.MultipleChoice;

            Choices.Add("New Choice");
        }

        #region Element Creation
        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();

            choicePort.name = "";

            Button deleteChoiceBtn = DSElementUtility.CreateButton("X");
            TextField choiceTextField = DSElementUtility.CreateTextField(choice);

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceBtn);
            return choicePort;
        }
        #endregion

    }

}
#endif