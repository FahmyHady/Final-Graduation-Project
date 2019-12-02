using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Trigger : MonoBehaviour
{
    bool started;
    TutorialManager manager;
    private void Start()
    {
        manager = FindObjectOfType<TutorialManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!started)
        {
            manager.StageTwoStart();
            Destroy(gameObject);
        }
    }
}
