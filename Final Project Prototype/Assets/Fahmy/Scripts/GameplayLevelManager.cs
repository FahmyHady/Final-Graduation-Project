using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLevelManager : MonoBehaviour
{
    public float buttonAppearSpeed;
    public Key door1Key;
    public int stageOneItemsCount;
    int countItemsFixed;

    public void IncreaseFixedCount()
    {
        countItemsFixed++;
        CheckLevelPassCondition();
    }

    private void CheckLevelPassCondition()
    {
        if (countItemsFixed >= stageOneItemsCount)
        {
            StartCoroutine(ButtonAppear(door1Key.gameObject));
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
