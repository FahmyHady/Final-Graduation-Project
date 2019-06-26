using GamepadInput;
using UnityEngine;

[RequireComponent(typeof(PlayerStateInfo))]
[ExecuteInEditMode]
public class PresentationInteractor : MonoBehaviour
{
    #region Fields
    private InteractionType actType;
    private bool canInteract;
    private GamepadState gamepad;
    private TimeHoldinghandler holdinghandler;
    private float holdTime;
    private PlayerStateInfo info;
    private PresentationInteractable interactableObj;
    [SerializeField] private bool isDoingBad;
    private bool isHold;
    private bool isLeftPress;
    private bool isLocked;
    private bool isPlaced;
    private bool isRightPress;
    private bool isTimed;
    private int patternDifficulty;
    private QTEController qTE;
    private RoleState state;
    #endregion Fields

    #region Delegates

    private delegate void RoleState();

    #endregion Delegates

    #region Properties
    public PresentationInteractable InteractableObj { get => interactableObj; set => interactableObj = value; }
    public bool IsDoingBad { get => isDoingBad; set => isDoingBad = value; }
    public bool IsHold { get => isHold; set => isHold = value; }
    public bool IsPlaced { get => isPlaced; set => isPlaced = value; }
    private bool CanInteract { get => canInteract; set => canInteract = value; }
    private bool IsTimed { get => isTimed; set => isTimed = value; }
    #endregion Properties

    #region Methods
    private void OnDisable()
    {
        if (CanInteract)
            UnHold();
        CanInteract = false;
        InteractableObj = null;
        DisableQTE();
        holdinghandler = null;
        qTE = null;
    }
    public void Enter(PresentationInteractable act)
    {
        InteractableObj = act;
        holdinghandler = act.GetComponent<TimeHoldinghandler>();
        qTE = act.GetComponent<QTEController>();
        EnableQTE();
        EnableTHT();
        CanInteract = true;
        IsTimed = InteractableObj.IsTimed;
        if (IsTimed)
        {
            holdTime = InteractableObj.HoldTime;
        }
        else
        {
            patternDifficulty = InteractableObj.Pattern;
        }
    }

    public void Exit()
    {
        if (CanInteract)
            UnHold();
        CanInteract = false;
        InteractableObj = null;
        DisableTHT();
        DisableQTE();
        holdinghandler = null;
        qTE = null;
    }

    private void AngelState()
    {
        switch (InteractableObj.Type)
        {
            case InteractableType.None:
                break;

            case InteractableType.Damaged:
               Interact(InteractionType.Good); isRightPress = true; 
              
                break;

            case InteractableType.Working:
                if (gamepad.RightShoulderDown && (!InteractableObj.HasInteracted) && !isLocked) { Interact(InteractionType.Good); isRightPress = true; }
                if (gamepad.RightShoulderUp && IsHold && isRightPress) { UnHold(); isRightPress = false; }
                break;

            case InteractableType.Shielded:
                break;
        }
    }

    private void DevilState()
    {
        switch (InteractableObj.Type)
        {
            case InteractableType.None:
                break;

            case InteractableType.Working:
                if (gamepad.LeftShoulderDwon && (!InteractableObj.HasInteracted) && !isLocked) { Interact(InteractionType.Bad); isLeftPress = true; }
                if (gamepad.LeftShoulderUp && IsHold && isLeftPress) { UnHold(); isLeftPress = false; }
                break;

            case InteractableType.Damaged:
                break;

            case InteractableType.Shielded:
                if (gamepad.LeftShoulderDwon && (!InteractableObj.HasInteracted) && !isLocked) { Interact(InteractionType.Bad); isLeftPress = true; }
                if (gamepad.LeftShoulderUp && IsHold && isLeftPress) { UnHold(); isLeftPress = false; }
                break;
        }
    }

    private void DisableQTE()
    {
        qTE.patternDone -= SuccessfulHold;
        qTE.patternMissed -= UnHold;
        qTE.timeEnd -= UnHold;
    }

