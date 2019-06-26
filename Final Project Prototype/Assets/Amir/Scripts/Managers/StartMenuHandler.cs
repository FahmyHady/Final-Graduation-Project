using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHandler : MonoBehaviour
{
    bool isStarted;
    public void NewGame() {
        if (!isStarted)
            GameManager.Instance.NewGame();
    }
}
