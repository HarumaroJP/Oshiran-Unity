using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class AudioClipPlayer : MonoBehaviour
{
    [Space]
    [Header("Audio")]
    [SerializeField]
    AudioClip bgmClip;

    [SerializeField] AudioClip gameOverClip;

    [SerializeField] AudioClip clearClip;

    public void PlayBGM()
    {
        AudioSystem.Instance.PlayBGM(bgmClip);
    }

    public void PlayClear()
    {
        AudioSystem.Instance.PlayBGM(clearClip);
    }

    public void GameOver()
    {
        AudioSystem.Instance.PlayBGM(gameOverClip, 4f);
    }
}