using System;
using Oshiran.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameEventRelay : MonoBehaviour
{
    [SerializeField] public GameMode gameMode;

    [SerializeField] AudioClipPlayer audioClipPlayer;

    [SerializeField] PlayerController controller;
    [SerializeField] StageItemManager stageItemManager;
    [SerializeField] StageMover stageMover;
    [SerializeField] GameObject retryCanvas;
    [SerializeField] GameObject clearCanvas;
    [SerializeField] public ParticleSystem clearParticle;

    public event Action OnGameRestart;
    public event Action OnGameClear;
    public event Action OnPlayerDeath;

    void Awake()
    {
        controller.OnDeath += OnDeath;
    }

    public void OnRestart()
    {
        retryCanvas.SetActive(false);
        stageItemManager.ResetStageItems();
        controller.Restart();
        audioClipPlayer.PlayBGM();
        clearParticle.Stop();

        OnGameRestart?.Invoke();
    }

    public void OnDeath()
    {
        retryCanvas.SetActive(true);
        audioClipPlayer.GameOver();
        stageMover.Pause();

        OnPlayerDeath?.Invoke();
    }

    public void OnClear()
    {
        clearParticle.Play();

        controller.GetPlayerStatus().Reset();
        clearCanvas.SetActive(true);
        stageMover.Pause();
        audioClipPlayer.PlayClear();

        OnGameClear?.Invoke();
    }

    public void BackHome()
    {
        AudioSystem.Instance.StopBGM();
        SceneManager.LoadScene("Menu");
    }
}