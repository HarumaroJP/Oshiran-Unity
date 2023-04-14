using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerStatus
{
    SpriteRenderer m_Renderer;
    Sprite[] m_HipSprites;
    PlayerSpec m_Spec;

    public SpriteFlip spriteFlip;
    public PlayerState currentState;
    public float crouchingTime;
    public float onaraCapacity;
    public float onaraAmount;
    public bool isCrouching;
    public bool isAlive;

    public event Action<float, float> OnSetOnara;

    public void SetProperty(PlayerSpec spec)
    {
        m_Spec = spec;
    }

    public void SetAppearance(SpriteRenderer renderer, Sprite[] hipSprites, Sprite[] runSprites)
    {
        m_Renderer = renderer;
        m_HipSprites = hipSprites;

        spriteFlip = new SpriteFlip(renderer, runSprites, m_Spec.runInterval);
    }

    public void SetOnara(float t)
    {
        onaraAmount += t;
        onaraAmount = Mathf.Clamp(onaraAmount, 0f, onaraCapacity);

        OnSetOnara?.Invoke(onaraAmount, onaraCapacity);
    }

    public void SetStatus(PlayerState state)
    {
        currentState = state;
        m_Renderer.sprite = m_HipSprites[(int)state];
    }


    public void Reset()
    {
        isAlive = false;
        isCrouching = false;
        spriteFlip.isRunning = false;
        onaraAmount = 0f;
        SetOnara(0f);
        crouchingTime = 0f;
    }
}

public enum PlayerState
{
    Normal = 0,
    Crouch = 1,
    CrouchLv1 = 2,
    CrouchLv2 = 3,
    CrouchLv3 = 4,
    CrouchLv4 = 5
}