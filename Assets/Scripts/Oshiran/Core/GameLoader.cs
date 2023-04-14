using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour {
    [SerializeField] Image progressBar;
    public UnityEvent onSceneLoadCompleted;
    public bool AllowSceneActivation { get; set; }

    void Awake() { Application.targetFrameRate = Screen.currentResolution.refreshRate; }

    public void LoadStage(string str) { LoadMainScene(str).Forget(); }

    Tween m_progressBar;

    async UniTaskVoid LoadMainScene(string str) {
        try {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            AsyncOperation operation = SceneManager.LoadSceneAsync(str);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f) {
                // Debug.Log(operation.progress * 100f + "%読み込み完了");
                await UniTask.Yield();
            }

            m_progressBar = progressBar.DOFillAmount(0.8f, 0.8f).Play();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            m_progressBar = progressBar.DOFillAmount(1f, 0.8f).Play().OnComplete(() => onSceneLoadCompleted?.Invoke());

            await UniTask.WaitUntil(() => AllowSceneActivation);

            DOTween.KillAll();
            DOTween.ClearCachedTweens();

            await UniTask.Yield();

            operation.allowSceneActivation = true;
        }
        catch {
            // ignore UnobservedTaskException.
        }
    }

    void OnDisable() { m_progressBar?.Kill(); }
}