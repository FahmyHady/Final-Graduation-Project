using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Child me;
    bool notInRange;
    public float speed;
    public bool useSkill;
    bool skillUsed;
    private void OnEnable()
    {
        notInRange = true;
        skillUsed = false;

    }
    void Start()
    {
        me = GetComponent<Child>();
    }

    private void OnTriggerEnter(Collider other)
    {
        notInRange = false;
        if (useSkill && !skillUsed)
        {
            me.SkillOne("animation");
            skillUsed = true;
            notInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        notInRange = true;

    }
    void Update()
    {
        if (notInRange)
        {
            me.rb.velocity = transform.forward * speed;
        }
        else
        {
            me.rb.velocity = Vector3.zero;
        }
    }
}
