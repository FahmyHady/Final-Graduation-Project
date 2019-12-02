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

    float finalFixRate;
    float finalRegenRate;

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
            AudioManager.Play(AudioManager.AudioItems.Hera, "Skill3");
            CalculateFinalFixRate();
            ApplyFinalFixRate();
        }
    }
    void CalculateFinalFixRate()
    {
        finalFixRate = originalFixRate + (fixImprovedRate / playersToAid.Count);

    }
    void CalculateFinalRegenRate()
    {
        finalRegenRate = originalRegenRate + (staminaRegeImprovedRate / playersToAid.Count);

    }

    void ApplyFinalFixRate()
    {
        if (playersToAid.Count > 0)
        {
            foreach (var item in playersToAid)
            {
                if (item.Value.FixRate != finalFixRate)
                {
                    item.Value.FixRate = finalFixRate;
                }
            }
        }
    }
    void ApplyFinalRegenRate()
    {
        if (playersToAid.Count > 0)
        {
            foreach (var item in playersToAid)
            {
                if (item.Value.RegenRate != finalRegenRate)
                {
                    item.Value.RegenRate = finalRegenRate;
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
            AudioManager.Play(AudioManager.AudioItems.Hera, "Skill2");
            CalculateFinalRegenRate();
            ApplyFinalRegenRate();
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
            if (!playersToAid.ContainsKey(other))
            {
                playersToAid.Add(other, other.gameObject.GetComponentInParent<PlayerStateInfo>());
                originalFixRate = playersToAid[other].FixRate;
                originalRegenRate = playersToAid[other].RegenRate;

                if (skillTwoUsed)
                {
                    CalculateFinalRegenRate();
                    ApplyFinalRegenRate();
                }
                else if (skillThreeUsed)
                {
                    CalculateFinalFixRate();
                    ApplyFinalFixRate();
                }
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
            if (skillTwoUsed)
            {
                CalculateFinalRegenRate();
                ApplyFinalRegenRate();
            }
            else if (skillThreeUsed)
            {
                CalculateFinalFixRate();
                ApplyFinalFixRate();
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