using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    #region Fields
    public GameObject Door;
    public GameObject effect;
    Vector3 newPos;
    public float doorspeed;
    public bool canInteract;
    public bool interacted;
    Parent parent;
    #endregion Fields

    #region Methods
    private void Start()
    {
        newPos = new Vector3(Door.transform.localPosition.x - 5, Door.transform.localPosition.y, Door.transform.localPosition.z);
    }

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
            Open();
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
        while (Door.transform.localPosition != newPos)
        {
            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, newPos, Time.deltaTime * doorspeed);
            yield return null;
        }
    }
    #endregion Methods
}
