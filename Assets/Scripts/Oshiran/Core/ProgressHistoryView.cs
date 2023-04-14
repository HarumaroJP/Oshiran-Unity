using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHistoryView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI easyProgressText;
    [SerializeField] TextMeshProUGUI normalProgressText;
    [SerializeField] TextMeshProUGUI hardProgressText;

    public void SetProgress(int easy, int normal, int hard)
    {
        easyProgressText.text = $"{easy}%";
        normalProgressText.text = $"{normal}%";
        hardProgressText.text = $"{hard}%";
    }
}