using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : BaseSkill
{
    bool explode;
    BaseCharacter myChar;
    SphereCollider me;
    [Header("Aris Fire Wave Skill", order = 2)]
    [Range(0.1f, 1)]
    public float slowEffect;
    [Range(5, 15)]
    public float radius;
    [Range(0.001f, 0.01f)]
    public float speed;
    [Range(50, 150)]
    public float power;
    [Range(1, 5)]
    public float slowTime;

    private void Awake()
    {
        myChar = GetComponentInParent<BaseCharacter>();


    }
    private void OnEnable()
    {

        myChar.rb.isKinematic = true;
        myChar.canMove = false;
        explode = true;
    }
    void FixedUpdate()
    {
        if (explode)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 15, speed);
            if (transform.localScale.x > radius)
            {
                explode = false;
                transform.localScale = new Vector3(1, 0.1f, 1);

                gameObject.SetActive(false);
                myChar.rb.isKinematic = false;
                myChar.canMove = true;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != myChar.transform)
        {

            collision.gameObject.GetComponent<BaseCharacter>()?.Slowed(slowTime, slowEffect);

        }

    }

}

