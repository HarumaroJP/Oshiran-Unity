using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHistoryView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialProgressText;
    [SerializeField] GameObject tutorialProgressBatch;
    [SerializeField] TextMeshProUGUI easyProgressText;
    [SerializeField] GameObject easyProgressBatch;
    [SerializeField] TextMeshProUGUI normalProgressText;
    [SerializeField] GameObject normalProgressBatch;
    [SerializeField] TextMeshProUGUI hardProgressText;
    [SerializeField] GameObject hardProgressBatch;

    public void SetProgress(int tutorial, int easy, int normal, int hard)
    {
        tutorialProgressText.text = $"{tutorial}%";
        tutorialProgressBatch.SetActive(tutorial >= 100);

        easyProgressText.text = $"{easy}%";
        easyProgressBatch.SetActive(easy >= 100);

        normalProgressText.text = $"{normal}%";
        normalProgressBatch.SetActive(normal >= 100);

        hardProgressText.text = $"{hard}%";
        hardProgressBatch.SetActive(hard >= 100);
    }
}