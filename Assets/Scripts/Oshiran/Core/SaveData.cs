using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/SaveData")]
public class SaveData : ScriptableObject
{
    public float Volume_BGM = 0.3f;
    public float Volume_SE = 0.1f;

    public int Prgs_Easy = 0;
    public int Prgs_Normal = 0;
    public int Prgs_Hard = 0;
}