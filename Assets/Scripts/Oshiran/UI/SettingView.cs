using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    public event Func<(float bgm, float se)> GetVolume;
    public event Action OnSettingViewClosed;

    public void SetOpen(bool isOpen)
    {
        if (isOpen)
        {
            (float bgm, float se) volume = GetVolume();
            bgmSlider.value = volume.bgm;
            seSlider.value = volume.se;

            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            OnSettingViewClosed?.Invoke();
        }
    }
}