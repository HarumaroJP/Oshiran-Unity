using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonOpening : MonoBehaviour
{
    [SerializeField] Button easyButton;
    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;

    public void SetEnable(bool enable)
    {
        easyButton.interactable = enable;
        normalButton.interactable = enable;
        hardButton.interactable = enable;
    }
}