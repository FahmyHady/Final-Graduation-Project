using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraPresentationMovement : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> virtualCameras;
    int currentIndex;
    int temp;
    void Start()
    {
        currentIndex = 0;
        foreach (var item in virtualCameras)
        {
            item.Priority = 0;
        }
        virtualCameras[currentIndex].Priority = 10;
    }

    public void NextCam() {
        temp = Mathf.Clamp(currentIndex + 1, 0, virtualCameras.Count - 1);
        if (temp != currentIndex) {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }

    public void PreCam() {
        temp = Mathf.Clamp(currentIndex - 1, 0, virtualCameras.Count - 1);
        if (temp != currentIndex)
        {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }
    public void GotoCam(int index) {
        temp = Mathf.Clamp(index, 0, virtualCameras.Count - 1);
        if (temp != currentIndex && temp == index)
        {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }
}
