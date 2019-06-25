using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Child me;
    bool notInRange;
   public float speed;
    private void OnEnable()
    {
        notInRange = true;

    }
    void Start()
    {
        me = GetComponent<Child>();
    }

    private void OnTriggerEnter(Collider other)
    {
        notInRange = false;
    }
    private void OnTriggerExit(Collider other)
    {
        notInRange = true;
    }
    // Update is called once per frame
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
