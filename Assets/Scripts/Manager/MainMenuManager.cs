using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
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

    [SerializeField] private Button _startButton;

    private float _startTime;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _startButton.onClick?.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        TurnOnPC(.5f);
        //GameManager.Instance.LoadLevel(1, 3f);
    }

    public void TurnOnPC(float delay)
    {
        _bg.gameObject.SetActive(true);
        BlendInImage(_bg, delay);
        StartCoroutine(EnableLoadingText());
    }

    public void BlendInImage(Image img, float delay)
    {
        Color color = img.color;
        img.color = _transparent;
        img.DOBlendableColor(color, delay);
    }

    private IEnumerator EnableLoadingText()
    {
        Image img = _mainLoadingIcon.GetComponent<Image>();
        if (img != null)
        {
            Color color = img.color;
            img.color = _transparent;
            img.DOBlendableColor(color, 5f);
        }
        List<TMP_Text> tps = _tmpList;

        for (int i = 0; i < tps.Count; i++)
        {
            tps[i].gameObject.SetActive(true);
            yield return StartCoroutine(StartTypingAnimation(tps[i]));
        }

        GameManager.Instance.LoadLevel(1);
        Destroy(this);
    }

    private IEnumerator StartTypingAnimation(TMP_Text tmp)
    {
        string msg = tmp.text;
        tmp.text = "";
        yield return StartCoroutine(TypeText(tmp, msg));
    }

    private IEnumerator TypeText(TMP_Text tmp, string msg)
    {
        tmp.color = _transparent;

        foreach (char c in msg)
        {
            tmp.DOBlendableColor(Color.white, typingSpeed);
            tmp.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (_mainLoadingIcon == null) return;

        if (Time.time - _startTime >= _timeStep)
        {
            Vector3 iconAngle = _mainLoadingIcon.localEulerAngles;
            iconAngle.z += _oneStepAngle;

            _mainLoadingIcon.localEulerAngles = iconAngle;

            _startTime = Time.time;
        }
    }
}
