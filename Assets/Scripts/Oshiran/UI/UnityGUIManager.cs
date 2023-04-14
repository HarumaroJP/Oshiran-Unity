using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityGUIManager : MonoBehaviour {

    [SerializeField] Font misakiGothicFont;


    void Start() {
        Initialize();
    }


    void Initialize() {
        misakiGothicFont.material.mainTexture.filterMode = FilterMode.Point;
    }

}
