using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
public class PlayerSkillsHandler : MonoBehaviour
{
    PlayerStateInfo info;
    BaseCharacter myChar;
    GamepadState gamepad;

    void Start()
    {
        info = this.GetComponentInParent<PlayerStateInfo>();
        myChar = this.GetComponentInParent<BaseCharacter>();
        
    }

    // Update is called once per frame
    void Update()
    {

        myChar.animator.SetFloat(myChar.CharacterVelocityAnimator, myChar.rb.velocity.magnitude);
        myChar.animator.SetBool("InAir", info.IsControllerInAir);
        gamepad = GamePad.GetState((GamePad.Index)info.PlayerController);
        if (Input.GetKeyDown(KeyCode.Mouse1) )
        {
            info.IsControllerThrowing = true;

            myChar.animator.SetTrigger("Throwing");

        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            myChar.SkillTwo();

        }


    }
    void UseSkill(int WhichSkill)
    {
        switch (WhichSkill)
        {
            case 1:
                myChar.SkillOne();
             
                break;
            case 2:
                myChar.SkillTwo();
                break;
            default:
                myChar.SkillOne();
                break;
        }

    }
}
