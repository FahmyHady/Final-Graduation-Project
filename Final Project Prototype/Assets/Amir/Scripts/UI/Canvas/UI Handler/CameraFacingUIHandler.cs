using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingUIHandler : MonoBehaviour
{
  

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform.position);
    }
}
