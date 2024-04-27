using DS.Windows;
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace DS.Utilities
{
    using DS.Data;
    using DS.Data.Save;
    using DS.ScriptableObjects;
    using Elements;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class DSIOUtility
    {
        private static DSGraphView graphView;
        private static string graphFileName;
        private static string containerFolderPath;

        private static List<DSNode> nodes;

        private static Dictionary<string, DSDialogueSO> createdDialogues;
        private static Dictionary<string, DSNode> loadedNodes;

        public static void Initialize(string graphName, DSGraphView dsGraphView)
        {
            graphView = dsGraphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}"; 

            nodes = new List<DSNode>();
            createdDialogues = new Dictionary<string, DSDialogueSO>();
            loadedNodes = new Dictionary<string, DSNode>();
        }

        #region Save Methods

        public static void Save()
        {
            CreateStaticFolders();

            GetElementsFromGraphView();

            DSGraphSaveDataSO graphData = CreateAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            DSDialogueContainerSO dialogueContrainer = CreateAsset<DSDialogueContainerSO>(containerFolderPath, graphFileName);
            dialogueContrainer.Initialize(graphFileName);


            SaveAsset(graphData);
            SaveAsset(dialogueContrainer);
            SaveNodes(graphData, dialogueContrainer);
        }
        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
        {
            List<string> nodeNames = new List<string>();

            foreach (DSNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, dialogueContainer);

                nodeNames.Add(node.DialogueName);
            }

            UpdateDialoguesChoicesConnections();
            UpdateOldNodes(nodeNames, graphData);
        }

        private static void UpdateOldNodes(List<string> currentNodeNames, DSGraphSaveDataSO graphData)
        {
            if (graphData.OldNodeNames != null && graphData.OldNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldNodeNames.Except(currentNodeNames).ToList();

                foreach(string nodeToRemove in  nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphData.OldNodeNames = new List<string>(currentNodeNames);
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
        {
            List<DSChoiceSaveData> choices = CloneNodeChoices(node.Choices);

            DSNodeSaveData nodeData = new DSNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                DialogueType = node.DialogueType,
                Position = node.GetPosition().position
            };

            graphData.Nodes.Add(nodeData);
        }

        private static List<DSChoiceSaveData> CloneNodeChoices(List<DSChoiceSaveData> nodeChoices)
        {
            List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

            foreach (DSChoiceSaveData choice in nodeChoices)
            {
                DSChoiceSaveData choiceData = new DSChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };

                choices.Add(choiceData);
            }

            return choices;
        }

        private static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
        {
            DSDialogueSO dialogue;
            dialogue = CreateAsset<DSDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
            dialogueContainer.Dialogues.Add(dialogue);

            dialogue.Initialize(node.DialogueName, node.Text, ConvertNodeChoicesToDialogueChoices(node.Choices), node.DialogueType, node.IsStartingNode());

            createdDialogues.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        #endregion

        #region Load Methods

        public static void Load()
        {
            DSGraphSaveDataSO graphData = LoadAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);

            if (graphData == null) { EditorUtility.DisplayDialog("couldn't load the file", "The file at the following path could not found:\n\n" + $"Assets/Editor/DialogueSystem/Graphs/{graphFileName}\n\n" + "Make sure you chose the right file and it's placed in the folder path", "Thx!" ); return; }
            DSEditorWindow.UpdateFileName(graphData.FileName);

            LoadNodes(graphData.Nodes);
            LoadNodesConnection();
        }

        private static void LoadNodesConnection()
        {
            foreach(KeyValuePair<string, DSNode> loadedNode in loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    DSChoiceSaveData choiceData = choicePort.userData as DSChoiceSaveData;

                    if (string.IsNullOrEmpty(choiceData.NodeID)) { continue; }
                    
                    DSNode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void LoadNodes(List<DSNodeSaveData> nodes)
        {
            foreach(DSNodeSaveData nodeData in nodes)
            {
                List<DSChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
                DSNode node = graphView.CreateNode(nodeData.Name, nodeData.Position, nodeData.DialogueType, false);

                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;

                node.Draw();

                graphView.AddElement(node);

                loadedNodes.Add(node.ID, node);

            }
        }

        #endregion
        private static void UpdateDialoguesChoicesConnections()
        {
            foreach(DSNode node in nodes)
            {
                DSDialogueSO dialogue = createdDialogues[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                    if (string.IsNullOrEmpty(nodeChoice.NodeID)) continue;
                    dialogue.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }
        private static List<DSDialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DSChoiceSaveData> nodeChoices)
        {
            List<DSDialogueChoiceData> dialogueChoices = new List<DSDialogueChoiceData>();

            foreach (DSChoiceSaveData nodeChoice in nodeChoices)
            {
                DSDialogueChoiceData choiceData = new DSDialogueChoiceData() 
                {
                    Text = nodeChoice.Text
                };
                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }

        #region Get
        private static void GetElementsFromGraphView()
        {

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DSNode node)
                {
                    nodes.Add(node);

                    return;
                }
            });
        }

        #endregion

        #region Create Methods

        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = path + "/" + assetName + ".asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static void CreateStaticFolders()
        {
            // ! Editor: Graphs
            CreateFolder("Assets/Editor/DialogueSystem", "Graphs");

            // ! Assets: DS
            CreateFolder("Assets", "DialogueSystem");
            CreateFolder("Assets/DialogueSystem", "Dialogues");

            // ! DS: Dialogues
            CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);

            // ! Global
            CreateFolder(containerFolderPath, "Global");

            // ! Global Dialogues
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }
        #endregion

        private static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }
       
    }

}
