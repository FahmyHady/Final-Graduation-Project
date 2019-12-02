using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Trigger : MonoBehaviour
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
            manager.StageFourStart();
            Destroy(gameObject);
        }
    }
}
