using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JetTutorial : MonoBehaviour
{
    [SerializeField] Vector3 moveOffset;
    [SerializeField] int bounceCount;
    [SerializeField] RectTransform rectTransform;

    Vector3 origin;

    void Awake()
    {
        origin = rectTransform.anchoredPosition;
    }

    void OnEnable()
    {
        rectTransform.gameObject.SetActive(true);
        rectTransform.DOAnchorPos(origin + moveOffset, 0.5f).SetLoops(bounceCount * 2, LoopType.Yoyo).Play();
    }

    void OnDisable()
    {
        rectTransform.gameObject.SetActive(false);
    }
}