using UnityEngine;
public enum Character { Zeus, Aris, Aphrodite };
public class Child : BaseCharacter
{
    #region Fields
    [SerializeField] protected Character SelectedChar;
    private Collider objectInFrontOfMe;
    #endregion Fields

    #region Enums
    
    #endregion Enums
    public void disableOrEnableController()
    {
        myStateInfo.IsControllerDisable = !myStateInfo.IsControllerDisable;
        rb.isKinematic = !rb.isKinematic;
    }
    #region Methods
    public void SkillOne(string animationOrskill)
    {
        switch (animationOrskill)
        {
            case "animation":
                if (objectInFrontOfMe != null)
                {
                    switch (SelectedChar)
                    {
                        case Character.Zeus:
                            if (objectInFrontOfMe && objectInFrontOfMe.tag == "Key")
                            {
                                if (!amIUsingAnySkill() && CheckStamina(skillOneStaminaCost))
                                {
                                    skillOneUsed = true;
                                    animator.SetTrigger("UsingSkill");
                                    disableOrEnableController();
                                    AudioManager.Play(AudioManager.AudioItems.Zeus, "BoltVL");

                                }
                            }
                            break;
                        case Character.Aris:
                            if (objectInFrontOfMe && objectInFrontOfMe.tag == "Boulder")
                            {
                                if (!amIUsingAnySkill() && CheckStamina(skillOneStaminaCost))
                                {
                                    skillOneUsed = true;
                                    animator.SetTrigger("UsingSkill");
                                    disableOrEnableController();
                                    AudioManager.Play(AudioManager.AudioItems.Aris, "CrashVL");

                                }
                            }
                            break;
                        case Character.Aphrodite:
                            if (objectInFrontOfMe && objectInFrontOfMe.tag == "MagicBlock")
                            {
                                if (!amIUsingAnySkill() && CheckStamina(skillOneStaminaCost))
                                {
                                    skillOneUsed = true;
                                    animator.SetTrigger("UsingSkill");
                                    disableOrEnableController();
                                    AudioManager.Play(AudioManager.AudioItems.Aphrodite, "MagicLiftVL");

                                }
                            }
                            break;
                    }

                }
                break;

            case "skill":

                switch (SelectedChar)
                {
                    case Character.Zeus:

                        skillOneUsed = false;
                        disableOrEnableController();
                        if (objectInFrontOfMe != null && objectInFrontOfMe.gameObject.GetComponent<Key>() != null)
                        {
                            objectInFrontOfMe.gameObject.GetComponent<Key>().vanish();
                           // objectInFrontOfMe.gameObject.GetComponent<Key>().vanish();
                            myStateInfo.CurrentStamina -= skillOneStaminaCost;
                            AudioManager.Play(AudioManager.AudioItems.Zeus, "Bolt");
                        }

                        break;
                    case Character.Aris:

                        skillOneUsed = false;
                        disableOrEnableController();
                        if (objectInFrontOfMe != null && objectInFrontOfMe.gameObject.GetComponent<Breaking>() != null)
                        {
                            objectInFrontOfMe?.gameObject.GetComponent<Breaking>()?.Explode();
                            myStateInfo.CurrentStamina -= skillOneStaminaCost;
                            AudioManager.Play(AudioManager.AudioItems.Aris, "Crash");
                        }
                        break;
                    case Character.Aphrodite:
                        skillOneUsed = false;
                        disableOrEnableController();
                        if (objectInFrontOfMe != null && objectInFrontOfMe.gameObject.GetComponent<Lifting>() != null)
                        {
                            objectInFrontOfMe?.gameObject.GetComponent<Lifting>()?.Floating();
                            myStateInfo.CurrentStamina -= skillOneStaminaCost;
                            AudioManager.Play(AudioManager.AudioItems.Aphrodite, "MagicLift");
                        }
                        break;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    { if (other.gameObject.layer == 13) { objectInFrontOfMe = other; } }

    private void OnTriggerExit(Collider other)
    { if (other.gameObject.layer == 13) { objectInFrontOfMe = null; } }
    #endregion Methods
}