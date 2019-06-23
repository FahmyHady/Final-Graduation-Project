using GamepadInput;
using UnityEngine;

public class PlayerStateInfo : MonoBehaviour
{
    #region Fields
    public bool IsControllerInAir;
    public bool IsControllerThrowing;
    [SerializeField] private GameEvent @event;
    [SerializeField] private GameEvent @eventPlayerChange;
    [SerializeField] private float currentStamina;
    [SerializeField] private float fixRate;
    [SerializeField] private int id;
    private bool isControllerBurned;
    private bool isControllerConfused;
    private bool isControllerDisable;
    private bool isFixing;
    [Range(0, 200)]
    [ReadOnlyWhenPlaying]
    [SerializeField] private float maxStamina;

    [SerializeField] private PlayerInfo player;
    //[SerializeField] private GamePad.Index playerController;

    [Range(0, 200)]
    [ReadOnlyWhenPlaying]
    [SerializeField] private float regenRate;

    [SerializeField] private PlayerRole role;
    [SerializeField] private float score;
    [SerializeField] private GameEvent staminaChangeEvent;
    #endregion Fields

    #region Properties

    public GamepadState Controller
    {
        get
        {
            if (IsControllerDisable || IsControllerInAir || IsControllerThrowing)
                return GetDisableContoller();
            else if (IsFixing)
                return GetFixingController();
            else if (IsControllerBurned)
                return GetBurnedContoller();
            else if (IsControllerConfused)
                return GetConfusedController();
            else
                return GetFixedContoller();
        }
    }

    public float CurrentStamina { get => currentStamina; set { currentStamina = value; staminaChangeEvent.Raise(); } }
    public float FixRate { get => fixRate; set => fixRate = value; }
    public bool IsConfused { get => IsControllerConfused; set => IsControllerConfused = value; }
    public bool IsControllerBurned { get => isControllerBurned; set => isControllerBurned = value; }
    public bool IsControllerConfused { get => isControllerConfused; set => isControllerConfused = value; }
    public bool IsFixing { get => isFixing; set => isFixing = value; }
    public float MaxStamina { get => maxStamina; }
    public PlayerInfo Player { get => player; set { player = value; @eventPlayerChange.Raise(); } }
    public int PlayerController { get => (int)Player.Controller; }
    public float RegenRate { get => regenRate; set => regenRate = value; }
    public PlayerRole Role { get => role; set => role = value; }
    public float Score { get => score; set { score = value; @event.Raise(); } }
    internal bool IsControllerDisable { get => isControllerDisable; set => isControllerDisable = value; }
    #endregion Properties

    #region Methods

    public string PlayerInfo()
    { return $"P{PlayerController}:{Score}"; }

    private GamepadState GetFixingController()
    { return GamePad.ApplyMoveDisable(GamePad.GetState(Player.Controller)); }
    private GamepadState GetBurnedContoller()
    { return GamePad.GetState(GamePad.Index.Disable); }

    private GamepadState GetConfusedController()
    { return GamePad.ApplyConfused(GamePad.GetState(player.Controller)); }

    private GamepadState GetDisableContoller()
    { return GamePad.ApplyMoveDisable(GamePad.GetState((player.Controller))); }

    private GamepadState GetFixedContoller()
    { return GamePad.GetState(Player.Controller); }

    #endregion Methods
}