    private void DisableTHT()
    { holdinghandler.timeEnd -= SuccessfulHold; }

    private void EnableQTE()
    {
        qTE.patternDone += SuccessfulHold;
        qTE.patternMissed += UnHold;
        qTE.timeEnd += UnHold;
    }

    private void EnableTHT()
    { holdinghandler.timeEnd += SuccessfulHold; }

    private void HephaestusState()
    {
        switch (InteractableObj.Type)
        {
            case InteractableType.None:
                break;

            case InteractableType.Damaged:
              Interact(InteractionType.Good); isRightPress = true;               
                break;

            case InteractableType.Working:
                break;

            case InteractableType.Shielded:
                break;
        }
    }
    
    private void HumanState()
    {
        switch (InteractableObj.Type)
        {
            case InteractableType.None:
                break;

            case InteractableType.Damaged:
               Interact(InteractionType.Good); isRightPress = true;
             
                break;

            case InteractableType.Working:
                if (gamepad.LeftShoulderDwon && (!InteractableObj.HasInteracted) & !isLocked) { Interact(InteractionType.Bad); isLeftPress = true; }
                if (gamepad.LeftShoulderUp && IsHold && isLeftPress) { UnHold(); isLeftPress = false; }
                if (gamepad.RightShoulderDown && (!InteractableObj.HasInteracted) && !isLocked) { Interact(InteractionType.Good); isRightPress = true; }
                if (gamepad.RightShoulderUp && IsHold && isRightPress) { UnHold(); isRightPress = false; }
                break;

            case InteractableType.Shielded:
                if (gamepad.LeftShoulderDwon && (!InteractableObj.HasInteracted) && !isLocked) { Interact(InteractionType.Bad); isLeftPress = true; }
                if (gamepad.LeftShoulderUp && IsHold && isLeftPress) { UnHold(); isLeftPress = false; }
                break;
        }
    }

    private void Interact(InteractionType type)
    {
        IsHold = true;
        isLocked = true;
        actType = type;
        InteractableObj.HoldPlace();
        //InteractableObj.HasInteracted = true;
        if (IsTimed) { holdinghandler.StartTime(InteractableObj.HoldTime, info); }
        else { qTE.StartQTE(InteractableObj.Pattern, (0.211f * InteractableObj.Pattern) + 1.75f, info.PlayerController); }
        info.IsFixing = true;
    }

    private void SetUpRoleState()
    {
        switch (info.Role)
        {
            case PlayerRole.None: state = null; break;
            case PlayerRole.Babysitter: break;
            case PlayerRole.Devil: state = DevilState; break;
            case PlayerRole.Hephaestus: state = HephaestusState; break;
            case PlayerRole.Angel: state = AngelState; break;
            case PlayerRole.Human: state = HumanState; break;
        }
    }

    private void Start()
    {
        info = this.GetComponent<PlayerStateInfo>();
        SetUpRoleState();
    }

    private void SuccessfulHold()
    {
        info.Score += InteractableObj.Score;
        if (actType == InteractionType.Bad) { InteractableObj.Type = InteractableObj.Type.Pre(); }
        else if (actType == InteractionType.Good) { InteractableObj.Type = InteractableObj.Type.Next(); }
        InteractableObj.FixedItemDone();
        UnHold();
    }

    private void UnHold()
    {
        isHold = false;
        isLocked = false;
        actType = InteractionType.None;
        InteractableObj.HasInteracted = false;
        InteractableObj.RelesePlace();
        info.IsFixing = false;

        if (IsTimed)
        {
            if (holdinghandler.IsStarted) holdinghandler.StopTime(info);
        }
        else
        {
            if (qTE.IsStarted) qTE.StopQTE();
        }
    }

    private void Update()
    {
        if (CanInteract)
        {
            gamepad = info.Controller;
            state?.Invoke();
        }
    }

    #endregion Methods
}