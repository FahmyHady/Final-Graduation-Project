﻿using GamepadInput;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    #region Fields
    private static SlotManager manager;
    [SerializeField] private GamePad.Button acceptedBtnKey;
    [SerializeField] private List<Sprite> sprite;
    [SerializeField] private List<SlotHandler> handlers;
    private List<GamePad.Index> indices;
    [SerializeField] private Button readyBtn;
    [SerializeField] private Text readyText;
    [SerializeField] private GamePad.Button rejectBtnKey;
    [SerializeField] private UINavigationHandler iNavigationHandler;
    private Queue<SlotHandler> slots;
    #endregion Fields

    #region Properties
    public static SlotManager Manager { get => manager; set => manager = value; }
    public GamePad.Button AcceptedBtnKey { get => acceptedBtnKey; set => acceptedBtnKey = value; }
    public GamePad.Button RejectBtnKey { get => rejectBtnKey; set => rejectBtnKey = value; }
    #endregion Properties

    #region Methods

    public Sprite GetNextSprite(ref int index)
    {
        index = (index + 1) % sprite.Count;
        return sprite[index];
    }

    public Sprite GetPrevSprite(ref int index)
    {
        index = (index - 1 + sprite.Count) % sprite.Count;
        return sprite[index];
    }

    public void Ready(Sprite readySprite)
    {
        var notReadySlot = handlers.Where(i => i.State != SlotState.Stady && i.State != SlotState.Ready).Select(i => i).ToList();
        foreach (var item in notReadySlot) { item.CheckSprite(readySprite); }
        var toReadySlot = handlers.Where(i => i.State == SlotState.Stady).Select(i => i).First();
        toReadySlot.Ready();
        sprite.Remove(readySprite);
        CheckReadyPlayers();
    }

    public void UnAssign(GamePad.Index index)
    {
        var freeSlot = handlers.Where(i => i.State == SlotState.Free).Select(i => i).ToList();
        foreach (var item in freeSlot)
        {
            item.ResetSlot();
            slots.Enqueue(item);
        }
        if (indices.Contains(index)) indices.Remove(index);
    }

    public void UnReady(Sprite sprite)
    {
        var unReadySlot = handlers.Where(i => i.State == SlotState.UnReady).Select(i => i).ToList();
        foreach (var item in unReadySlot) { item.UnReady(); }
        this.sprite.Add(sprite);
        CheckReadyPlayers();
    }

    private void Awake()
    { if (Manager == null) Manager = this; }

    private void CheckReadyPlayers()
    {
        var readyPlayers = handlers.Where(i => i.State == SlotState.Ready).Select(i => i).ToList();
       if (readyPlayers.Count == handlers.Count) { readyBtn.enabled = true; readyText.color = Color.white; }
       // if (readyPlayers.Count >=2) { readyBtn.enabled = true; readyText.color = Color.white; }
        else { readyBtn.enabled = false; readyText.color = Color.black; }
    }

    private void Confirm()
    { for (int i = 0; i < handlers.Count; i++) { handlers[i].ConfirmPlayer(); } }

    private void OnValidate()
    { if (acceptedBtnKey == rejectBtnKey) { rejectBtnKey++; } }

    private void Start()
    {
        indices = new List<GamePad.Index>();
        slots = new Queue<SlotHandler>();
        foreach (var i in handlers)
        {
            i.ResetSlot();
            slots.Enqueue(i);
        }
        readyBtn.onClick.AddListener(Confirm);
        readyBtn.enabled = false;
        readyText.color = Color.black;
    }

    private void Update()
    {
        if (slots.Count != 0)
        {
            if (GamePad.GetButtonDown(AcceptedBtnKey, GamePad.Index.Any))
            {
                foreach (GamePad.Index item in ((System.Enum.GetValues(typeof(GamePad.Index)))))
                {
                    if (GamePad.GetButtonDown(AcceptedBtnKey, item) && !indices.Contains(item) && item > 0)
                    {
                        //TODO
                        indices.Add(item);
                        slots.Dequeue().Assign(item);
                        break;
                    }
                }
            }
        }
        if (readyBtn.enabled)
        {
            if (GamePad.GetButtonDown(AcceptedBtnKey, GamePad.Index.Any))
            {
                iNavigationHandler.Submit();
            }
        }
        else if (slots.Count == 4)
        {
            if (GamePad.GetButtonDown(RejectBtnKey, GamePad.Index.Any))
            {
                iNavigationHandler.Back();
            }
        }
    }

    #endregion Methods
}