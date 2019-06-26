using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraDisableItems : MonoBehaviour
{
    bool isInactive;
    CinemachineVirtualCamera me;
    public GameObject shotItems;
    public bool NoDisableObject;
    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<CinemachineVirtualCamera>();
    }

    IEnumerator TurnOff()
    {
        isInactive = true;
        yield return new WaitForSeconds(3);
        if (isInactive && !NoDisableObject)
        {

            shotItems.SetActive(false);
        }
    }
    void Update()
    {
        if (me.Priority > 0 && isInactive)
        {
            shotItems.SetActive(true);
            isInactive = false;
        }
        else if (me.Priority < 1 && !isInactive)
        {
            StartCoroutine(TurnOff());
        }
    }
}
