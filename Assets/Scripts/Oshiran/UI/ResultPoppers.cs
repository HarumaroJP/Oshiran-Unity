using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResultPoppers : MonoBehaviour
{
    [SerializeField] float popDelay;
    [SerializeField] Popper[] poppers;

    public void Execute()
    {
        if (popDelay <= 0f)
        {
            Pop();
        }
        else
        {
            ExecuteAsync().Forget();
        }
    }

    async UniTaskVoid ExecuteAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(popDelay));
        Pop();
    }

    void Pop()
    {
        foreach (Popper popper in poppers)
        {
            popper.Shoot();
        }
    }
}