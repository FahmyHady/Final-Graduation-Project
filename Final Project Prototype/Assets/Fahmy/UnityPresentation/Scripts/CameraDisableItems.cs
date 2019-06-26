using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraDisableItems : MonoBehaviour
{
   internal bool isInactive;
    CinemachineVirtualCamera me;
    public GameObject shotItems;
    public CameraPresentationMovement mycameras;
    void Start()
    {
        me = GetComponent<CinemachineVirtualCamera>();
    }

    IEnumerator TurnOff()
    {
        isInactive = true;
        yield return new WaitForSeconds(3);
        if (isInactive)
        {
            if (shotItems != mycameras.NextCamM.GetComponent<CameraDisableItems>().shotItems)
            {
                shotItems.SetActive(false);

            }
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
