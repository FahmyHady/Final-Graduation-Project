using GamepadInput;
using UnityEngine;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject assignPanel;
    private AssignPanelHandler assignPanelHandler;
    private int colorIndex;
    [SerializeField] private GamePad.Index controller;
    private bool isDone;
    private Color outLine;
    [SerializeField] private GameObject ReadyPanel;
    [SerializeField] private SlotState state;
    [SerializeField] private GameObject unAssignPanel;
    [SerializeField] PlayerInfo player;
    [SerializeField] Image readyBG;
    #endregion Fields

    #region Properties
    public bool IsDone { get => isDone; set => isDone = value; }
    public SlotState State { get => state; set => state = value; }
    #endregion Properties

    #region Methods
    public void Assign(GamePad.Index _Controller)
    {
        controller = _Controller;
        State = SlotState.Assigned;
        unAssignPanel.gameObject.SetActive(false);
        assignPanelHandler.SetControllerName(controller.ToString());
        assignPanel.gameObject.SetActive(true);
        assignPanelHandler?.SetPanelColor(outLine);
        IsDone = false;
    }

    public void CheckColor(Color readyColor)
    { if (outLine == readyColor) { HandleNextColor(); } }

    public void Ready()
    {
        State = SlotState.Ready;
        assignPanel.gameObject.SetActive(false);
        readyBG.color = new Color(outLine.r, outLine.g, outLine.b, 0.5f);
        ReadyPanel.gameObject.SetActive(true);
    }

    public void ResetSlot()
    {
        State = SlotState.Unassign;
        unAssignPanel.gameObject.SetActive(true);
        assignPanel.gameObject.SetActive(false);
        colorIndex = -1;
        HandleNextColor();
    }

    public void UnReady()
    {
        State = SlotState.Assigned;
        assignPanel.gameObject.SetActive(true);
        ReadyPanel.gameObject.SetActive(false);
    }

    private void HandleNextColor()
    {
        outLine = SlotManager.Manager.GetNextColor(ref colorIndex);
        AudioManager.Play(AudioManager.AudioItems.MainMenu, "Hover");
        assignPanelHandler?.SetPanelColor(outLine);
    }

    private void HandlePrevColor()
    {
        outLine = SlotManager.Manager.GetPrevColor(ref colorIndex);
        AudioManager.Play(AudioManager.AudioItems.MainMenu, "Hover");
        assignPanelHandler?.SetPanelColor(outLine);
    }

    private void Start()
    { assignPanelHandler = assignPanel.GetComponentInChildren<AssignPanelHandler>(); }

    // Update is called once per frame
    private void Update()
    {
        if (GamePad.GetButtonDown(SlotManager.Manager.RejectBtnKey, controller))
        {
            if (State == SlotState.Assigned)
            {
                State = SlotState.Free;
                SlotManager.Manager.UnAssign(controller);
            }
            else if (State == SlotState.Ready)
            {
                State = SlotState.UnReady;
                SlotManager.Manager.UnReady(outLine);
            }
        }
        else if (State == SlotState.Assigned)
        {
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, controller)) { HandlePrevColor(); }
            else if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, controller)) { HandleNextColor(); }
            else if (GamePad.GetButtonDown(SlotManager.Manager.AcceptedBtnKey, controller) && IsDone)
            {
                State = SlotState.Stady;
                AudioManager.Play(AudioManager.AudioItems.MainMenu, "Start");
                SlotManager.Manager.Ready(outLine);
            }
            else if (!IsDone) { IsDone = true; }
        }
    }
    public void ConfirmPlayer() {
        player.Controller = controller;
        player.Outline = outLine;
    }
    #endregion Methods
}