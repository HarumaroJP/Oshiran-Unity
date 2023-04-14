#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Popper))]
public class PopperEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Popper popper = target as Popper;

        if (GUILayout.Button("Shoot")) {
            popper.Shoot();
        }
    }
}
#endif
