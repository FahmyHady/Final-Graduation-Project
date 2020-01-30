using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class LiftObject : MonoBehaviour
{
    GamepadState gamepad;
    PlayerStateInfo info;
    PlayerInteractor interactor;
    [SerializeField] float rad;
    bool isLifting;
    public Vector3 offset;
    Collider[] colliderHits;
    Collider hitTareget;
    [SerializeField] int indexHit;
    void Start()
    {
        info = this.GetComponent<PlayerStateInfo>();
    }
    void FixedUpdate()
    {
        gamepad = info.Controller;
        if (gamepad.LeftShoulderDwon)
        {
            colliderHits = Physics.OverlapSphere(this.transform.position, rad, 512, QueryTriggerInteraction.Ignore);
            if (colliderHits.Length > 0 && !isLifting)
            {
                for (int i = 0; i < colliderHits.Length; i++)
                {
                    if (colliderHits[i].GetComponentInParent<PlayerInteractor>().IsDoingBad)
                    {
                        indexHit = i;
                        colliderHits[i].transform.parent.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
                        colliderHits[i].transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                        colliderHits[i].transform.parent.position = transform.position + offset;
                        colliderHits[i].transform.parent.SetParent(transform);
                        hitTareget = colliderHits[i];
                        isLifting = true;
                        break;
                    }
                }              
            }
        }
     
        if (gamepad.LeftShoulderUp&& isLifting)
        {
            hitTareget.transform.parent.parent = null;
            hitTareget.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
            hitTareget.transform.parent.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            isLifting = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, rad);
    }


}
