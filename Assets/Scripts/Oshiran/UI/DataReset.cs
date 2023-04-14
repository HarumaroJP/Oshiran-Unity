using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DataReset : MonoBehaviour
{
    [SerializeField] ProgressHistoryView progressHistoryView;

    SaveFile saveFile;

    [Inject]
    public void Construct(SaveFile saveFile)
    {
        this.saveFile = saveFile;
    }

    public void ResetSaveData()
    {
        saveFile.ResetData();
        progressHistoryView.SetProgress(0, 0, 0);
    }
}