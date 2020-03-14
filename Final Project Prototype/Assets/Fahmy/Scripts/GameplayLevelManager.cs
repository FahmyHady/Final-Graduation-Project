using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[System.Serializable]
public class StageAndItsFixables
{
    public int stageNumber;
    public GameObject[] stageFixables;
}
public class GameplayLevelManager : MonoBehaviour
{
    static public GameplayLevelManager instance;
    public List<StageAndItsFixables> stagesAndtheirFixables = new List<StageAndItsFixables>();
    public float buttonAppearSpeed;
    public Key door1Key;
    public Key door2Key;
    public Key door3Key;
    public Key door4Key;
    public int stageOneItemsCount;
    public int stageTwoItemsCount;
    public int stageThreeItemsCount;
    public int stageFourItemsCount;
    public Transform[] CloudAndChildrenLocationsOne;
    public Transform[] CloudAndChildrenLocationsTwo;
    public Transform[] CloudAndChildrenLocationsThree;
    public Transform[] CloudAndChildrenLocationsFour;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera cam3;
    public CinemachineVirtualCamera cam4;
    CinemachineVirtualCamera currentCam;
    public RoleAssignHandler assignHandler;
    int currentStageInLevel;
    bool passedLevel1;
    bool passedLevel2;
    bool passedLevel3;
    bool passedLevel4;
    int countItemsFixed;
    [HideInInspector] public List<GameObject> children = new List<GameObject>();
    private void Awake()
    {
        instance = this;
        assignHandler.Assign();

    }
    public void FindItemsNumbers()
    {
        StageAndItsFixables stageOneObj = new StageAndItsFixables();
        stageOneObj.stageFixables = GameObject.FindGameObjectsWithTag("Stage One");
        stageOneObj.stageNumber = 1;
        StageAndItsFixables stageTwoObj = new StageAndItsFixables();
        stageTwoObj.stageFixables = GameObject.FindGameObjectsWithTag("Stage Two");
        stageTwoObj.stageNumber = 2;
        StageAndItsFixables stageThreeObj = new StageAndItsFixables();
        stageThreeObj.stageFixables = GameObject.FindGameObjectsWithTag("Stage Three");
        stageThreeObj.stageNumber = 3;
        StageAndItsFixables stageFourObj = new StageAndItsFixables();
        stageFourObj.stageFixables = GameObject.FindGameObjectsWithTag("Stage Four");
        stageFourObj.stageNumber = 4;
        stageOneItemsCount = stageOneObj.stageFixables.Length;
        stageTwoItemsCount = stageTwoObj.stageFixables.Length;
        stageThreeItemsCount = stageThreeObj.stageFixables.Length;
        stageFourItemsCount = stageFourObj.stageFixables.Length;
        stagesAndtheirFixables.Add(stageOneObj);
        stagesAndtheirFixables.Add(stageTwoObj);
        stagesAndtheirFixables.Add(stageThreeObj);
        stagesAndtheirFixables.Add(stageFourObj);
    }
    private void Start()
    {
        cam1.Priority++;
        currentCam = cam1;
        currentStageInLevel = 1;
        for (int i = 0; i < RoleAssignHandler.Instance.infos.Count; i++)
        {
            children.Add(RoleAssignHandler.Instance.infos[i].gameObject);
        }
    }
    public void IncreaseFixedCount()
    {
        countItemsFixed++;
        CheckLevelPassCondition();
    }
    public void DecreaseFixedCount()
    {
        countItemsFixed--;
    }
    public void MoveToNextCamera(int cameraNumber)
    {
        currentCam.Priority--;
        currentStageInLevel = cameraNumber;
        switch (cameraNumber)
        {
            case 1:
                cam1.Priority++;
                currentCam = cam1;
                Cloud.Instance.MoveMeAndMyChildrenToNewLocations(CloudAndChildrenLocationsOne);

                break;

            case 2:
                cam2.Priority++;
                currentCam = cam2;
                Cloud.Instance.MoveMeAndMyChildrenToNewLocations(CloudAndChildrenLocationsTwo);
                break;
            case 3:
                cam3.Priority++;
                currentCam = cam3;
                Cloud.Instance.MoveMeAndMyChildrenToNewLocations(CloudAndChildrenLocationsThree);

                break;
            case 4:
                cam4.Priority++;
                currentCam = cam4;
                Cloud.Instance.MoveMeAndMyChildrenToNewLocations(CloudAndChildrenLocationsFour);

                break;

        }
    }
    private void CheckLevelPassCondition()
    {

        if (countItemsFixed >= stageOneItemsCount && !passedLevel1)
        {
            passedLevel1 = true;
            countItemsFixed = 0;

            StartCoroutine(ButtonAppear(door1Key.gameObject));
        }
        else if (countItemsFixed >= stageTwoItemsCount && !passedLevel2)
        {
            passedLevel2 = true;
            countItemsFixed = 0;

            StartCoroutine(ButtonAppear(door2Key.gameObject));
        }
        else if (countItemsFixed >= stageThreeItemsCount && !passedLevel3)
        {
            passedLevel3 = true;
            countItemsFixed = 0;

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
        Vector3 newPos = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 1.2f, button.transform.localPosition.z);
        while (button.transform.localPosition != newPos)
        {
            button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, newPos, Time.deltaTime * buttonAppearSpeed);
            yield return null;
        }
    }

    public GameObject GetRandomMeteorTarget()
    {
        List<GameObject> objectsToTarget = new List<GameObject>();
        objectsToTarget.AddRange(children);
        objectsToTarget.AddRange(stagesAndtheirFixables[currentStageInLevel - 1].stageFixables);
        return objectsToTarget[UnityEngine.Random.Range(0, objectsToTarget.Count)];

    }
}
