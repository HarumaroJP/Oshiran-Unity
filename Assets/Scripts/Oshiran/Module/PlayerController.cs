using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerController : BaseController, IController
{
    [SerializeField] BoxCollider2D playerCol;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] SpriteAtlas hipAtlas;
    [SerializeField] SpriteAtlas runAtlas;
    [SerializeField] SpriteAnimation fireAnimation;
    [SerializeField] ParticleSystem mainJet;
    [SerializeField] ParticleSystem subJet;
    [SerializeField] ParticleSystem groundDust;

    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerSpec spec;

    [Header("Ground Collide")]
    [SerializeField]
    Vector2 currentColSize;

    [SerializeField] Vector3 collideOffset;
    [SerializeField] float collideDistance;

    [SerializeField] Vector2 crouchColSize, currentColOffset, crouchColOffset;

    [SerializeField] ContactFilter2D contactFilterGround;

    [Header("Other Settings")]
    [SerializeField]
    float deathForceOffset;

    [SerializeField] float deathForceDuration;

    ContactFilter2D m_contactFilterWall;

    Vector2 m_jumpBuffer, m_jumpOffset, m_crouchBuffer;
    Vector3 m_hipPositionBuffer;
    PhysicsScene2D m_physicsScene;
    Transform m_myTransform;

    bool m_isJetting;
    bool m_isCrouching;
    bool m_isGround;

    public event Action OnDeath;

    CancellationTokenSource fireAnimCanceller = new CancellationTokenSource();

    public void Setup()
    {
        m_physicsScene = Physics2D.defaultPhysicsScene;
        m_myTransform = transform;
        status.SetProperty(spec);

        Initialize();
        AnimateRun();

        status.isAlive = true;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return status;
    }

    public PlayerSpec GetPlayerSpec()
    {
        return spec;
    }

    void Initialize()
    {
        //Register sprite
        Sprite[] hipsCrouch = new Sprite[hipAtlas.spriteCount];
        hipAtlas.GetSprites(hipsCrouch);
        hipsCrouch = hipsCrouch.OrderByName();

        Sprite[] hipsRun = new Sprite[runAtlas.spriteCount];
        runAtlas.GetSprites(hipsRun);
        hipsRun = hipsRun.OrderByName();

        status.SetAppearance(GetComponent<SpriteRenderer>(), hipsCrouch, hipsRun);

        //Set control offset
        m_jumpBuffer = new Vector2(0f, 115f);
        m_jumpOffset = new Vector2(0f, 250f);
        m_crouchBuffer = new Vector2(0f, -2f);

        //Register key callback
        jumpAction.performed += Jump;
        crouchAction.performed += Crouch;

        //Cache collider info
        currentColSize = playerCol.size;
        currentColOffset = playerCol.offset;

        //Cache player position (for player init)
        m_hipPositionBuffer = transform.localPosition;


        int ignore = LayerMask.NameToLayer("Ignore Raycast");
        ignoreLayer = 1 << ignore;
        ignoreLayer = ~ignoreLayer;
    }


    int ignoreLayer;


    void Update()
    {
        if (!status.isAlive) return;

        //if oshiri hit the wall
        if (m_physicsScene.Raycast(m_myTransform.position + collideOffset,
                Vector2.right,
                collideDistance,
                ignoreLayer))
        {
            Death(false);
        }

        m_isGround = m_physicsScene.Raycast(m_myTransform.position + new Vector3(0, -0.9f, 0), -Vector2.up, 0.5f);

        if (m_isGround && status.isCrouching)
        {
            if (!groundDust.isPlaying)
            {
                groundDust.Play();
            }
        }
        else
        {
            if (groundDust.isPlaying)
            {
                groundDust.Stop();
            }
        }
#if UNITY_STANDALONE
        UpdateKeyEvent();

        if (jetControl.wasPressedThisFrame) JetStart();
        if (jetControl.wasReleasedThisFrame) JetEnd();
#endif


        if (m_isJetting) Jet();
        if (status.isCrouching) CountCrouch();
    }


    void Jump(InputAction.CallbackContext ctx)
    {
        if (rig.IsTouching(contactFilterGround))
        {
            AddForce(m_jumpBuffer * Mathf.Pow(status.crouchingTime, 2) + m_jumpOffset);
        }

        status.crouchingTime = 0f;
        status.isCrouching = false;
        status.SetStatus(PlayerState.Normal);
        playerCol.size = currentColSize;
        playerCol.offset = currentColOffset;
        AnimateRun();
    }


    public void AddForce(Vector2 force)
    {
        rig.AddForce(force, ForceMode2D.Impulse);
    }


    public void JetStart()
    {
        if (status.onaraAmount <= 0f) return;

        m_isJetting = true;
        mainJet.Play();
        subJet.Play();
    }


    public void Jet()
    {
        status.SetOnara(-Time.deltaTime);

        if (status.onaraAmount <= 0f)
        {
            JetEnd();
            return;
        }

        rig.AddForce(new Vector2(0f, spec.jetPower * (100f * Time.deltaTime)), ForceMode2D.Force);
    }


    public void JetEnd()
    {
        m_isJetting = false;
        mainJet.Stop();
        subJet.Stop();
    }


    void Crouch(InputAction.CallbackContext ctx)
    {
        status.SetStatus(PlayerState.Crouch);
        status.isCrouching = true;
        status.spriteFlip.isRunning = false;

        playerCol.size = crouchColSize;
        playerCol.offset = crouchColOffset;
    }


    void CountCrouch()
    {
        status.crouchingTime += Time.deltaTime;
        rig.AddForce(m_crouchBuffer, ForceMode2D.Force);

        if (status.crouchingTime.InRange(0f, 1f))
        {
            status.SetStatus(PlayerState.CrouchLv1);
        }
        else if (status.crouchingTime.InRange(1f, 2f))
        {
            status.SetStatus(PlayerState.CrouchLv2);
        }
        else if (status.crouchingTime.InRange(2f, 3f))
        {
            status.SetStatus(PlayerState.CrouchLv3);
        }
        else if (status.crouchingTime > 3f)
        {
            status.SetStatus(PlayerState.CrouchLv4);
            Death(true);
        }
    }


    void AnimateRun()
    {
        status.spriteFlip.Cancel();
        status.spriteFlip.isRunning = true;
        status.spriteFlip.UpdateSprite();
    }


    public void Run()
    {
        Enable();
        rig.simulated = true;
        AnimateRun();
    }


    public void Idle()
    {
        Disable();
        rig.simulated = false;
    }


    public void Death(bool doMove)
    {
        Disable();
        status.Reset();

        if (doMove)
        {
            fireAnimation.SpriteAnimeStart(fireAnimCanceller.Token);

            rig.DOMoveX(rig.position.x + deathForceOffset, deathForceDuration)
                .SetEase(Ease.OutQuad)
                .Play()
                .OnComplete(() =>
                {
                    groundDust.Stop();
                });
        }

        OnDeath?.Invoke();
    }


    public void AddOnaraAmount(float t)
    {
        status.SetOnara(t);
    }

    public void Restart()
    {
        Enable();

        fireAnimCanceller?.Cancel();
        fireAnimCanceller?.Dispose();

        fireAnimCanceller = new CancellationTokenSource();

        fireAnimation.ResetSprite();

        rig.simulated = true;
        rig.velocity = Vector2.zero;
        transform.localPosition = m_hipPositionBuffer;

        status.isAlive = true;
        status.SetStatus(PlayerState.Normal);

        AnimateRun();
#if UNITY_EDITOR
        DevEx.ClearLog();
#endif
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position + collideOffset;
        Gizmos.DrawLine(origin, origin + Vector3.right * collideDistance);

        origin = transform.position + new Vector3(0, -0.9f, 0);
        Gizmos.DrawLine(origin, origin - Vector3.up * 0.5f);
    }


    void OnDestroy()
    {
        jumpAction.performed -= Jump;
        crouchAction.performed -= Crouch;
    }
}