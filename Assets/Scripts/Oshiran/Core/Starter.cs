using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class Starter : IAsyncStartable
{
    readonly SaveFile saveFile;

    [Inject]
    public Starter(SaveFile saveFile)
    {
        this.saveFile = saveFile;

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await saveFile.Load();

        AudioSystem.Instance.Initialize();
        AudioSystem.Instance.ChangeVolume(saveFile.Data.Volume_BGM, saveFile.Data.Volume_SE);
    }
}

public enum GameMode
{
    Easy,
    Normal,
    Hard
}