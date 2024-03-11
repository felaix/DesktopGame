using DS.Elements;
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;


namespace DS.Windows
{
    using Elements;
    using Enumerations;

    public class DSGraphView : GraphView
    {
        public DSGraphView() {
            AddManipulators();
            AddGridBackground();
            //CreateNode();
            AddStyles();
        }

        private DSNode CreateNode(Vector2 position, DSDialogueType type)
        {

            DSNode node;

            switch (type)
            {
                case DSDialogueType.SingleChoice:
                    node = new DSSingleChoiceNode();
                    break;
                case DSDialogueType.MultipleChoice:
                    node = new DSMultipleChoiceNode();
                    break;
                default:
                    node = new DSNode();
                    break;
            }

            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node", default));
            this.AddManipulator(CreateNodeContextualMenu("Add Single Node", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Node", DSDialogueType.MultipleChoice));

        }

        private IManipulator CreateNodeContextualMenu(string actionText, DSDialogueType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionText, actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition, type)))
                );
            return contextualMenuManipulator;
        }

        private void AddStyles()
        {
            StyleSheet graphViewstyleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyle.uss");
            StyleSheet nodeStyleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSNodeStyle.uss");

            styleSheets.Add(graphViewstyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
    }

}
