using UnityEngine;

public interface IController {
    void Run();
    void Idle();
    void Death(bool doMove);
    void AddForce(Vector2 force);
    void AddOnaraAmount(float t);
    void Enable();
    void Disable();
}