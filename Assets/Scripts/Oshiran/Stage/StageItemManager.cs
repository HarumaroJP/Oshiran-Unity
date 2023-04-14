using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageItemManager : MonoBehaviour
{
    [SerializeField] List<ExecutableEntity> executables;

    public void Awake()
    {
        executables = GetComponentsInChildren<ExecutableEntity>().ToList();
    }

    public void ResetStageItems()
    {
        foreach (ExecutableEntity ex in executables)
        {
            ex.Rewind();
        }
    }
}