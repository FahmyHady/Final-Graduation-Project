using GamepadInput;
using UnityEngine;

public class PresentationChildrenSkillsHandlers : MonoBehaviour
{
    #region Fields
    private Child child;
    private GamepadState gamepad;
    #endregion Fields

    #region Methods
    private void LateUpdate()
    {
       
        child.animator.SetFloat(child.CharacterVelocityAnimator, child.rb.velocity.magnitude);
        child.animator.SetBool("InAir", child.myStateInfo.IsControllerInAir);
        child.animator.SetBool("Burning", child.myStateInfo.IsControllerBurned);
        child.animator.SetBool("Fixing", child.myStateInfo.IsFixing);
    }

    private void Start()
    {
        child = GetComponentInParent<Child>();
    }
    private void OnTriggerEnter(Collider other)
    {
        UseSkill(1);

    }
   
    private void UseSkill(int WhichSkill)
    {
        switch (WhichSkill)
        {
            case 1: child.SkillOne("animation"); break;
            case 2: child.SkillTwo(); break;
            case 3: child.SkillThree(); break;
            case 4:child.SkillOne("skill");break;
        }
    }
    #endregion Methods
}