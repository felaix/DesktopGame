using DS.Utilities;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "Dialogue Filename";
        private Button saveBtn;

        [MenuItem("Tools/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();
            AddToolbar();
            AddStyle();
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            TextField fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "File name: ");

            saveBtn = DSElementUtility.CreateButton("Save");

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveBtn);

            rootVisualElement.Add(toolbar);
        }

        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddStyle()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        #region Utility methods
        public void EnableSaving()
        {
            saveBtn.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveBtn.SetEnabled(false);
        }

        #endregion
    }

}
