using UnityEngine;

public class SpearTrap : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<IController>().Death(false);
        }
    }
}
