using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;


namespace DS.Windows
{
    using Data.Error;
    using DS.Data.Save;
    using Elements;
    using Enumerations;
    using System;
    using System.Collections.Generic;

    public class DSGraphView : GraphView
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;

        private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

        #region Repeated Names
         
        private int repeatedNamesAmount;
        public int RepeatedNamesAmount
        {
            get { return repeatedNamesAmount; }
            set
            {
                repeatedNamesAmount = value;
                if (repeatedNamesAmount == 0)
                {
                    editorWindow.EnableSaving();
                }

                if (repeatedNamesAmount > 0)
                {
                    editorWindow.DisableSaving();
                }
            }
        }
        #endregion
        public DSGraphView(DSEditorWindow dSEditor) {
            editorWindow = dSEditor;

            ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();

            AddStyles();
        }

        public DSNode CreateNode(string nodeName, Vector2 position, DSDialogueType type, bool showDraw = true)
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

            node.Initialize(nodeName, this, position);
            if (showDraw) node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        #region Repeated Elements

        public void RemoveGroupedNode(DSNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

            List<DSNode> groupNodeList = groupedNodes[group][nodeName].Nodes;

            groupNodeList.Remove(node);

            node.ResetStyle();

            if (groupNodeList.Count == 1) { groupNodeList[0].ResetStyle(); --RepeatedNamesAmount; return; }

            if (groupNodeList.Count == 0)
            {

                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0) { groupedNodes.Remove(group); }

            }
        }

        // ! Add Node to Group
        public void AddGroupedNode(DSNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {

                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                nodeErrorData.Nodes.Add(node);

                groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Add(node);


            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        // Add node to graph view (not inside group)
        public void AddUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DSNodeErrorData errorData = new DSNodeErrorData();
                errorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName, errorData);
                return;
            }

            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;

                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Remove(node);
            node.ResetStyle();
            if (ungroupedNodesList.Count == 1) { ungroupedNodesList[0].ResetStyle(); --RepeatedNamesAmount;  return; }
            if (ungroupedNodesList.Count == 0) { ungroupedNodes.Remove(nodeName); }
        }

        #endregion

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port => { if (startPort == port) return; if (startPort.node == port.node) return; if (startPort.direction == port.direction) return; compatiblePorts.Add(port); });

            return compatiblePorts;
        }

        #region Manipulators

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node", default));
            this.AddManipulator(CreateNodeContextualMenu("Add Single Node", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Node", DSDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextualMenu());

        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );
            return contextualMenuManipulator;
        }

        public Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group() { title = title };
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        private IManipulator CreateNodeContextualMenu(string actionText, DSDialogueType type)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionText, actionEvent => AddElement(CreateNode("DialogueName", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition), type)))
                );
            return contextualMenuManipulator;
        }

        #endregion

        #region Add Elements

        private void AddSearchWindow()
        {
            if (searchWindow == null) { searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>(); searchWindow.Initialize(this); }
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
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

        #endregion

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (isSearchWindow) { worldMousePosition -= editorWindow.position.position; }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));

            groupedNodes.Clear();
            ungroupedNodes.Clear();

            RepeatedNamesAmount = 0;
        }

        #endregion

        #region Callbacks

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null) 
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DSNode nextNode = (DSNode) edge.input.node;
                        DSChoiceSaveData choiceData = (DSChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;

                        DSChoiceSaveData choiceData = (DSChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<DSNode> nodesToDelete = new List<DSNode>();

                foreach (GraphElement element in selection)
                {
                    if (element is DSNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    } 
                }

                foreach (DSNode node in nodesToDelete)
                {
                    node.Group?.RemoveElement(node);
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode)) { continue; }

                    DSNode node = (DSNode) element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, group);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode)) { continue; }

                    DSNode node = (DSNode)element;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }


        #endregion
    }

}
