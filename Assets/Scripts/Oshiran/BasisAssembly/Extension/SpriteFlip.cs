using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class SpriteFlip
{
    readonly SpriteRenderer m_Renderer;
    readonly Sprite[] m_Sprites;
    readonly float m_interval;
    CancellationTokenSource m_cTokenSrc;

    int m_CurrentFrame;
    public bool isRunning;

    public SpriteFlip(SpriteRenderer renderer, Sprite[] sprites, float interval)
    {
        m_Renderer = renderer;
        m_Sprites = sprites;
        m_interval = interval;
    }

    public async UniTaskVoid UpdateSprite()
    {
        m_cTokenSrc?.Dispose();
        m_cTokenSrc = new CancellationTokenSource();
        CancellationToken cToken = m_cTokenSrc.Token;

        while (isRunning && !cToken.IsCancellationRequested)
        {
            m_Renderer.sprite = m_Sprites[m_CurrentFrame];
            m_CurrentFrame++;

            if (m_CurrentFrame >= m_Sprites.Length) m_CurrentFrame = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(m_interval), cancellationToken: cToken);
        }
    }

    public void Cancel()
    {
        m_cTokenSrc?.Cancel();
    }
}