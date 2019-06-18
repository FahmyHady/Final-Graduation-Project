using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusingShout : BaseSkill
{
    [SerializeField]
    float radius;
    [SerializeField]
    float durationOfConfusion;


    Collider[] playersColliders;
    LayerMask playerLayerMask = 1 << 9;

    

    private void OnEnable()
    {
        playersColliders=Physics.OverlapSphere(transform.position, radius, playerLayerMask);
        for (int i = 0; i < playersColliders.Length; i++)
        {
            playersColliders[i].gameObject.GetComponentInParent<BaseCharacter>().Confuse(durationOfConfusion);
        }
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
