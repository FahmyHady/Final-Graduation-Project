using GamepadInput;
using UnityEngine;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject assignPanel;
    private AssignPanelHandler assignPanelHandler;
    private int spriteIndex;
    [SerializeField] private GamePad.Index controller;
    private bool isDone;
    public ReadyAndNotReadySpritePair notSelectedSprite;
    [SerializeField] private GameObject ReadyPanel;
    [SerializeField] private SlotState state;
    [SerializeField] private GameObject unAssignPanel;
    [SerializeField] PlayerInfo player;
    [SerializeField] Image readyBG;
  static  int savedPlayerNumber;
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
        assignPanelHandler?.SetPanelSprite(notSelectedSprite.unreadySprite);
        IsDone = false;
    }

    public void CheckSprite(Sprite readySprite)
    { if (notSelectedSprite.readySprite == readySprite) { HandleNextSprite(); } }

    public void Ready()
    {
        State = SlotState.Ready;
        assignPanel.gameObject.SetActive(false);
        readyBG.sprite = notSelectedSprite.readySprite;
        ReadyPanel.gameObject.SetActive(true);
    }

    public void ResetSlot()
    {
        State = SlotState.Unassign;
        unAssignPanel.gameObject.SetActive(true);
        assignPanel.gameObject.SetActive(false);
        spriteIndex = -1;
        HandleNextSprite();
    }

    public void UnReady()
    {
        State = SlotState.Assigned;
        assignPanel.gameObject.SetActive(true);
        ReadyPanel.gameObject.SetActive(false);
    }

    private void HandleNextSprite()
    {
        notSelectedSprite = SlotManager.Manager.GetNextSprite(ref spriteIndex);
        AudioManager.Play(AudioManager.AudioItems.MainMenu, "Hover");
        assignPanelHandler?.SetPanelSprite(notSelectedSprite.unreadySprite);
    }

    private void HandlePrevSprite()
    {
        notSelectedSprite = SlotManager.Manager.GetPrevSprite(ref spriteIndex);
        AudioManager.Play(AudioManager.AudioItems.MainMenu, "Hover");
        assignPanelHandler?.SetPanelSprite(notSelectedSprite.unreadySprite);
    }

    private void Start()
    {
        savedPlayerNumber = 0;
        assignPanelHandler = assignPanel.GetComponentInChildren<AssignPanelHandler>(); }

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
                SlotManager.Manager.UnReady(notSelectedSprite);
            }
        }
        else if (State == SlotState.Assigned)
        {
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, controller)) { HandlePrevSprite(); }
            else if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, controller)) { HandleNextSprite(); }
            else if (GamePad.GetButtonDown(SlotManager.Manager.AcceptedBtnKey, controller) && IsDone)
            {
                State = SlotState.Stady;
                AudioManager.Play(AudioManager.AudioItems.MainMenu, "Start");
                SlotManager.Manager.Ready(notSelectedSprite);
            }
            else if (!IsDone) { IsDone = true; }
        }
    }
    public void ConfirmPlayer() {
        player.Controller = controller;
        PlayerPrefs.SetInt("pickedChar/SpriteIndex"+savedPlayerNumber,(int)notSelectedSprite.thisChar);//Picked character sprite number and match it in the start of the next scene with character list
        savedPlayerNumber++;                                    //the characters in the character list have to have same order as the sprites
     //   player.Outline = outLine;
    }
    #endregion Methods
}