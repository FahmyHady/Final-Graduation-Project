using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene : MonoBehaviour
{
    [SerializeField] float timeToWaitBeforeEndingScene = 30;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainMenu());
    }
    IEnumerator LoadMainMenu()
    {

        yield return new WaitForSeconds(timeToWaitBeforeEndingScene);

      SceneLoader.Instance.LoadANewScene(1);
    }
}
