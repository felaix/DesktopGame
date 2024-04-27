#if UNITY_EDITOR
using UnityEngine;
using DS.Enumerations;
using UnityEditor.Experimental.GraphView;

namespace DS.Elements
{
    using DS.Data.Save;
    using DS.Windows;
    using Utilities;

    public class DSSingleChoiceNode : DSNode
    {


        public override void Draw()
        {
            base.Draw();

            // ! OUTPUT CONTAINER

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();

        }

        public override void Initialize(string nodeName, DSGraphView graphView, Vector2 position)
        {
            base.Initialize(nodeName, graphView, position);

            DialogueType = DSDialogueType.SingleChoice;

            DSChoiceSaveData choiceSaveData = new DSChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceSaveData);
        }


    }

}
#endif
