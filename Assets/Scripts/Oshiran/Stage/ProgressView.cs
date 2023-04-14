using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressView : MonoBehaviour
{
    [SerializeField] GameObject newRecordObj;
    [SerializeField] GameObject normalRecordObj;
    [SerializeField] ResultPoppers resultPoppers;
    [SerializeField] TextTweener counterText;

    public void SetValue(int value)
    {
        counterText.SetValue(value);
    }

    public Sequence DOValue(int value, float delay)
    {
        return counterText.DOValue(value, delay);
    }

    public void NewRecord()
    {
        newRecordObj.SetActive(true);
        resultPoppers.Execute();
    }

    public void NormalRecord()
    {
        normalRecordObj.SetActive(true);
    }

    public void ResetDisplay()
    {
        newRecordObj.SetActive(false);
        normalRecordObj.SetActive(false);
    }
}