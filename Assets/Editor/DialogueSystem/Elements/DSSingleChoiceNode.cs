#if UNITY_EDITOR
using UnityEngine;
using DS.Enumerations;
using UnityEditor.Experimental.GraphView;

namespace DS.Elements
{
    using DS.Windows;
    using Utilities;

    public class DSSingleChoiceNode : DSNode
    {


        public override void Draw()
        {
            base.Draw();

            // ! OUTPUT CONTAINER

            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);

                choicePort.name = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();

        }

        public override void Initialize(DSGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = DSDialogueType.SingleChoice;

            Choices.Add("Next Dialogue");
        }


    }

}
#endif
