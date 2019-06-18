using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : BaseSkill
{
    [SerializeField]
    float delayBeforeStun;
    [SerializeField]
    float stunDuration;
    bool trapTriggered;
    List<BaseCharacter> playersInsideTrap = new List<BaseCharacter>();
    Transform parent;
    Vector3 startPos;
    private void Awake()
    {
        parent = transform.parent;
        startPos = transform.localPosition;
    }
    private void OnEnable()
    {
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform.root && !trapTriggered)
        {
                        StartCoroutine(StunStart());
        }
        playersInsideTrap.Add(other.gameObject.GetComponentInParent<BaseCharacter>());

    }
    private void OnTriggerExit(Collider other)
    {
        playersInsideTrap.Remove(other.gameObject.GetComponentInParent<BaseCharacter>());
    }
    
    
    IEnumerator StunStart()
    {
        yield return new WaitForSeconds(delayBeforeStun);
        for (int i = 0; i < playersInsideTrap.Count; i++)
        {
            playersInsideTrap[i].Stun(stunDuration);

        }
        trapTriggered = false;
        transform.parent = parent;
        transform.localPosition = startPos;
        playersInsideTrap.Clear();
        gameObject.SetActive(false);
    }

    
}
