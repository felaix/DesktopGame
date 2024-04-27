using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSFEditorWindow : EditorWindow
{
    private GraphView graph;

    [MenuItem("Tools/Graph Editor")]
    public static void ShowWindow()
    {
        GetWindow<DSFEditorWindow>();
    }

    private void OnEnable()
    {
        graph = new DSFGraphView();

        graph.styleSheets.Add(Resources.Load<StyleSheet>("GraphStyleSheet"));

        rootVisualElement.Add(graph);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graph);
    }
}
