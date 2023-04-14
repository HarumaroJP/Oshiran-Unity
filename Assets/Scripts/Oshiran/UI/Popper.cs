using DG.Tweening;
using UnityEngine;

public class Popper : MonoBehaviour
{
    [SerializeField] ParticleSystem shootEffect;

    [SerializeField] float duration = 0.6f;
    [SerializeField] Vector2 shootOffset;
    [SerializeField] Vector2 scaleOffset;

    public void Shoot()
    {
        shootEffect.Play();
        transform.DOPunchPosition(shootOffset, duration, 1).Play();
        transform.DOPunchScale(scaleOffset, duration, 1).Play();
    }
}