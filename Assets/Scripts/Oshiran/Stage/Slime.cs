using System;
using DG.Tweening;
using UnityEngine;

public class Slime : ExecutableEntity {
    [SerializeField] Vector2 bouncePower;

    Sequence m_seqBounce;
    IController m_controller;


    void Start() {
        m_seqBounce = DOTween.Sequence().SetAutoKill(false).SetLink(gameObject);

        m_seqBounce.Append(transform.DOScaleY(0.5f, 0.18f).SetEase(Ease.OutBounce))
                   .AppendCallback(Bounce)
                   .Append(transform.DOScaleY(2f, 0.1f).SetEase(Ease.OutBounce))
                   .Append(transform.DOScaleY(1f, 0.18f).SetEase(Ease.OutBounce));
    }


    public override void Rewind() {
        m_seqBounce.Rewind();
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            m_controller = other.GetComponent<IController>();
            m_seqBounce.Play();
        }
    }


    void Bounce() {
        m_controller.AddForce(bouncePower * 100);
    }
}
