using GamepadInput;
using UnityEngine;

public class ParentSkillAndAnimationHandler : MonoBehaviour
{
    #region Fields
    private GamepadState gamepad;
    private Parent parent;
    #endregion Fields

    #region Methods
    private void Start()
    {
        parent = GetComponentInParent<Parent>();
    }

    private void Update()
    {
        gamepad = parent.myStateInfo.Controller;
        if (gamepad.XDown) { UseSkill(1); }
        if (gamepad.BUp) { parent.ResetAllRegenOrFixRates(true); parent.myStateInfo.IsControllerDisable = false; }
        else if (gamepad.B) { UseSkill(2); parent.myStateInfo.IsControllerDisable = true; }
        if (gamepad.YUp) { parent.ResetAllRegenOrFixRates(false); parent.myStateInfo.IsControllerDisable = false; }
        else if (gamepad.Y) { UseSkill(3); parent.myStateInfo.IsControllerDisable = true; }
    }
    private void LateUpdate()
    { parent.animator.SetFloat(parent.CharacterVelocityAnimator, parent.rb.velocity.magnitude);
        parent.animator.SetBool("GivingPower", parent.amIUsingAnySkill(2)||parent.amIUsingAnySkill(3));
    }

    private void UseSkill(int WhichSkill)
    {
        switch (WhichSkill)
        {
            case 1:
                parent.SkillOne();

                break;

            case 2:
                parent.SkillTwo();
                break;

            case 3:
                parent.SkillThree();
                break;

            case 4:
                parent.ReleaseChildFromHandle();
                break;
        }
    }
    #endregion Methods
}