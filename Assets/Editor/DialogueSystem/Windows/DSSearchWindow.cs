using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DS;

namespace DS.Windows
{
    using Elements;
    using Enumerations;

    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DSGraphView graphView;

        public void Initialize(DSGraphView dsGraphView)
        {
            graphView = dsGraphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry (new GUIContent("Single Choice"))
                {
                    level = 2,
                    userData = DSDialogueType.SingleChoice
                },
                new SearchTreeEntry (new GUIContent("Multiple Choice"))
                {
                    level = 2,
                    userData = DSDialogueType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry (new GUIContent("Single Group"))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return searchEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMoussePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData) 
            {
                case DSDialogueType.SingleChoice:
                    {
                        DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode) graphView.CreateNode(localMoussePosition, DSDialogueType.SingleChoice);
                        graphView.AddElement(singleChoiceNode);
                        return true;
                    }
                case DSDialogueType.MultipleChoice:
                    {
                        DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode) graphView.CreateNode(localMoussePosition, DSDialogueType.MultipleChoice);
                        graphView.AddElement(multipleChoiceNode);
                        return true;
                    }
                case Group _:
                    {
                        Group group = graphView.CreateGroup("Dialogue Group", localMoussePosition);
                        graphView.AddElement(group);
                        return true;
                    }
                default: return false;
            }
        }
    }
    

}
