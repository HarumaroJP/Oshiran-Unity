using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VolcanicBomb : MovableEntity {

    [SerializeField] float forceTorque;


    protected override async UniTaskVoid ExecuteAsync(CancellationToken cToken) {
        await UniTask.WaitUntil(() => mainCamera.position.x >= transform.position.x - detectExtents,
            cancellationToken: cToken);

        rig.simulated = true;
        rig.AddForce(forceVector, ForceMode2D.Impulse);
        rig.AddTorque(forceTorque, ForceMode2D.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cToken);

        rig.simulated = false;
        gameObject.SetActive(false);
    }
}
