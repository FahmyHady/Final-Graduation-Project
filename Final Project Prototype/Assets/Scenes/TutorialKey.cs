using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKey : MonoBehaviour
{
    #region Fields
    public GameObject Door;
    public GameObject effect;
    Vector3 newPos;
    public float doorspeed;
    public bool canInteract;
    public bool interacted;
    #endregion Fields

    #region Methods
    private void Start()
    {
        newPos = new Vector3(Door.transform.localPosition.x , Door.transform.localPosition.y, Door.transform.localPosition.z - 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Parent")
        {
        canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }
    public void Open()
    {
        Instantiate(effect, transform.position, transform.rotation);
        interacted = true;
    }

    private void Update()
    {

        if (interacted)
        {

            Door.transform.localPosition = Vector3.MoveTowards(Door.transform.localPosition, newPos, Time.deltaTime * doorspeed);
        }
    }

    #endregion Methods
}
