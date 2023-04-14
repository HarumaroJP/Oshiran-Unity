using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    IController m_playerController;
    void Start() { m_playerController = GameObject.FindWithTag("Player").GetComponent<IController>(); }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            m_playerController.Death(false);
        }
    }
}