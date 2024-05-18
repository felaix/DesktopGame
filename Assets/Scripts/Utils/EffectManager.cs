using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public Image _bg;

    [Header("Loading Animation")]
    [SerializeField] private RectTransform _mainLoadingIcon;
    [SerializeField] private float _timeStep;
    [SerializeField] private float _oneStepAngle;

    [Header("Typing Animation")]
    [SerializeField] public List<TMP_Text> _tmpList = new();
    [SerializeField] private float typingSpeed;
    [SerializeField] private Color _transparent;

    public bool onLoad = false;

    private float _startTime;

    private void OnEnable()
    {
        if (onLoad) EnableLoadingText();
    }

    public void TurnOnPC(float delay)
    {
        _bg.gameObject.SetActive(true);
        BlendInImage(_bg, delay);
        EnableLoadingText();
    }

    public void BlendInImage(Image img, float delay)
    {
        Color color = img.color;
        img.color = _transparent;
        img.DOBlendableColor(color, delay);
    }
         
    private async void EnableLoadingText()
    {
        Image img = _mainLoadingIcon.GetComponent<Image>();
        if (img != null )
        {
            Color color = img.color;
            img.color = _transparent;
            img.DOBlendableColor(color, 5f);
        }
        List<TMP_Text> tps = _tmpList;

        for (int i = 0; i < tps.Count; i++)
        {
            tps[i].gameObject.SetActive(true);
            await StartTypingAnimation(tps[i]);
        }

        GameManager.Instance.LoadLevel(1);
        Destroy(this);
    }

    private async Task StartTypingAnimation(TMP_Text tmp)
    {
        string msg = tmp.text;
        tmp.text = "";
        await TypeText(tmp, msg);
    }

    private async Task TypeText(TMP_Text tmp, string msg)
    {
        tmp.color = _transparent;

        foreach (char c in msg)
        {
            tmp.DOBlendableColor(Color.white, typingSpeed);
            tmp.text += c;
            await Wait(typingSpeed);
        }
    }

    private async Task Wait(float speed)
    {
        await Task.Delay((int)(speed * 1000));
    }

    void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - _startTime >= _timeStep)
        {
            Vector3 iconAngle = _mainLoadingIcon.localEulerAngles;
            iconAngle.z += _oneStepAngle;

            _mainLoadingIcon.localEulerAngles = iconAngle;

            _startTime = Time.time;
        }
    }
}
