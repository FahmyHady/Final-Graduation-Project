using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StunProjectile : BaseSkill
{
    Transform parent;
    [Header("Projectile Properties")]
    [SerializeField]
    [Range(1, 10)]
    float stunDuration;
    [SerializeField]
    float speed;
    [SerializeField]
    [Range(1, 5)]
    float activeDuration;
    Rigidbody myrb;
    float timeStamp;
    Vector3 myStartPos;
    Quaternion myStartQuaternion;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BaseCharacter>()?.Stun(stunDuration);

        ResetSkill();
    }

    private void Awake()
    {
        myStartPos = transform.localPosition;
        myStartQuaternion = transform.localRotation;
        myrb = GetComponent<Rigidbody>();
        parent = transform.parent;
    }
    private void OnEnable()
    {

        timeStamp = Time.time + activeDuration;
        transform.parent = null;
        myrb.AddForce(transform.forward * speed);

    }
    private void Update()
    {
        if (Time.time >= timeStamp)
        {
            ResetSkill();
        }
    }
    private void ResetSkill()
    {
        gameObject.SetActive(false);
        myrb.velocity = Vector3.zero;
        transform.parent = parent;
        gameObject.transform.localPosition = myStartPos;
        gameObject.transform.localRotation = myStartQuaternion;
    }
}
