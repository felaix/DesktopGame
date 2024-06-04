using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

[DefaultExecutionOrder(0)]
public class ActionInvoker : MonoBehaviour
{

    public static ActionInvoker Instance { get; private set; }

    private List<(Button btn, OnClickEvent evt, bool invoked, Transform objToScale)> _savedButtons = new List<(Button btn, OnClickEvent evt, bool invoked, Transform objToScale)>();

    [SerializeField] private GameObject _virusLoader;

    private void Awake()
    {
        Instance = this;
    }
    public void SaveButtonEvent(Button btn, OnClickEvent evt, Transform objToScale = null)
    {
        _savedButtons.Add((btn, evt, true, objToScale));
    }

    public void InvokeButton(Button btn)
    {
        // Check if the button already exists
        int index = _savedButtons.FindIndex(x => x.btn == btn);
        if (index == -1)
        {
            Debug.Log("Button not found.");
            return;
        }

        var (button, evt, invoked, objToScale) = _savedButtons[index];
  
        invoked = !invoked;

        switch (evt)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                if (objToScale == null) return;
                if (invoked)
                    objToScale.DOScale(Vector3.one, .5f); // Scale to normal size
                else
                    objToScale.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .5f); // Scale up
                break;
            case OnClickEvent.DownloadVirus:
                _virusLoader.SetActive(true);
                break;
        }

        _savedButtons[index] = (button, evt, invoked, objToScale);
    }

    public void InvokeOnClickEvent(Button btn, bool invoked, OnClickEvent ev = OnClickEvent.Null, Transform objToScale = null) 
    {

        var savedButton = _savedButtons.Find(x => x.btn == btn);
        if (savedButton == default)
        {
            Debug.Log("Button not found.");
            SaveButtonEvent(btn, ev);
        }


        switch (ev)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                if (objToScale == null) return;
                if (invoked) objToScale.DOScale(Vector3.one, .5f);
                else objToScale.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .5f);
                return;
        }
    }

    public void InvokeSavedEvent(bool invoked, OnClickEvent ev = OnClickEvent.Null, Transform objToScale = null)
    {
        switch (ev)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                if (objToScale == null) return;
                if (invoked) objToScale.DOScale(Vector3.one, .5f);
                else objToScale.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .5f);
                return;
        }
    }


}
