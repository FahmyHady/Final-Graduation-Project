using GamepadInput;
using UnityEngine;
using System.Collections;
public class PresentationParentSkillAndAnimationHandler : MonoBehaviour
{
    #region Fields
    private GamepadState gamepad;
    private PresentationParent parent;
    public bool isBuffing;

    #endregion Fields

    #region Methods
    private void Start()
    {
        parent = GetComponentInParent<PresentationParent>();
    }
    IEnumerator useSkill()
    {
        if (isBuffing)
        {
            yield return new WaitForSeconds(3);
            UseSkill(3);

        }
        else
        {
            yield return new WaitForSeconds(3);
            UseSkill(1);
        }
    }
    private void OnDisable()
    {
        parent.ResetAllRegenOrFixRates(false);
    }
    private void OnEnable()
    {
        if (isBuffing)
        {

            StartCoroutine(useSkill());
        }
        else
        {
            StartCoroutine(useSkill()); 
        }
    }
    private void LateUpdate()
    {
        parent.animator.SetFloat(parent.CharacterVelocityAnimator, parent.rb.velocity.magnitude);
        parent.animator.SetBool("GivingPower", parent.amIUsingAnySkill(2) || parent.amIUsingAnySkill(3));
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