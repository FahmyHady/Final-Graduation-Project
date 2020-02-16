using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameplayLevelManager : MonoBehaviour
{
    static public GameplayLevelManager instance;
    public float buttonAppearSpeed;
    public Key door1Key;
    public Key door2Key;
    public Key door3Key;
    public Key door4Key;
    public int stageOneItemsCount;
    public int stageTwoItemsCount;
    public int stageThreeItemsCount;
    public int stageFourItemsCount;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera cam3;
    public CinemachineVirtualCamera cam4;
    public RoleAssignHandler assignHandler;
    bool passedLevel1;
    bool passedLevel2;
    bool passedLevel3;
    bool passedLevel4;
    int countItemsFixed;
    private void Awake()
    {
        instance = this;
        assignHandler.Assign();
    }
    private void Start()
    {
        cam1.Priority++;
    }
    public void IncreaseFixedCount()
    {
        countItemsFixed++;
        CheckLevelPassCondition();
    }

    private void CheckLevelPassCondition()
    {

        if (countItemsFixed >= stageOneItemsCount && !passedLevel1)
        {
            passedLevel1 = true;
            countItemsFixed = 0;
            cam1.Priority--;
            cam2.Priority++;
            StartCoroutine(ButtonAppear(door1Key.gameObject));
        }
        else if (countItemsFixed >= stageTwoItemsCount && !passedLevel2)
        {
            passedLevel2 = true;
            countItemsFixed = 0;
            cam2.Priority--;
            cam3.Priority++;
            StartCoroutine(ButtonAppear(door2Key.gameObject));
        }
        else if (countItemsFixed >= stageThreeItemsCount && !passedLevel3)
        {
            passedLevel3 = true;
            countItemsFixed = 0;
            cam3.Priority--;
            cam4.Priority++;
            StartCoroutine(ButtonAppear(door3Key.gameObject));
        }
        else if (countItemsFixed >= stageFourItemsCount && !passedLevel4)
        {
            passedLevel4 = true;
            countItemsFixed = 0;
            StartCoroutine(ButtonAppear(door4Key.gameObject));
        }

    }

    IEnumerator ButtonAppear(GameObject button)
    {
        Vector3 newPos = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 0.35f, button.transform.localPosition.z);
        while (button.transform.localPosition != newPos)
        {
            button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, newPos, Time.deltaTime * buttonAppearSpeed);
            yield return null;
        }
    }
}
