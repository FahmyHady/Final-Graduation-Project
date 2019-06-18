using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : BaseCharacter
{
    #region Fields
    private bool canThrow;
    [Header("Parent Properties")]

    [SerializeField] private float fixImprovedRate;
    [SerializeField] private GameObject fixRateFieldParticleEffect;
    [SerializeField] private float grabReach;
    private Transform objectToThrow;
    private Collider objectToThrowCollider;
    private float originalFixRate;
    private float originalRegenRate;
    private Dictionary<Collider, PlayerStateInfo> playersToAid = new Dictionary<Collider, PlayerStateInfo>();
    [SerializeField] private GameObject regenFieldParticleEffect;
    [SerializeField] private float staminaRegeImprovedRate;
    private Vector3 targetLocation;
    private Vector3 targetLocationPad;
    [SerializeField] private Transform throwingHandle;

    float finalRate;
    #endregion Fields

    #region Methods

    public void ReleaseChildFromHandle()
    {
        myStateInfo.IsControllerThrowing = false;
        skillOneUsed = false;
        objectToThrow.SetParent(null);
        objectToThrowCollider.attachedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        myThrow?.ThrowSomething(objectToThrow, targetLocation);
        AudioManager.Play(AudioManager.AudioItems.Skill, "ByeByeThrown");
    }

    /// <summary>
    /// If gets false resets FixRate
    /// </summary>
    public void ResetAllRegenOrFixRates(bool ResetRegen)
    {
        switch (ResetRegen)
        {
            case true:
                regenFieldParticleEffect.SetActive(false);
                skillTwoUsed = false;
                myStateInfo.RegenRate += skillTwoStaminaCost;
                if (playersToAid.Count > 0)
                {
                    foreach (var item in playersToAid) { item.Value.RegenRate = originalRegenRate; }
                }
                break;

            case false:
                fixRateFieldParticleEffect.SetActive(false);
                skillThreeUsed = false;
                myStateInfo.RegenRate += skillThreeStaminaCost;
                if (playersToAid.Count > 0)
                {
                    foreach (var item in playersToAid) { item.Value.FixRate = originalFixRate; }
                }
                break;

        }
    }

    public void SetTargetLocation(Vector3 location, bool canUThrow = false)
    {
        targetLocationPad = location;
        canThrow = canUThrow;
    }

    public override void SkillOne()
    {

        if (!amIUsingAnySkill() && CheckStamina(skillOneStaminaCost) && !IsOnCoolDown(ref timeStampOne, coolDownOne) && !myStateInfo.IsControllerThrowing)
        {
            StartThrowAnimation();
        }
    }

    public override void SkillThree()
    {
        if (!amIUsingAnySkill() && CheckStamina(skillThreeStaminaCost) && !IsOnCoolDown(ref timeStampThree, coolDownthree))
        {
            skillThreeUsed = true;
            myStateInfo.RegenRate = -skillThreeStaminaCost;
            fixRateFieldParticleEffect.SetActive(true);
            CalculateFinalRate();
            CalculateFinalRate();
            ApplyFinalRate();
        }
    }
    void CalculateFinalRate()
    {
        finalRate = originalFixRate + (fixImprovedRate / playersToAid.Count);

    }
    void ApplyFinalRate()
    {
        if (playersToAid.Count > 0)
        {
            foreach (var item in playersToAid)
            {
                if (item.Value.RegenRate != finalRate)
                {
                    item.Value.RegenRate = finalRate;
                }
            }
        }
    }
    public override void SkillTwo()
    {
        if (!amIUsingAnySkill() && CheckStamina(skillTwoStaminaCost) && !IsOnCoolDown(ref timeStampTwo, coolDownTwo))
        {
            skillTwoUsed = true;
            myStateInfo.RegenRate = -skillTwoStaminaCost;
            regenFieldParticleEffect.SetActive(true);
            CalculateFinalRate();
            ApplyFinalRate();
        }
    }

    public void StartThrowAnimation()
    {
        objectToThrowCollider = DetectSurroundingBurningPlayers(grabReach, true);
        if (objectToThrowCollider)
        {

            objectToThrow = objectToThrowCollider.transform;
            myStateInfo.CurrentStamina -= skillOneStaminaCost;
            targetLocation = Cloud.myLocation;
            skillOneUsed = true;
            ThrowingProcedure();
            if (canThrow)
            {
                targetLocation = targetLocationPad;
                ThrowingProcedure();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            playersToAid.Add(other, other.gameObject.GetComponentInParent<PlayerStateInfo>());
            originalFixRate = playersToAid[other].FixRate;
            originalRegenRate = playersToAid[other].RegenRate;
            if (skillTwoUsed || skillThreeUsed)
            {
                CalculateFinalRate();
                ApplyFinalRate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            playersToAid[other].RegenRate = originalRegenRate;
            playersToAid[other].FixRate = originalFixRate;
            playersToAid.Remove(other);
            if (skillTwoUsed || skillThreeUsed)
            {
                CalculateFinalRate();
                ApplyFinalRate();
            }
        }
    }

    private IEnumerator RotateLerpToTarget(Vector3 target, float rotSpeed)
    {
        Vector3 newPos = target - transform.position;
        Quaternion newRot = Quaternion.LookRotation(newPos);
        while (transform.rotation != newRot)
        {
            yield return new WaitForFixedUpdate();
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, rotSpeed);
        }
    }

    private void ThrowingAnimationSnapper(Transform handle)
    {
        rb.velocity = Vector3.zero;
        objectToThrowCollider.attachedRigidbody.detectCollisions = false;
        objectToThrow.parent = handle;
        objectToThrow.localPosition = Vector3.zero;
        objectToThrow.localRotation = Quaternion.identity;
        objectToThrowCollider.attachedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void ThrowingProcedure()
    {
        transform.LookAt(objectToThrow.position);
        StartCoroutine(RotateLerpToTarget(targetLocation, 0.05f));
        myStateInfo.IsControllerThrowing = true;
        ThrowingAnimationSnapper(throwingHandle);
        animator.SetTrigger("Throwing");
    }
    #endregion Methods
}