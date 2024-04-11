using DS.Utilities;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        private DSGraphView graphView;
        private readonly string defaultFileName = "Dialogue Filename";
        private Button saveBtn;
        private TextField fileNameTextField;

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

           fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "File name: ", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveBtn = DSElementUtility.CreateButton("Save", () => Save());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveBtn);

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog
                    (
                    "Invalid file name.",
                    "Please ensure the value is valid",
                    "Roger!"
                    ) ;
                return;
            }

            DSIOUtility.Initialize(fileNameTextField.value, graphView);
            DSIOUtility.Save();
        }

        private void AddGraphView()
        {
            graphView = new DSGraphView(this);

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
