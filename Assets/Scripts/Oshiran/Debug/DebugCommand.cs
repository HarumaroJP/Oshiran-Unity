using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCommand : MonoBehaviour {
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject cmdLine;
    [SerializeField] TMP_InputField cmdField;
    IController m_player;


    void Start() { m_player = GameObject.FindWithTag("Player").GetComponent<IController>(); }


    void Update() {
        if (Keyboard.current[Key.Slash].wasPressedThisFrame) {
            if (cmdLine.activeInHierarchy)
                Close();
            else
                Open();
        }
    }


    public void Open() {
        cmdLine.SetActive(true);
        cmdField.text = String.Empty;
        cmdField.ActivateInputField();
        m_player.Disable();
    }


    public void Close() {
        cmdLine.SetActive(false);
        m_player.Enable();
    }


    public void Run(TextMeshProUGUI cmdText) {
        string[] splitArr = cmdText.text.Split(' ');

        // Remove zero width space.
        splitArr = splitArr.Select(str => str.Replace(((char) 8203).ToString(), ""))
            .ToArray();

        switch (splitArr[0]) {
            case "player":
                if (splitArr.Length != 2) Debug.LogError("引数の数が不正です！");
                switch (splitArr[1]) {
                    case "stop":
                        m_player.Idle();
                        break;

                    case "start":
                        m_player.Run();
                        break;

                    default:
                        Debug.LogError("存在しない引数です！");
                        break;
                }

                break;

            case "restart":
                if (splitArr.Length != 1) Debug.LogError("引数の数が不正です！");
                m_player.Death(false);
                break;

            default:
                Debug.LogError("不正なコマンドです！");
                break;
        }

        Close();
    }
}