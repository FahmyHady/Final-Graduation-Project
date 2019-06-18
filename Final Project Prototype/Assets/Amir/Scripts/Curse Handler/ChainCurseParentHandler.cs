using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCurseParentHandler : MonoBehaviour,ICurseHandler
{
    PlayerStateInfo info;
    [SerializeField] GameEvent @event;
    public void Curse()
    {
        info.IsControllerDisable = true;  
    }

    public void StopCurse()
    {
        info.IsControllerDisable = false;
        @event.Raise();
    }

    // Start is called before the first frame update
    void Start()
    {
        info = this.GetComponentInParent<PlayerStateInfo>();   
    }

}
