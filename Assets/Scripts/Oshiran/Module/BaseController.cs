using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BaseController : MonoBehaviour {
    [SerializeField] PlayerInput playerInput;
    protected InputAction jumpAction, crouchAction, jetAction;
    protected ButtonControl jetControl;

    public bool IsJetWasPressd { get; set; }
    public bool IsJetWasReleased { get; set; }
    public bool IsJetting { get; set; }


    protected void Awake() {
        //new input system
        crouchAction = playerInput.currentActionMap["Crouch"];
        jumpAction = playerInput.currentActionMap["Jump"];

#if UNITY_STANDALONE
        jetAction = playerInput.currentActionMap["Jet"];
        jetControl = (ButtonControl) jetAction.controls[0];
#endif
    }


    protected void UpdateKeyEvent() {
        IsJetting = jetControl.isPressed;
        IsJetWasPressd = jetControl.wasPressedThisFrame;
        IsJetWasReleased = jetControl.wasReleasedThisFrame;
    }


    public void Enable() { playerInput.enabled = true; }


    public void Disable() { playerInput.enabled = false; }


    // protected virtual void Update() {
    //      legacy input system
    //      inputDownSpace = Input.GetKeyDown(KeyCode.Space);
    //      inputUpSpace = Input.GetKeyUp(KeyCode.Space);
    // }


    //     [SerializeField] private GUIStyle jumpUpStyle;
    //
    // #if UNITY_EDITOR
    //     private void OnGUI()
    //     {
    //         GUI.Box(new Rect(50, 50, 20, 20), "jumpUp : " + jumpAction.triggered, jumpUpStyle);
    //         GUI.Box(new Rect(50, 200, 20, 20), "jumpDown : " + crouchAction.triggered, jumpUpStyle);
    //     }
    // #endif
}