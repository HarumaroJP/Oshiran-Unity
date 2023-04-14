using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTweener : MonoBehaviour {
    [SerializeField] float duration;
    [SerializeField] Vector2 scaleOffset;
    [SerializeField] TextMeshProUGUI textMesh;


    public void SetValue(int value) {
        textMesh.text = $"{value}%";
    }


    public Sequence DOValue(int to, float delay) {
        SetValue(0);
        int progress = 0;
        float fadeDuration = 0.2f;

        //値のTween
        Tween valueTween = DOTween.To(() => progress,
                                       x => {
                                           progress = x;
                                           textMesh.text = $"{progress}%";
                                       },
                                       to,
                                       duration)
                                  .SetTarget(textMesh);

        //スケールのTween
        Tween scaleTween = textMesh.transform.DOScale(scaleOffset, fadeDuration);

        //なぜかRewindできないので一旦スケール1で対応
        Tween defaultScaleTween = textMesh.transform.DOScale(Vector3.one, fadeDuration);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.AppendCallback(() => valueTween.Play());
        seq.Append(scaleTween);
        seq.AppendInterval(duration - fadeDuration * 2f);
        seq.Append(defaultScaleTween);
        seq.Play();

        return seq;
    }
}
