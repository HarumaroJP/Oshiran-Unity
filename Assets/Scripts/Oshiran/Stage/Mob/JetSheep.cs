using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class JetSheep : MovableEntity
{
    protected override async UniTaskVoid ExecuteAsync(CancellationToken cToken)
    {
        await UniTask.WaitUntil(() => mainCamera.position.x >= transform.position.x - detectExtents,
            cancellationToken: cToken);

        float progressTime = 0f;
        while (!cToken.IsCancellationRequested || progressTime >= duration)
        {
            await UniTask.WaitForFixedUpdate(cToken);
            rig.AddForce(forceVector * Time.fixedDeltaTime * 100);
            progressTime += Time.fixedDeltaTime;
        }

        gameObject.SetActive(false);
    }
}