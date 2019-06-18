using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class ChainCurseChildHandler : MonoBehaviour,ICurseHandler
{
    PlayerStateInfo info;
    float delay;
    float elapsedTime;
    bool isStarted;
    GamepadState gamepad;
    RaycastHit hit;
    public float Delay { get => delay; set => delay = value; }
    public bool IsStarted { get => isStarted; private set => isStarted = value; }

    public void Curse()
    {
        elapsedTime = 0.0f;
        InvokeRepeating(nameof(UpdatePerSecA), 0.0f, 1.0f);
        IsStarted = true;
    }

    public void StopCurse()
    {
        if (IsInvoking(nameof(UpdatePerSecA)))
        {
            CancelInvoke(nameof(UpdatePerSecA));
        }
        else {
            info.IsControllerDisable = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        info = this.GetComponentInParent<PlayerStateInfo>();
    }

    // Update is called once per frame
    void UpdatePerSecA()
    {
        elapsedTime += 1.0f;
        if (elapsedTime >= Delay) {
            info.IsControllerDisable = true;
            CancelInvoke(nameof(UpdatePerSecA));
        }
    }
    private void FixedUpdate()
    {
        gamepad = info.Controller;
        if (gamepad.Y) {
            if (Physics.Raycast(info.transform.position,info.transform.forward, out hit, 2f)) {
                Debug.DrawRay(info.transform.position, info.transform.forward * 2, Color.red);
                if (hit.transform.parent.gameObject.tag == "Parent") {
                    StopCurse();
                    hit.transform.GetComponent<ChainCurseParentHandler>().StopCurse();
                }
                else if (hit.transform.parent.gameObject.tag == "Child") {
                    ChainCurseChildHandler childHandler = hit.transform.GetComponent<ChainCurseChildHandler>();
                    if (childHandler.isStarted) {
                        StopCurse();
                        childHandler.Curse();
                    }
                }

            }
        }
    }
}
