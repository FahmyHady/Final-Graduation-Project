using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHandler : MonoBehaviour
{
    bool isStarted;
    public void NewGame()
    {
        if (!isStarted)
        {
            isStarted = true;
            GameManager.Instance.NewGame();
        }
    }
    public void TutorialLevel()
    {
        if (!isStarted)
        {
            isStarted = true;
            GameManager.Instance.TutorialLevel();
        }
    }
}
