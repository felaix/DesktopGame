using DS.Utilities;
using System.IO;
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
        private static TextField fileNameTextField;

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
            Button clearBtn = DSElementUtility.CreateButton("Clear", () => Clear());
            Button resetBtn = DSElementUtility.CreateButton("Reset", () => ResetGraph());
            Button loadBtn = DSElementUtility.CreateButton("Load", () => Load());
           

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveBtn);
            toolbar.Add(clearBtn);
            toolbar.Add(resetBtn);
            toolbar.Add(loadBtn);

            rootVisualElement.Add(toolbar);
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");
            if (string.IsNullOrEmpty(filePath)) { return; }
            Clear();
            DSIOUtility.Initialize(Path.GetFileNameWithoutExtension(filePath), graphView);
            DSIOUtility.Load();
        }

        private void Clear()
        {
            graphView.ClearGraph(); 
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
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
