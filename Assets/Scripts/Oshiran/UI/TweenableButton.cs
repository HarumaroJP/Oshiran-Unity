using System;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class TweenableButton : MonoBehaviour
{
    [SerializeField] public float tweenDuration;

    [Space] [SerializeField] public Vector3 pointerMoveOffset;
    [SerializeField] public Vector3 pointerDownScaleOffset;

    [SerializeField] bool ignoreTimeScale;

    [Space] public AudioClip onClickSe;

    [Space] public UnityEvent onClickByTween;

    RectTransform m_rectTransform;
    Button button;
    Vector3 m_positionCache;
    Vector3 m_scaleCache;

    bool m_isSceneUnload;


    void Awake()
    {
        SceneManager.sceneUnloaded += _ => m_isSceneUnload = false;

        //Cache initial coordinates.
        m_rectTransform = GetComponent<RectTransform>();

        m_positionCache = m_rectTransform.anchoredPosition;
        m_scaleCache = m_rectTransform.localScale;

        //Register Events.
        button = GetComponent<Button>();
        button.onClick.AddListener(() => AudioSystem.Instance.PlaySe(onClickSe));

        EventTrigger trigger = GetComponent<EventTrigger>();


        EventTrigger.Entry entryPointerDown;

        if (trigger.triggers.Any(t => t.eventID == EventTriggerType.PointerDown))
        {
            //If event trigger already has pointer down event.
            entryPointerDown = trigger.triggers.Find(e => e.eventID == EventTriggerType.PointerDown);
        }
        else
        {
            entryPointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown,
            };
            trigger.triggers.Add(entryPointerDown);
        }

        entryPointerDown.callback.AddListener(_ =>
        {
            if (!button.interactable) return;
            OnPointerDown();
        });

        EventTrigger.Entry entryPointerUp;

        if (trigger.triggers.Any(t => t.eventID == EventTriggerType.PointerUp))
        {
            //If event trigger already has pointer up event.
            entryPointerUp = trigger.triggers.Find(e => e.eventID == EventTriggerType.PointerUp);
        }
        else
        {
            entryPointerUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp,
            };
            trigger.triggers.Add(entryPointerUp);
        }

        entryPointerUp.callback.AddListener(_ =>
        {
            if (!button.interactable) return;
            OnPointerUp();
        });
    }


    void OnPointerDown()
    {
        m_tweenPos = m_rectTransform.DOAnchorPos(m_positionCache + pointerMoveOffset, tweenDuration)
            .SetLink(gameObject);
        m_tweenScale = m_rectTransform.DOScale(m_scaleCache + pointerDownScaleOffset, tweenDuration)
            .SetLink(gameObject);

        if (ignoreTimeScale)
        {
            m_tweenPos.SetUpdate(true);
            m_tweenScale.SetUpdate(true);
        }

        m_tweenPos.Play();
        m_tweenScale.Play();

        OnClickAsync().Forget();
    }


    Tween m_tweenPos;
    Tween m_tweenScale;


    void OnPointerUp()
    {
        if (m_isSceneUnload) return;

        m_tweenPos = m_rectTransform.DOAnchorPos(m_positionCache, tweenDuration).SetLink(gameObject);
        m_tweenScale = m_rectTransform.DOScale(m_scaleCache, tweenDuration).SetLink(gameObject);

        if (ignoreTimeScale)
        {
            m_tweenPos.SetUpdate(true);
            m_tweenScale.SetUpdate(true);
        }

        m_tweenPos.Play();
        m_tweenScale.Play();
    }


    async UniTaskVoid OnClickAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(tweenDuration * 2f), ignoreTimeScale);
        await UniTask.Yield();

        m_tweenPos?.Kill();
        m_tweenScale?.Kill();

        onClickByTween?.Invoke();
    }
}