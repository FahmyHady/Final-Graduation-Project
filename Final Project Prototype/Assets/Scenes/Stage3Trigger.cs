using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Trigger : MonoBehaviour
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
            manager.StageThreeStart();
            Destroy(gameObject);
        }
    }
}
