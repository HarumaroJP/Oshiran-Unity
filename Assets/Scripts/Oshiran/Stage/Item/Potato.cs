using System;
using UnityEngine;

public class Potato : ExecutableEntity
{
    [SerializeField] float onaraAmount;
    [SerializeField] AudioClip getSe;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IController>().AddOnaraAmount(onaraAmount);
            AudioSystem.Instance.PlaySe(getSe);
            gameObject.SetActive(false);
        }
    }


    public override void Rewind()
    {
        gameObject.SetActive(true);
    }
}