using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oshiran.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveFile
{
    const string FILENAME_CONFIG = "SaveData";

    public SaveData Data => saveData;
    public bool IsLoaded => isLoaded;

    SaveData saveData;
    bool isLoaded = false;

    public async UniTask Load()
    {
        saveData = ScriptableObject.CreateInstance<SaveData>();

        if (SaveSystem.FileExists(FILENAME_CONFIG))
        {
            await SaveSystem.LoadOverwrite(FILENAME_CONFIG, saveData);
        }

        isLoaded = true;
    }

    public async UniTask Save()
    {
        await SaveSystem.Save(saveData, FILENAME_CONFIG, true);
    }


    public void SetProgress(GameMode mode, int progress)
    {
        if (GetProgress(mode) > progress) return;

        switch (mode)
        {
            case GameMode.Tutorial:
                saveData.Prgs_Tutorial = progress;
                break;
            case GameMode.Easy:
                saveData.Prgs_Easy = progress;
                break;

            case GameMode.Normal:
                saveData.Prgs_Normal = progress;
                break;

            case GameMode.Hard:
                saveData.Prgs_Hard = progress;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }

        saveData.PlayOnce = true;

        Save().Forget();
    }

    public (float bgm, float se) GetVolume() => (saveData.Volume_BGM, saveData.Volume_SE);


    public void SaveVolume(float bgm, float se)
    {
        saveData.Volume_BGM = bgm;
        saveData.Volume_SE = se;

        Save().Forget();
    }


    public int GetProgress(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Tutorial:
                return saveData.Prgs_Tutorial;
            case GameMode.Easy:
                return saveData.Prgs_Easy;

            case GameMode.Normal:
                return saveData.Prgs_Normal;

            case GameMode.Hard:
                return saveData.Prgs_Hard;

            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }


    public (int tutorial, int easy, int normal, int hard) GetProgress()
    {
        return (saveData.Prgs_Tutorial, saveData.Prgs_Easy, saveData.Prgs_Normal, saveData.Prgs_Hard);
    }


    public void ResetData()
    {
        saveData.Prgs_Easy = 0;
        saveData.Prgs_Normal = 0;
        saveData.Prgs_Hard = 0;

        Save().Forget();
    }
}