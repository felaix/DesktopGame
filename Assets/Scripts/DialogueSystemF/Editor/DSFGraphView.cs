using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DSFGraphView : GraphView
{
    public DSFGraphView()
    {
        AddBackground();
    }

    private void AddBackground()
    {
        GridBackground bg = new();
        bg.StretchToParentSize();
        Insert(0, bg);
    }
}
