using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GamepadInput;
public class CameraPresentationMovement : MonoBehaviour
{
    public List<CinemachineVirtualCamera> virtualCameras;
    public int currentIndex;
    int temp;
    bool clicked;
  public  CinemachineVirtualCamera NextCamM { get => virtualCameras[currentIndex ]; }
    void Start()
    {

        currentIndex = 0;
        foreach (var item in virtualCameras)
        {
            item.Priority = 0;
        }
        virtualCameras[currentIndex].Priority = 10;
    }

    public void NextCam()
    {
        temp = Mathf.Clamp(currentIndex + 1, 0, virtualCameras.Count - 1);
        if (temp != currentIndex)
        {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }

    public void PreCam()
    {
        temp = Mathf.Clamp(currentIndex - 1, 0, virtualCameras.Count - 1);
        if (temp != currentIndex)
        {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }
    public void GotoCam(int index)
    {
        temp = Mathf.Clamp(index, 0, virtualCameras.Count - 1);
        if (temp != currentIndex && temp == index)
        {
            virtualCameras[temp].Priority = 10;
            virtualCameras[currentIndex].Priority = 0;
            currentIndex = temp;
        }
    }
    private void Update()
    {
        AxisUsed();

    }
    void AxisUsed()
    {
        if (GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x != 0)
        {
            if (clicked == false)
            {
                if (GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x > 0)
                {
                    NextCam();
                }
                if (GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x < 0)
                {
                    PreCam();

                }
                clicked = true;
            }
        }
        if (GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any).x == 0)
        {
            clicked = false;
        }
    }
}
