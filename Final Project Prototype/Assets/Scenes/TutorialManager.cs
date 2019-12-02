using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;
using Cinemachine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtualCameras;
    public GameObject lighteningSpawnEffect;
    public float buttonAppearSpeed;
    public Animator TutorialBubble;
    public Text TutorialText;
    string tempText;
    public GameObject parentPrefab;
    public GameObject AresPrefab;
    public GameObject AresObstaclePrefab;
    public GameObject AphroditePrefab;
    public GameObject AphroditeObstaclePrefab;
    public GameObject ZeusPrefab;
    public GameObject ZeusObstaclePrefab;
    PlayerStateInfo temp;

    #region Stage1
    public GameObject Stage1;
    public TutorialKey door1Key;
    bool objectSaved;
    public Interactable itemToProtect;
    public PresentationInteractable itemToFix;
    public GameObject stage1Part1;
    public GameObject stage1Part2;
    public GameObject stage1Part3;
    #endregion
    #region Stage2
    public GameObject Stage2;
    public TutorialKey door2Key;
    public Interactable itemToFix2;

    #endregion 
    #region Stage3
    public GameObject Stage3;
    public TutorialKey door3Key;
    #endregion 
    #region Stage4
    public GameObject Stage4;
    public TutorialKey door4Key;
#endregion
    MeteorMovmentController meteor;
    public GameObject meteorPrefab;
    public Parent activePlayer;
    void Start()
    {
        TutorialText.text = ArabicFixer.Fix(TutorialText.text, false, false);
        Invoke("StageOneStart", 3);
    }
    void StageOneStart()
    {
        TutorialBubble.Play("Show");
        itemToProtect.Type = InteractableType.Working;
        meteor = Instantiate(meteorPrefab, new Vector3(17, 31.5f, -18), Quaternion.identity).GetComponent<MeteorMovmentController>();
        meteor.Fire(itemToProtect.transform.position);
        Invoke("StageOneEnd", 7);
    }

    void StageOneEnd()
    {
        if (itemToProtect.Type == InteractableType.Damaged)
        {
            StageOneStart();
        }
        else
        {
            CallBubbleAnimate("عاش يا بطل");
            Invoke("StageOnePartTwoStart", 3);
        }

    }
    void StageOnePartTwoStart()
    {
        Destroy(stage1Part1);
        Instantiate(lighteningSpawnEffect, itemToFix.gameObject.transform.position, Quaternion.identity);
        CallBubbleAnimate("صاحبك بيحاول يصلح بس ضعيف لوحده, استخدم درعك عشان تساعده ");
        stage1Part2.SetActive(true);
        Invoke("StageOnePartTwoEnd", 7);

    }
    void StageOnePartTwoEnd()
    {
        if (itemToFix.Type == InteractableType.Working)
        {
            CallBubbleAnimate("تمام! ");
            Invoke("StageOnePartThreeStart", 4);
            activePlayer.gameObject.GetComponent<PlayerStateInfo>().IsControllerDisable = true;
            activePlayer.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            activePlayer.gameObject.GetComponentInChildren<ParentSkillAndAnimationHandler>().enabled=false;

        }
        else
        {

            stage1Part2.SetActive(false);
            stage1Part2.SetActive(true);
            Invoke("StageOnePartTwoEnd", 7);

        }
    }
    void StageOnePartThreeStart()
    {
        Destroy(stage1Part2);
        Instantiate(lighteningSpawnEffect, stage1Part3.gameObject.transform.position, Quaternion.identity);
        CallBubbleAnimate("مش دايما هتلحق تدمر النيزك, بس تقدر تطفى أصحابك ");
        stage1Part3.SetActive(true);
        meteor = Instantiate(meteorPrefab, new Vector3(17, 31.5f, -18), Quaternion.identity).GetComponent<MeteorMovmentController>();
        meteor.Fire(stage1Part3.transform.position);
        Invoke("StageOnePartThreeEnd", 6);
    }
    void StageOnePartThreeEnd()
    {
     

        
        if (activePlayer.gameObject.GetComponentInChildren<ParentSkillAndAnimationHandler>().enabled == false)
        {
            temp = stage1Part3.GetComponentInChildren<PlayerStateInfo>();
            activePlayer.gameObject.GetComponent<PlayerStateInfo>().IsControllerDisable = false;
            activePlayer.gameObject.GetComponentInChildren<ParentSkillAndAnimationHandler>().enabled = true;

            Invoke("StageOnePartThreeEnd", 4);
        }
        else if (!temp.IsControllerBurned && !temp.IsControllerInAir)
        {

            Destroy(stage1Part3);
            StartCoroutine(ButtonAppear(door1Key.gameObject));
            CallBubbleAnimate("عاش يا بطل, دوس على الزرار عشان تفتح الباب");
            objectSaved = true;
        }
        else
        {
            Invoke("StageOnePartThreeEnd", 4);
        }
    }
    public void StageTwoStart()
    {
        virtualCameras[1].Priority += 10;
        CallBubbleAnimate("هتحتاج تكسر الحجر و تصلح التالف ");
        Stage1.SetActive(false);
        Stage2.SetActive(true);
        Invoke("startStage2", 3);
        Invoke("StageTwoEnd", 5);
    }
    void startStage2()
    {
        objectSaved = false;
        activePlayer.gameObject.SetActive(false);
        GameObject temp = Instantiate(AresObstaclePrefab, new Vector3(31, 19.2f, 1.63f), Quaternion.identity);
        itemToFix2 = temp.GetComponentInChildren<Interactable>();
        temp.transform.localScale = Vector3.one * 0.7f;
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
        temp = Instantiate(AresPrefab, new Vector3(28.5f, 19, -3.5f), Quaternion.identity);
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
    }
    void StageTwoEnd()
    {
        if (itemToFix2.Type == InteractableType.Working)
        {
            GameObject temp = FindObjectOfType<Child>().gameObject;
            activePlayer.gameObject.transform.position = temp.transform.position;
            activePlayer.gameObject.SetActive(true);

            Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
            Destroy(temp);
            StartCoroutine(ButtonAppear(door2Key.gameObject));
            objectSaved = true;
            CallBubbleAnimate("عاش يا بطل, دوس على الزرار عشان تفتح الباب");

        }
        else
        {

            Invoke("StageTwoEnd", 1);

        }
    }
    public void StageThreeStart()
    {
        virtualCameras[2].Priority += 10;
        CallBubbleAnimate("هتحتاج ترفع الحجر الكبير و تصلح التالف ");
        Stage2.SetActive(false);
        Stage3.SetActive(true);
        Invoke("startStage3", 3);
        Invoke("StageThreeEnd", 5);
    }
    void startStage3()
    {
        objectSaved = false;
        activePlayer.gameObject.SetActive(false);
        GameObject temp = Instantiate(AphroditeObstaclePrefab, new Vector3(31, 19.2f, 23f), Quaternion.identity);
        itemToFix2 = temp.GetComponentInChildren<Interactable>();
        temp.transform.localScale = Vector3.one * 0.4f;
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
        temp = Instantiate(AphroditePrefab, new Vector3(28.5f, 19, 23-3.5f), Quaternion.identity);
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
    }

    void StageThreeEnd()
    {
        if (itemToFix2.Type == InteractableType.Working)
        {
            GameObject temp = FindObjectOfType<Child>().gameObject;
            activePlayer.gameObject.transform.position = temp.transform.position;
            activePlayer.gameObject.SetActive(true);
            Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
            Destroy(temp);
            StartCoroutine(ButtonAppear(door3Key.gameObject));
            objectSaved = true;
            CallBubbleAnimate("عاش يا بطل, دوس على الزرار عشان تفتح الباب");

        }
        else
        {

            Invoke("StageThreeEnd", 1);

        }
    }
    public void StageFourStart()
    {
        virtualCameras[3].Priority += 10;
        CallBubbleAnimate("هتحتاج تكسر البوابة و تصلح التالف ");
        Stage3.SetActive(false);
        Stage4.SetActive(true);
        Invoke("startStage4", 3);
        Invoke("StageFourEnd", 5);
    }
    void startStage4()
    {
        objectSaved = false;
        activePlayer.gameObject.SetActive(false);

        GameObject temp = Instantiate(ZeusObstaclePrefab, new Vector3(26.43f, 19.2f, 38.28f),Quaternion.Euler(0,-73.4f,0));
        itemToFix2 = temp.GetComponentInChildren<Interactable>();
        temp.transform.localScale = Vector3.one * 0.7f;
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
        temp = Instantiate(ZeusPrefab, new Vector3(28.5f, 19, 38.28f - 3.5f), Quaternion.identity);
        Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
    }
    void StageFourEnd()
    {
        if (itemToFix2.Type == InteractableType.Working)
        {
            GameObject temp = FindObjectOfType<Child>().gameObject;
            activePlayer.gameObject.transform.position = temp.transform.position;
            activePlayer.gameObject.SetActive(true);
            Instantiate(lighteningSpawnEffect, temp.transform.position, Quaternion.identity);
            Destroy(temp);
            StartCoroutine(ButtonAppear(door4Key.gameObject));
            objectSaved = true;
            CallBubbleAnimate("عاش يا بطل, دوس على الزرار عشان تنهى التمرين");

        }
        else
        {

            Invoke("StageFourEnd", 1);

        }
    }
    private void CallBubbleAnimate(string WhatToWrite)
    {
        TutorialBubble.Play("Hide");
        tempText = WhatToWrite;
        Invoke("TutorialBubbleAnimate", 1.5f);
    }

    void TutorialBubbleAnimate()
    {
        TutorialText.text = tempText;
        TutorialText.text = ArabicFixer.Fix(TutorialText.text, false, false);
        TutorialBubble.Play("Show");
    }
    void Update()
    {
        if (activePlayer.myStateInfo.Controller.XDown)
        {
            if (door1Key.canInteract && !door1Key.interacted && objectSaved)
            {
                door1Key.Open();
            }
            else if (door2Key.canInteract && !door2Key.interacted && objectSaved)
            {
                door2Key.Open();
            }
            else if (door3Key.canInteract && !door3Key.interacted && objectSaved)
            {
                door3Key.Open();
            }
            else if (door4Key.canInteract && !door4Key.interacted && objectSaved)
            {
                CallBubbleAnimate("انت دلوقتى جاهز");
                Invoke("LoadMenuScene",4);
            }
        }
    }
    void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator ButtonAppear(GameObject button)
    {
        Vector3 newPos = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 0.5f, button.transform.localPosition.z);
        while (button.transform.localPosition != newPos)
        {
            button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, newPos, Time.deltaTime * buttonAppearSpeed);
            yield return null;
        }
    }
}
