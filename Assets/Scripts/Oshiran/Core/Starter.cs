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

public class Starter : IInitializable, IAsyncStartable
{
    readonly SaveFile saveFile;

    [Inject]
    public Starter(SaveFile saveFile)
    {
        this.saveFile = saveFile;

        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }

    public void Initialize()
    {
        AudioSystem.Instance.Initialize();
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await saveFile.Load();

        AudioSystem.Instance.ChangeVolume(saveFile.Data.Volume_BGM, saveFile.Data.Volume_SE);
    }
}

public enum GameMode
{
    Tutorial,
    Easy,
    Normal,
    Hard
}