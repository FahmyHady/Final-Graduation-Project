using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
    #region Fields
    private List<ICursed> curseHandlers;
    [TypeConstraint(typeof(ICursed))]
    [SerializeField] private List<GameObject> curses;
    private float delay;
    private float elapsedTime;
    private int lockedKeys;
    [SerializeField] private int maxCursesAtTime;
    [SerializeField] private float maxDelay;
    [SerializeField] private float minDelay;
    private int selecedCurse;
    private float time;
    [SerializeField] private float timeOffset;
    [SerializeField] private int totalCurseAmount;
    #endregion Fields

    #region Properties
    public float TimeValue => time;
    #endregion Properties

    #region Methods
    public void ReleaseKey()
    {
        lockedKeys = Mathf.Max(lockedKeys - 1, 0);
        if (lockedKeys < maxCursesAtTime && !IsInvoking(nameof(UpdateParSec))) RunUpdate();
    }

    private void HandelCursesValues()
    {
        curseHandlers?.Clear();
        curseHandlers = curses.Select(i => i.GetComponent<ICursed>()).Where(i => i.Weight <= totalCurseAmount).ToList();
        if (curseHandlers == null) curseHandlers = new List<ICursed>();
        maxCursesAtTime = Mathf.Min(maxCursesAtTime, curseHandlers.Count);
        delay = Random.Range(0, TimeValue / 4) + timeOffset;
    }

    private void HoldKay()
    {
        lockedKeys = Mathf.Min(lockedKeys + 1, maxCursesAtTime);
        if (lockedKeys >= maxCursesAtTime && IsInvoking(nameof(UpdateParSec))) StopUpdate();
    }

    private void OnValidate()
    { HandelCursesValues(); }

    private void RunUpdate()
    { InvokeRepeating(nameof(UpdateParSec), 0, 1); }

    private void Start()
    {
        curseHandlers = new List<ICursed>();
        //List<int> timeValues = RoundManager.Instance.GetRoundTime();
        time = 10;
        HandelCursesValues();
        RunUpdate();
    }
    private void StopUpdate()
    { CancelInvoke(nameof(UpdateParSec)); }

    private void UpdateParSec()
    {
        if (lockedKeys >= maxCursesAtTime || curseHandlers.Count == 0) { StopUpdate(); return; }
        elapsedTime += 1.0f;
        if (elapsedTime >= delay)
        {
            selecedCurse = Random.Range(0, curseHandlers.Count);
            if (curseHandlers[selecedCurse].IsOnceOnly && curseHandlers[selecedCurse].IsStartCurse)
            {
                elapsedTime = delay - 1;
                return;
            }
            curseHandlers[selecedCurse].StartCurse();
            totalCurseAmount -= curseHandlers[selecedCurse].Weight;
            HoldKay();
            HandelCursesValues();
            delay = Random.Range(minDelay, maxDelay);
            elapsedTime = 0.0f;
        }
    }
    #endregion Methods
}