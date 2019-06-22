using System.Collections;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    #region CharacterProperties

    #region SkillsProperties

    [Range(0, 20)]
    public float coolDownOne;

    [Range(0, 20)]
    public float coolDownthree;

    [Range(0, 20)]
    public float coolDownTwo;

   
    protected float timeStampOne;
    protected float timeStampThree;
    protected float timeStampTwo;
    [Header("Skill One")]
    [SerializeField]
    public float skillOneStaminaCost;


    [Header("Skill Two")]
    [SerializeField]
    public float skillTwoStaminaCost;

    protected bool skillOneUsed;

    protected bool skillThreeUsed;

    protected bool skillTwoUsed;

    [Header("Skill Three")]
    [SerializeField]
    public float skillThreeStaminaCost;
    #endregion SkillsProperties




    //--------------------------------------------------------------------------
    [SerializeField]
    public float speed;

    internal bool canMove;
    //If false can NOT move BUT CAN use skills
    internal bool isSlowed;

    internal bool isStunned;  //If true can NOT move NOR use skills
    [SerializeField]
    [Range(0.5f, 2)]
    protected float slowResistance;

    //--------------------------------------------------------------------------
    [SerializeField]
    private float maxSpeed;

    //The Factor by which each character resists Slow, Different than Slow Effect from the skills

    #endregion CharacterProperties

    #region Intializations
    internal Animator animator;
    internal int CharacterVelocityAnimator;
    internal PlayerMovementController myController;
    internal PlayerInteractor myInteractor;
    internal PlayerStateInfo myStateInfo;
    internal Throw myThrow;
    internal Rigidbody rb;
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

    protected virtual void Awake()
    {
        myThrow = GetComponent<Throw>();
        myStateInfo = GetComponent<PlayerStateInfo>();
        myController = GetComponent<PlayerMovementController>();
        myInteractor = GetComponent<PlayerInteractor>();
        CharacterVelocityAnimator = Animator.StringToHash("Speed");
        animator = GetComponentInChildren<Animator>();
        canMove = true;
        rb = GetComponentInParent<Rigidbody>();
        slowResistance = Mathf.Max(0.1f, slowResistance);
    }

    #endregion Intializations


    #region Methods
    public bool amIUsingAnySkill(int whichSkill = 0)
    {
        switch (whichSkill)
        {
            case 1:
                if (skillOneUsed)
                    return true;
                else return false;
            case 2:
                if (skillTwoUsed)
                    return true;
                else return false;
            case 3:
                if (skillThreeUsed)
                    return true;
                else return false;

            default:
                if (skillOneUsed || skillTwoUsed || skillThreeUsed)
                    return true;
                else
                    return false;
        }
    }

    public bool CheckStamina(float skillCost)
    {
        if (myStateInfo.CurrentStamina > skillCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PushedBack(Vector3 pushingzone, float power) //A function that throws the character backwards if needed
    {
        rb.AddExplosionForce(power, pushingzone, 0, 0);
    }
    #endregion Methods

    #region StunFunction

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        EnableorDisableMovement();
        yield return new WaitForSeconds(duration);
        isStunned = false;
        EnableorDisableMovement();
    }

    #endregion StunFunction

    #region SlowFunction

    /// <summary>
    /// Slows character for a time or until called again with value 0 to remove slow
    /// </summary>
    /// <param name="slowTime">If 100 character will be slowed until called again with value 0</param>
    /// <param name="slowFactor"></param>
    public void Slowed(float slowTime, float slowFactor)
    {
        switch (slowTime)
        {
            case 0:
                if (isSlowed)
                {
                    speed /= slowFactor * slowResistance; //if Slow time 0 removes any slow (careful not to use without slowing else it increases speed)
                    isSlowed = false;
                }
                break;

            case 100:
                if (!isSlowed)
                {
                    speed *= slowFactor * slowResistance; //if Slow time 100 Slows character until called again with value 0
                    isSlowed = true;
                }

                break;

            default:
                if (!isSlowed)
                {
                    StartCoroutine(Slow(slowTime, slowFactor)); //slows character for a given time only
                }

                break;
        }
    }

    private IEnumerator Slow(float slowTime, float slowFactor)
    {
        speed *= slowFactor * slowResistance;
        isSlowed = true;
        yield return new WaitForSeconds(slowTime);
        speed /= slowFactor * slowResistance;
        isSlowed = false;
    }

    #endregion SlowFunction

    #region ConfuseFunction

    public void Confuse(float duration)
    {
        StartCoroutine(ConfuseCoroutine(duration));
    }

    private IEnumerator ConfuseCoroutine(float duration)
    {
        myStateInfo.IsConfused = true;
        yield return new WaitForSeconds(duration);
        myStateInfo.IsConfused = false;
    }

    #endregion ConfuseFunction

    #region CoolDownCheck

    protected bool IsOnCoolDown(ref float TimeStampOfSkillUsed, float coolDown)
    {
        if (Time.time >= TimeStampOfSkillUsed)
        {
            TimeStampOfSkillUsed = Time.time + coolDown;

            return false;
        }
        else
        {
            return true;
            //OnCoolDown
        }
    }

    #endregion CoolDownCheck

    public virtual void SkillOne()
    { }

    public virtual void SkillThree()
    { }

    public virtual void SkillTwo()
    { }
    /// <summary>
    /// Returns all colliders in range including me
    /// </summary>

    #region DetectSurroundingPlayers

    public Collider[] DetectSurroundingPlayers(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 9);
        if (colliders.Length > 0)
        {
            return colliders;
        }

        return null;
    }

    /// <summary>
    /// Returns nearest collider excluding me
    /// </summary>

    public Collider DetectSurroundingBurningPlayers(float radius, bool OnlyNearestOne)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 9);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform != transform && colliders[i].gameObject.GetComponent<BaseCharacter>().myStateInfo.IsControllerBurned)
                {
                    return colliders[i];
                }
            }
        }

        return null;
    }

    #endregion DetectSurroundingPlayers
    private void EnableorDisableMovement()
    {
        myStateInfo.IsControllerDisable = !myStateInfo.IsControllerDisable;
    }
}