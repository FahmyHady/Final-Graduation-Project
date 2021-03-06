﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    #region Fields
    public GameObject Door;
    public GameObject effect;
     public  Transform newPos;
    public float doorspeed;
    public bool canInteract;
    public bool interacted;
    Parent parent;
    #endregion Fields

    #region Methods
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Parent")
        {
            canInteract = true;
            parent = other.GetComponent<Parent>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        parent = null;
    }
    private void Update()
    {
        if (parent && parent.myStateInfo.Controller.XDown && canInteract)
        {
            AudioManager.Play(AudioManager.AudioItems.KeyButton, "Click");
            Open();
            AudioManager.Play(AudioManager.AudioItems.KeyButton, "Open");

        }

    }
    public void Open()
    {
        if (!interacted)
        {
            interacted = true;
            Instantiate(effect, transform.position, transform.rotation);
            StartCoroutine(DoorOpen());
        }
    }


    IEnumerator DoorOpen()
    {
        Vector3 positionToMoveTo = newPos.position;
        while (Door.transform.position != newPos.position)
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, positionToMoveTo, Time.deltaTime * doorspeed);
            yield return null;
        }
    }
    #endregion Methods
}
