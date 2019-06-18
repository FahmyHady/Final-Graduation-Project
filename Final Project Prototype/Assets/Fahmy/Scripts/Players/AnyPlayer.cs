using UnityEngine;

public class AnyPlayer : BaseCharacter
{
    #region Fields
    private GameObject FirstSkill, SecondSkill;
    [SerializeField] private Player myPlayer;
    private BaseSkill[] skills;
    #endregion Fields

    #region Enums
    private enum Player
    { Hera, Zeus, Aris, Aphrodite };
    #endregion Enums

    #region Methods
    public override void SkillOne() //Fire Wave
    {
        if (FirstSkill && !IsOnCoolDown(ref timeStampOne, coolDownOne)) { FirstSkill.SetActive(true); }
    }

    public override void SkillTwo()
    {
        if (SecondSkill && !IsOnCoolDown(ref timeStampTwo, coolDownTwo)) { SecondSkill.SetActive(true); }
    }

    protected override void Awake()
    {
        base.Awake();
        skills = GetComponentsInChildren<BaseSkill>(true);
        switch (skills.Length)
        {
            case 1: FirstSkill = skills[0].gameObject;  break;

            case 2:
                FirstSkill = skills[0].gameObject;
                SecondSkill = skills[1].gameObject;
                break;
            case 0: print(gameObject.name + " Has No Skills"); break;
        }

        /*switch (myPlayer)
        {
            case Player.Hera:
                break;

            case Player.Zeus:
                break;

            case Player.Aris:
                break;

            case Player.Aphrodite:
                break;
        }*/
    }
    #endregion Methods
}