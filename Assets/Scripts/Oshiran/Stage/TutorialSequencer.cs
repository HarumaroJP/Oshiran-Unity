using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequencer : MonoBehaviour
{
    [SerializeField] List<TutorialPanel> panels;
    [SerializeField] Image noticePanel;
    CancellationTokenSource m_cTokenSrc;

    void Start()
    {
        Color color = noticePanel.color;
        color.a = 0f;
        noticePanel.color = color;
    }

    async UniTaskVoid StartAsync(CancellationToken token)
    {
        GameObject previousObj = null;
        bool firstTake = true;

        foreach (TutorialPanel panel in panels)
        {
            if (!firstTake)
            {
                previousObj.SetActive(false);
            }

            panel.gameObject.SetActive(true);
            previousObj = panel.gameObject;

            if (panel.notice)
            {
                Notice();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(panel.duration), cancellationToken: token);

            firstTake = false;
        }

        previousObj.SetActive(false);
    }

    public void Cancel()
    {
        m_cTokenSrc?.Cancel();
    }

    public void Notice()
    {
        noticePanel.DOFade(1f, 2f).SetEase(Ease.Flash, 4).SetDelay(1f).Play();
    }

    public void Restart()
    {
        //Initialize all panels.
        foreach (TutorialPanel panel in panels)
        {
            panel.gameObject.SetActive(false);
        }

        m_cTokenSrc?.Dispose();
        m_cTokenSrc = new CancellationTokenSource();

        StartAsync(m_cTokenSrc.Token).Forget();
    }

    void OnDestroy()
    {
        m_cTokenSrc?.Cancel();
    }

    [Serializable]
    public struct TutorialPanel
    {
        public GameObject gameObject;
        public float duration;
        public bool notice;
    }
}