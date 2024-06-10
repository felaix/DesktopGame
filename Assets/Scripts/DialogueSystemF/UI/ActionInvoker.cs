using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

[DefaultExecutionOrder(0)]
public class ActionInvoker : MonoBehaviour
{
    public static ActionInvoker Instance { get; private set; }

    private List<(Button btn, OnClickEvent evt, bool invoked)> _savedButtons = new List<(Button btn, OnClickEvent evt, bool invoked)>();

    [SerializeField] private GameObject _virusLoader;
    [SerializeField] private GameObject _photoContainer;

    private void Awake()
    {
        Instance = this;
    }
    public void SaveButtonEvent(Button btn, OnClickEvent evt)
    {
        _savedButtons.Add((btn, evt, true));
    }

    public void SetPhotoSprite(Sprite sprite)
    {
        Debug.Log(sprite.ToString());
        _photoContainer.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
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

        var (button, evt, invoked) = _savedButtons[index];
  
        invoked = !invoked;

        switch (evt)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                _photoContainer.SetActive(true);
                break;
            case OnClickEvent.DownloadVirus:
                _virusLoader.SetActive(true);
                break;
        }

        _savedButtons[index] = (button, evt, invoked);
    }

    public void InvokeOnClickEvent(Button btn, bool invoked, OnClickEvent ev = OnClickEvent.Null) 
    {
        // -- Check if the btn is already saved --
        var savedButton = _savedButtons.Find(x => x.btn == btn);
        if (savedButton == default)
        {
            Debug.Log("Button not found.");
            SaveButtonEvent(btn, ev);
        }

        // -- Trigger event based on OnClickEvent Enum --
        switch (ev)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                return;
        }
    }

    public void InvokeSavedEvent(bool invoked, OnClickEvent ev = OnClickEvent.Null)
    {
        switch (ev)
        {
            case OnClickEvent.Null:
                return;
            case OnClickEvent.PrintHelloWorld:
                Debug.Log("Hello World");
                return;
            case OnClickEvent.ScaleImage:
                return;
        }
    }


}
