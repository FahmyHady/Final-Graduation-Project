using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    #region Fields
    public GameObject Door;
    public GameObject lightingStrike;

    public float doorspeed;
    bool interacted = false;

    #endregion Fields

    #region Methods
    public void vanish()
    {
        Instantiate(lightingStrike, transform.position, transform.rotation);
        Instantiate(lightingStrike, Door.transform.position, Door.transform.rotation);
        interacted = true;
    }

    private void Update()
    {

        if (interacted)
        {

            Door.transform.position = Vector3.Lerp(Door.transform.position, new Vector3(Door.transform.position.x, -4.5f, Door.transform.position.z), Time.time * doorspeed);
        }
    }

    #endregion Methods
}